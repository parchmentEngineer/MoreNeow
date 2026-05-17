using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoreNeow.MoreNeowCode.Powers;

namespace MoreNeow.MoreNeowCode.Cards.AltStarters;


[Pool(typeof(SilentCardPool))]
public class SilentSkill : MoreNeowCard
{
    public SilentSkill() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("ExtraBlock", 2),
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) =>
        {
            ICombatState combatState = card.CombatState;
            int cardsInHand = card.Owner.PlayerCombatState.Hand.Cards.Count(c => c != card);
            //int cardsAddingToHand = Math.Min(card.DynamicVars.Cards.IntValue, card.Owner.PlayerCombatState.DrawPile.Cards.Count(c => c != card) + card.Owner.PlayerCombatState.DiscardPile.Cards.Count(c => c != card));
            return (combatState != null ? cardsInHand + card.DynamicVars["ExtraBlock"].IntValue : 0);
        }))
    ];
    public override bool GainsBlock => true;
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        //int cardsInHand = Owner.PlayerCombatState.Hand.Cards.Count(c => c != this);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(play.Target), DynamicVars.CalculatedBlock.Props, play);
    }
    
    protected override void OnUpgrade() => DynamicVars["ExtraBlock"].UpgradeValueBy(3M);
    //protected override void OnUpgrade() => AddKeyword(CardKeyword.Sly);
}
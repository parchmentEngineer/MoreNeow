using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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

namespace MoreNeow.MoreNeowCode.Cards.Scrapped;


[Pool(typeof(DeprecatedCardPool))]
public class IroncladAttackScrapped : MoreNeowCard
{
    public IroncladAttackScrapped() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4m, ValueProp.Move),
        new RepeatVar(2),
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedVar("CalculatedHPLoss").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) =>
        {
            ICombatState combatState = card.CombatState;
            int cardsInHand = card.Owner.PlayerCombatState.Hand.Cards.Count(c => c != card);
            int cardsPlayedThisTurn = CombatManager.Instance.History.CardPlaysFinished.Count(e => e.HappenedThisTurn(combatState) && e.CardPlay.Card.Owner == card.Owner);
            return (combatState != null ? cardsPlayedThisTurn + 1 : 0);
        }))
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        await CommonActions.CardAttack(this, play.Target).WithHitCount(DynamicVars.Repeat.IntValue).Execute(choiceContext);
        await CreatureCmd.Damage(choiceContext, Owner.Creature, ((CalculatedVar)DynamicVars["CalculatedHPLoss"]).Calculate(play.Target), ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2);
}
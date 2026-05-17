using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoreNeow.MoreNeowCode.Cards.Scrapped;


[Pool(typeof(DeprecatedCardPool))]
public class NecrobinderSkillScrapped : MoreNeowCard
{
    public NecrobinderSkillScrapped() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(4m, ValueProp.Move)];

    protected override bool ShouldGlowGoldInternal => this.isExhaustPileEmpty;
    
    private bool isExhaustPileEmpty
    {
        get
        {
            //return PileType.Exhaust.GetPile(Owner).Cards.Count == 0;
            return Owner.PlayerCombatState.ExhaustPile.Cards.Count(c => c is Soul) == 0;
        }
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        if (isExhaustPileEmpty)
        {
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Soul.Create(Owner, 1, CombatState), PileType.Draw, Owner, CardPilePosition.Random));
        }
    }

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3);
}
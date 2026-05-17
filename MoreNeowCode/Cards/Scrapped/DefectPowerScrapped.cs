using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;
using MoreNeow.MoreNeowCode.Powers;

namespace MoreNeow.MoreNeowCode.Cards.Scrapped;


[Pool(typeof(DeprecatedCardPool))]
public class DefectPowerScrapped : MoreNeowCard
{
    public DefectPowerScrapped() : base(1, CardType.Power, CardRarity.Basic, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<IcebergPower>(3)];
    //protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BrawlPower>()];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await OrbCmd.Channel<FrostOrb>(choiceContext, Owner);
        await PowerCmd.Apply<IcebergPower>(choiceContext, Owner.Creature, DynamicVars["IcebergPower"].BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
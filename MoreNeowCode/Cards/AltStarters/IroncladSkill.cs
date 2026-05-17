using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoreNeow.MoreNeowCode.Powers;

namespace MoreNeow.MoreNeowCode.Cards.AltStarters;


[Pool(typeof(IroncladCardPool))]
public class IroncladSkill : MoreNeowCard
{
    public IroncladSkill() : base(2, CardType.Skill, CardRarity.Basic, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3m, ValueProp.Move), new PowerVar<BrawlPower>(1)];
    //protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BrawlPower>()];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await PowerCmd.Apply<BrawlPower>(choiceContext, Owner.Creature, DynamicVars["BrawlPower"].BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(4);
    //protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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
public class IroncladSkillScrapped : MoreNeowCard
{
    public IroncladSkillScrapped() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3m, ValueProp.Move)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }
    
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3);
}
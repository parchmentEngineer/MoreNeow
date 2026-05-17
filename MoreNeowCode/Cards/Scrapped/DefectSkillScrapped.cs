using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace MoreNeow.MoreNeowCode.Cards.Scrapped;


[Pool(typeof(DeprecatedCardPool))]
public class DefectSkillScrapped : MoreNeowCard
{
    public DefectSkillScrapped() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self) { }
    //protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(4m, ValueProp.Move), new PowerVar<BrawlPower>(1)];
    //protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BrawlPower>()];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner.PlayerCombatState.OrbQueue.Orbs.Count <= 0)
            return;
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        OrbModel orb = Owner.PlayerCombatState.OrbQueue.Orbs.First();
        await OrbCmd.EvokeNext(choiceContext, Owner);
        await OrbCmd.Channel(choiceContext, orb, Owner);
    }
    
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
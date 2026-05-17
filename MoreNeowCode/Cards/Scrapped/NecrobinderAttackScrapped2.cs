using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MoreNeow.MoreNeowCode.Cards.Scrapped;


[Pool(typeof(DeprecatedCardPool))]
public class NecrobinderAttackScrapped2 : MoreNeowCard
{
    public NecrobinderAttackScrapped2() : base(1, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DoomPower>(8)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DoomPower>()];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<DoomPower>(choiceContext, play.Target, DynamicVars.Doom.BaseValue, Owner.Creature, this);
        CardCmd.ApplyKeyword(this, CardKeyword.Ethereal);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Doom.UpgradeValueBy(3m);
    }
}
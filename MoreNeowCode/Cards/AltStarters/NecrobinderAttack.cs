using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoreNeow.MoreNeowCode.Cards.Scrapped;


[Pool(typeof(NecrobinderCardPool))]
public class NecrobinderAttack : MoreNeowCard
{
    public NecrobinderAttack() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new OstyDamageVar(6M, ValueProp.Move), new DynamicVar("HealthTarget", 10)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Soul>()];
    
    protected override bool ShouldGlowRedInternal => this.Owner.IsOstyMissing;
    
    protected override bool ShouldGlowGoldInternal => this.DoesOstyHaveHP;
    
    private bool DoesOstyHaveHP {
        get
        {
            Creature osty = this.Owner.Osty;
            int ostyHP = (osty == null || !osty.IsAlive ? 0 : osty.CurrentHp);
            return ostyHP >= 10;
        }
        
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        if (Osty.CheckMissingWithAnim(Owner))
            return;
        await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        if (DoesOstyHaveHP)
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Soul.Create(Owner, 1, CombatState), PileType.Draw, Owner, CardPilePosition.Random));
    }

    protected override void OnUpgrade() => DynamicVars.OstyDamage.UpgradeValueBy(3);
}
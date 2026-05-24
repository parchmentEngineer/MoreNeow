using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoreNeow.MoreNeowCode.Cards.Scrapped;


[Pool(typeof(DeprecatedCardPool))]
public class DefectAttackScrapped : MoreNeowCard
{
    public DefectAttackScrapped() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(5m, ValueProp.Move),
    ];

    //protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];
    
    private bool _evokedThisTurn;
    protected override bool ShouldGlowGoldInternal => this.HasEvokedThisTurn;
    
    private bool HasEvokedThisTurn
    {
        get
        {
            return EvokedThisTurn;
        }
    }
    
    
    private bool EvokedThisTurn
    {
        get => this._evokedThisTurn;
        set
        {
            this.AssertMutable();
            this._evokedThisTurn = value;
        }
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int hitCount = HasEvokedThisTurn ? 2 : 1;
        await CommonActions.CardAttack(this, play.Target).WithHitCount(hitCount).Execute(choiceContext);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Dazed>(Owner), PileType.Discard, Owner));
        await Cmd.Wait(0.5f);
    }
    
    public override Task AfterOrbEvoked(PlayerChoiceContext choiceContext, OrbModel orb,
        IEnumerable<Creature> targets)
    {
        if (orb.Owner != Owner)
            return Task.CompletedTask;
        EvokedThisTurn = true;
        return Task.CompletedTask;

    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == this.Owner.Creature.Side)
            EvokedThisTurn = false;
        return Task.CompletedTask;
    }
    
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2M);
}
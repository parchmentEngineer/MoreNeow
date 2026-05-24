using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
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
public class IroncladAttack : MoreNeowCard
{
    public IroncladAttack() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(5m, ValueProp.Move),
        new PowerVar<VigorPower>(5)
    ];

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];
    
    private Decimal _attacksPlayedThisTurn;
    protected override bool ShouldGlowGoldInternal => this.isThisThirdAttack;
    
    private bool isThisThirdAttack
    {
        get
        {
            return AttacksPlayedThisTurn == 2;
        }
    }
    
    
    private Decimal AttacksPlayedThisTurn
    {
        get => this._attacksPlayedThisTurn;
        set
        {
            this.AssertMutable();
            this._attacksPlayedThisTurn = value;
        }
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        if (isThisThirdAttack)
            await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars["VigorPower"].BaseValue, Owner.Creature, this);
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != this.Owner || cardPlay.Card.Type != CardType.Attack)
            return Task.CompletedTask;
        AttacksPlayedThisTurn += 1;
        return Task.CompletedTask;
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == this.Owner.Creature.Side)
            AttacksPlayedThisTurn = 0;
        return Task.CompletedTask;
    }

    /*public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == this.Owner.Creature.Side)
            AttacksPlayedThisTurn = 0;
        return Task.CompletedTask;
    }*/
    
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3M);
}
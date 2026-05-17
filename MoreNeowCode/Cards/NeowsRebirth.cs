using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MoreNeow.MoreNeowCode.Powers;

namespace MoreNeow.MoreNeowCode.Cards;


[Pool(typeof(ColorlessCardPool))]
public class NeowsRebirth : MoreNeowCard
{
    public NeowsRebirth() : base(3, CardType.Power, CardRarity.Ancient, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Combats", 3M)];
    
    private int _combatsSeen;
    
    [SavedProperty]
    public int CombatsSeen
    {
        get => this._combatsSeen;
        set
        {
            this.AssertMutable();
            this._combatsSeen = value;
            this.DynamicVars["Combats"].BaseValue = (Decimal) (3 - this.CombatsSeen);
        }
    }

    public override async Task AfterCombatEnd(CombatRoom _)
    {
        CardPile pile = Pile;
        if ((pile != null ? (pile.Type != PileType.Deck ? 1 : 0) : 1) != 0)
            return;
        CombatsSeen++;
        if (CombatsSeen < 3 || Pile.Type != PileType.Deck)
            return;
        await CardPileCmd.RemoveFromDeck(this);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay); 
        //await PowerCmd.Apply<NeowsRebirthPower>(Owner.Creature, 1M, Owner.Creature, this);
        await PowerCmd.Apply<NeowsRebirthPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-2);
}
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.sts2.Core.Nodes.TopBar;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MoreNeow.MoreNeowCode.Relics.Complex;

[Pool(typeof(EventRelicPool))]
public class RegalScepter : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new EnergyVar(2)];
    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        RunManager.Instance.GenerateRooms();
        await RunManager.Instance.GenerateMap();
        //NTopBarBossIcon.RefreshBossIcon();
        //return Task.CompletedTask;
    }
    
    public override Decimal ModifyHandDraw(Player player, Decimal count)
    {
        if (player != this.Owner || player.Creature.CombatState.RoundNumber > 1)
            return count;
        AbstractRoom currentRoom = player.RunState.CurrentRoom;
        return (currentRoom != null ? (currentRoom.RoomType != RoomType.Boss ? 1 : 0) : 1) != 0 ? count : count + (Decimal) this.DynamicVars.Cards.IntValue;
    }
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != Owner.Creature.Side || combatState.RoundNumber > 1)
            return;
        AbstractRoom currentRoom = combatState.RunState.CurrentRoom;
        if ((currentRoom != null ? (currentRoom.RoomType != RoomType.Boss ? 1 : 0) : 1) != 0)
            return;
        Flash();
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
}
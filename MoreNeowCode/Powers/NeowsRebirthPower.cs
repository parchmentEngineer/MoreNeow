using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MoreNeow.MoreNeowCode.Powers;


public class NeowsRebirthPower : MoreNeowPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterCombatEnd(CombatRoom room)
    {
        for (int index = 0; index < this.Amount; ++index)
        {
            room.AddExtraReward(Owner.Player, new CardRemovalReward(Owner.Player));
            room.AddExtraReward(Owner.Player, new CardReward(CardCreationOptions.ForRoom(Owner.Player, RoomType.Monster), 3, Owner.Player));
        }
    return Task.CompletedTask;
    }
    
}
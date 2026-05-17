using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MoreNeow.MoreNeowCode.Relics.Simple;


[Pool(typeof(EventRelicPool))]
public class HuntersGuide : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private int _activeAct = -1;
    [SavedProperty]
    public int ActiveAct
    {
        get => this._activeAct;
        set
        {
            this.AssertMutable();
            this._activeAct = value;
        }
    }
    
    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != this.Owner || (room != null ? (room.RoomType != RoomType.Elite ? 1 : 0) : 1) != 0)
            return false;
        if (ActiveAct != Owner.RunState.CurrentActIndex)
            return false;
        CardCreationOptions options1 = new CardCreationOptions([ModelDb.CardPool<ColorlessCardPool>()], CardCreationSource.Other, CardRarityOddsType.RegularEncounter);
        rewards.Add(new CardReward(options1, 3, player));
        return true;
    }
    
    public override Task AfterObtained()
    {
        this.ActiveAct = this.Owner.RunState.CurrentActIndex;
        return Task.CompletedTask;
    }
    
    public override Task AfterRoomEntered(AbstractRoom _)
    {
        this.Status = this.ActiveAct == this.Owner.RunState.CurrentActIndex ? RelicStatus.Normal : RelicStatus.Disabled;
        return Task.CompletedTask;
    }
    
}
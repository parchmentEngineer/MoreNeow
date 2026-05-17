using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MoreNeow.MoreNeowCode.Relics.Complex;


[Pool(typeof(EventRelicPool))]
public class IronCrown : MoreNeowRelic
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
    
    public override bool TryModifyCardRewardOptions(
        Player player,
        List<CardCreationResult> options,
        CardCreationOptions creationOptions)
    {
        if (this.Owner != player || creationOptions.Source != CardCreationSource.Encounter)
            return false;
        if (ActiveAct == Owner.RunState.CurrentActIndex)
        {
            IEnumerable<CardModel> cardModels = creationOptions.GetPossibleCards(player).Where(c => options.TrueForAll((Predicate<CardCreationResult>) (o => o.originalCard.Id != c.Id)));
            if (!cardModels.Any())
                cardModels = creationOptions.GetPossibleCards(player).Where(c => true);
            if (!cardModels.Any())
                return false;
            CardModel card = CardFactory.CreateForReward(this.Owner, 1, new CardCreationOptions(cardModels, CardCreationSource.Other, creationOptions.RarityOdds).WithFlags(CardCreationFlags.NoModifyHooks | CardCreationFlags.NoCardPoolModifications)).FirstOrDefault()?.Card;
            if (card != null)
            {
                CardCreationResult cardCreationResult = new CardCreationResult(card);
                //cardCreationResult.ModifyCard(card);
                options.Add(cardCreationResult);
            }
            return card != null;
        }
        else
        {
            /*if (options.Count <= 0)
                return false;
            if (options.Count == 1)
            {
                options.Clear();
            }
            else
            {
                options.RemoveRange(0, 2);
            }
            */
            return true;
        }
    }
    
    public override Task AfterObtained()
    {
        this.ActiveAct = this.Owner.RunState.CurrentActIndex;
        return Task.CompletedTask;
    }

    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (this.Owner != player || ActiveAct != Owner.RunState.CurrentActIndex)
            return false;
        List<Reward> toRemove = new();
        foreach (Reward reward in rewards)
        {
            if (reward.GetType() == typeof(GoldReward))
            {
                toRemove.Add(reward);
            }
        }
        foreach (Reward reward in toRemove)
        {
            if (reward.GetType() == typeof(GoldReward))
            {
                rewards.Remove(reward);
            }
        }
        return true;
    }

    public override Task AfterRoomEntered(AbstractRoom _)
    {
        //this.Status = this.ActiveAct == this.Owner.RunState.CurrentActIndex ? RelicStatus.Normal : RelicStatus.Disabled;
        return Task.CompletedTask;
    }
    
}
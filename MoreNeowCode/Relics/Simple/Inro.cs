using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MoreNeow.MoreNeowCode.Relics.Simple;


[Pool(typeof(EventRelicPool))]
public class Inro : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Adroit>(3);
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    public override bool HasUponPickupEffect => true;

    private int _timesUsed;
    
    [SavedProperty]
    public int TimesUsed
    {
        get => this._timesUsed;
        set
        {
            this.AssertMutable();
            this._timesUsed = value;
        }
    }
    
    public override bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewards, CardCreationOptions options)
    {
        if (player != this.Owner || this.TimesUsed >= this.DynamicVars.Cards.IntValue)
            return false;
        Adroit canonicalSlither = ModelDb.Enchantment<Adroit>();
        List<CardCreationResult> list = cardRewards.Where(r => canonicalSlither.CanEnchant(r.Card)).ToList();
        if (list.Count == 0)
            return false;
        foreach (CardCreationResult cardResult in list)
        {
            CardModel card = Owner.RunState.CloneCard(cardResult.Card);
            CardCmd.Enchant<Adroit>(card, 3);
            cardResult.ModifyCard(card, this);
        }
        /*CardCreationResult? cardCreationResult = Owner.RunState.Rng.Niche.NextItem(list);
        if (cardCreationResult == null)
            return false;
        CardModel card = Owner.RunState.CloneCard(cardCreationResult.Card);
        CardCmd.Enchant<Slither>(card, 1);
        cardCreationResult.ModifyCard(card, this);*/
        return true;
    }
    
    public override Task AfterModifyingCardRewardOptions()
    {
        if (this.TimesUsed >= this.DynamicVars.Cards.IntValue)
            return Task.CompletedTask;
        ++this.TimesUsed;
        return Task.CompletedTask;
    }
    
    public override async Task AfterObtained()
    {
        CardCreationOptions options1 = new CardCreationOptions([Owner.Character.CardPool], CardCreationSource.Other, CardRarityOddsType.Uniform, (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Common)).WithFlags(CardCreationFlags.NoUpgradeRoll);
        List<CardModel> list = CardFactory.CreateForReward(Owner, 3, options1).Select((Func<CardCreationResult, CardModel>) (r => r.Card)).ToList();
        CardModel card = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), (IReadOnlyList<CardModel>) list, Owner, true);
        if (list.Count <= 0)
            return;
        //CardModel card = list[0];
        if (card != null)
        {
            
            /*if (ModelDb.Enchantment<Adroit>().CanEnchant(card))
            {
                CardCmd.Enchant<Adroit>(card, 3M);
                //NRun? instance = NRun.Instance;
                //if (instance != null)
                    //instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node) NCardEnchantVfx.Create(card)!);
            }*/
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck));
        }
        //return Task.CompletedTask;
    }
}
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MoreNeow.MoreNeowCode.Relics.Simple;


[Pool(typeof(EventRelicPool))]
public class WindChimes : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Swift>(1);
    public override bool HasUponPickupEffect => true;

    public override Task AfterObtained()
    {
        
        List<CardModel> toEnchant = [];
        int strikesToFind = 2;
        int defendsToFind = 2;
        foreach (CardModel card in (IEnumerable<CardModel>)PileType.Deck.GetPile(this.Owner).Cards.ToList<CardModel>())
        {
            if (card.Rarity == CardRarity.Basic && ModelDb.Enchantment<Swift>().CanEnchant(card))
            {
                if (card.Tags.Contains(CardTag.Strike) && strikesToFind > 0)
                {
                    strikesToFind--;
                    toEnchant.Add(card);
                }
                if (card.Tags.Contains(CardTag.Defend) && defendsToFind > 0)
                {
                    defendsToFind--;
                    toEnchant.Add(card);
                }
            }
        }

        foreach (CardModel card in toEnchant)
        {
            CardCmd.Enchant<Swift>(card, 1M);
            NRun? instance = NRun.Instance;
            if (instance != null)
                instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node) NCardEnchantVfx.Create(card)!);
        }
        /*
        List<CardModel> deck = PileType.Deck.GetPile(Owner).Cards.Where(c => c.Rarity == CardRarity.Basic).ToList();
        CardModel? strike = deck.FirstOrDefault(c => c.Tags.Contains(CardTag.Strike));
        CardModel? defend = deck.FirstOrDefault(c => c.Tags.Contains(CardTag.Defend));

        if (strike != null)
        {
            if (ModelDb.Enchantment<Swift>().CanEnchant(strike))
            {
                CardCmd.Enchant<Swift>(strike, 1M);
                NRun? instance = NRun.Instance;
                if (instance != null)
                    instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node) NCardEnchantVfx.Create(strike)!);
            }
        }
        
        if (defend != null)
        {
            if (ModelDb.Enchantment<Swift>().CanEnchant(defend))
            {
                CardCmd.Enchant<Swift>(defend, 1M);
                NRun? instance = NRun.Instance;
                if (instance != null)
                    instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node) NCardEnchantVfx.Create(defend)!);
            }
        }*/
        return Task.CompletedTask;
    }
}
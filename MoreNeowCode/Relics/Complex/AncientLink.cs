using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MoreNeow.MoreNeowCode.Relics.Complex;


[Pool(typeof(EventRelicPool))]
public class AncientLink : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..HoverTipFactory.FromEnchantment<TezcatarasEmber>(), ..HoverTipFactory.FromEnchantment<Goopy>()];
    //HoverTipFactory.FromCard<Clumsy>()
    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        List<CardModel> deck = PileType.Deck.GetPile(Owner).Cards.Where(c => c.Rarity == CardRarity.Basic).ToList();
        CardModel? strike = deck.FirstOrDefault(c => c.Tags.Contains(CardTag.Strike));
        CardModel? defend = deck.FirstOrDefault(c => c.Tags.Contains(CardTag.Defend));

        if (strike != null)
        {
            if (ModelDb.Enchantment<TezcatarasEmber>().CanEnchant(strike))
            {
                CardCmd.Enchant<TezcatarasEmber>(strike, 1M);
                NRun? instance = NRun.Instance;
                if (instance != null)
                    instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node) NCardEnchantVfx.Create(strike)!);
            }
        }
        
        if (defend != null)
        {
            if (ModelDb.Enchantment<Goopy>().CanEnchant(defend))
            {
                CardCmd.Enchant<Goopy>(defend, 1M);
                NRun? instance = NRun.Instance;
                if (instance != null)
                    instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node) NCardEnchantVfx.Create(defend)!);
            }
        }
        //await CardPileCmd.AddCurseToDeck<Clumsy>(Owner);
        //return Task.CompletedTask;
    }
    
    public override Decimal ModifyMerchantPrice(Player player, MerchantEntry entry, Decimal originalPrice)
    {
        if (entry is MerchantCardRemovalEntry)
        {
            return player != Owner || !LocalContext.IsMe(Owner) ? originalPrice : originalPrice * (150M / 100M);
        }

        return originalPrice;
    }
}
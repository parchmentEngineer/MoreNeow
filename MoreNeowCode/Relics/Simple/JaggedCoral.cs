using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MoreNeow.MoreNeowCode.Enchantments;
using MoreNeow.MoreNeowCode.Extensions;

namespace MoreNeow.MoreNeowCode.Relics.Simple;


[Pool(typeof(EventRelicPool))]
public class JaggedCoral : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Serrated>(1);
    public override bool HasUponPickupEffect => true;

    public override Task AfterObtained()
    {
        
        List<CardModel> toEnchant = [];
        int strikesToFind = 1;
        foreach (CardModel card in (IEnumerable<CardModel>)PileType.Deck.GetPile(this.Owner).Cards.ToList<CardModel>())
        {
            if (card.Rarity == CardRarity.Basic && ModelDb.Enchantment<Serrated>().CanEnchant(card))
            {
                if (card.Tags.Contains(CardTag.Strike) && strikesToFind > 0)
                {
                    strikesToFind--;
                    toEnchant.Add(card);
                }
            }
        }

        foreach (CardModel card in toEnchant)
        {
            CardCmd.Enchant<Serrated>(card, 1M);
            NRun? instance = NRun.Instance;
            if (instance != null)
                instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node) NCardEnchantVfx.Create(card)!);
        }
        return Task.CompletedTask;
    }
    
    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (player != this.Owner)
            return false;
        options.Add(new PropagateRestSiteOption(player));
        return true;
    }
}
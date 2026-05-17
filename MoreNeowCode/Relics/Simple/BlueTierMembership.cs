using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace MoreNeow.MoreNeowCode.Relics.Simple;

[Pool(typeof(EventRelicPool))]
public class BlueTierMembership : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    //protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Apparition>()];
    //protected override IEnumerable<DynamicVar> CanonicalVars => [new MaxHpVar(18)];

    public override Decimal ModifyMerchantPrice(Player player, MerchantEntry entry, Decimal originalPrice)
    {
        if (entry is MerchantPotionEntry)
        {
            return player != Owner || !LocalContext.IsMe(Owner) ? originalPrice : originalPrice * (20M / 100M);
        }

        return originalPrice;
    }
}
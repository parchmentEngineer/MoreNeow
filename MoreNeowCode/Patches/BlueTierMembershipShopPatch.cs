using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MoreNeow.MoreNeowCode.Relics.Simple;

namespace MoreNeow.MoreNeowCode.Patches;

[HarmonyPatch(typeof(MerchantInventory), "PopulatePotionEntries")]
public class BlueTierMembershipShopPatch
{
    
    public static void Postfix(MerchantInventory __instance)
    {
        bool hasBlueTierMembership = false;
        foreach (RelicModel relic in __instance.Player.Relics)
        {
            if (relic.Id == ModelDb.GetId<BlueTierMembership>())
            {
                hasBlueTierMembership = true;
            }
        }

        if (!hasBlueTierMembership)
            return;
        Rng rng = __instance.Player.PlayerRng.Shops;
        IEnumerable<PotionModel> list = PotionFactory.GetPotionOptions(__instance.Player, Array.Empty<PotionModel>());
        PotionModel potion = rng.NextItem(list.Where(potion => potion.Rarity == PotionRarity.Rare));
        __instance._potionEntries[0] = new MerchantPotionEntry(potion.ToMutable(), __instance.Player);
    }
}
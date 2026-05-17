using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;

namespace MoreNeow.MoreNeowCode.Patches;
[HarmonyPatch(typeof(RestSiteOption))]
[HarmonyPatch("IconPath", MethodType.Getter)]
public class RestSiteImagePatch
{
    public static void Postfix(RestSiteOption __instance, ref string __result)
    {
        if (__instance.OptionId == "FEAST")
        {
            __result = Path.Join(MainFile.ModId, "images", "ui", "option_feast.png");
        }
        if (__instance.OptionId == "PROPAGATE")
        {
            __result = Path.Join(MainFile.ModId, "images", "ui", "option_propagate.png");
        }
    }
}
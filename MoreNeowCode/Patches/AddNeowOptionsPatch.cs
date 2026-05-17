using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MoreNeow.MoreNeowCode.Relics.Complex;
using MoreNeow.MoreNeowCode.Relics.Simple;

namespace MoreNeow.MoreNeowCode.Patches;

[HarmonyPatch(typeof(Neow), "PositiveOptions", MethodType.Getter)]
public class AddPositiveNeowOptionsPatch
{
    public static void Postfix(Neow __instance, ref IEnumerable<EventOption> __result)
    {
        if (__instance is not Neow neow)
            return;
        List<EventOption> options = __result.ToList();
        options.Add(RelicOption<BlueTierMembership>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        options.Add(RelicOption<HuntersGuide>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        options.Add(RelicOption<Inro>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        options.Add(RelicOption<ShiftingBlade>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        options.Add(RelicOption<WindChimes>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        options.Add(RelicOption<WrigglingInk>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        options.Add(RelicOption<PicnicBasket>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        options.Add(RelicOption<JaggedCoral>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        __result = options;
    }
    
    protected static EventOption RelicOption<T>(string pageName = "INITIAL", string? customDonePage = null, Neow neow = null) where T : RelicModel
    {
        return RelicOption(ModelDb.Relic<T>().ToMutable(), pageName, neow: neow);
    }

    protected static EventOption RelicOption(RelicModel relic, string pageName = "INITIAL", string? customDonePage = null, Neow neow = null)
    {
        relic.AssertMutable();
        relic.Owner = neow.Owner;

        string textKey = $"{StringHelper.Slugify(neow.GetType().Name)}.pages.{pageName}.options.{relic.Id.Entry}";
        //string textKey = neow.OptionKey(pageName, relic.Id.Entry);
        return EventOption.FromRelic(relic, neow, OnChosen, textKey);

        async Task OnChosen()
        {
            RelicModel relicModel = await RelicCmd.Obtain(relic, neow.Owner);
            PropertyInfo customDonePageProp = typeof(AncientEventModel).GetProperty("CustomDonePage",
                BindingFlags.NonPublic | BindingFlags.Instance);
            customDonePageProp.SetValue(neow, "NEOW.pages.DONE.POSITIVE.description");
        
            MethodInfo doneMethod = typeof(AncientEventModel).GetMethod("Done",
                BindingFlags.NonPublic | BindingFlags.Instance);
            doneMethod.Invoke(neow, null);

        }
    }
}

[HarmonyPatch(typeof(Neow), "CurseOptions", MethodType.Getter)]
public class AddCursedNeowOptionsPatch
{
    public static void Postfix(Neow __instance, ref IEnumerable<EventOption> __result)
    {
        if (__instance is not Neow neow)
            return;
        List<EventOption> options = __result.ToList();
        options.Add(RelicOption<AncientLink>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        options.Add(RelicOption<SpectersGrin>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        options.Add(RelicOption<IronCrown>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        __result = options;
    }
    
    protected static EventOption RelicOption<T>(string pageName = "INITIAL", string? customDonePage = null, Neow neow = null) where T : RelicModel
    {
        return RelicOption(ModelDb.Relic<T>().ToMutable(), pageName, neow: neow);
    }

    protected static EventOption RelicOption(RelicModel relic, string pageName = "INITIAL", string? customDonePage = null, Neow neow = null)
    {
        relic.AssertMutable();
        relic.Owner = neow.Owner;

        string textKey = $"{StringHelper.Slugify(neow.GetType().Name)}.pages.{pageName}.options.{relic.Id.Entry}";
        //string textKey = neow.OptionKey(pageName, relic.Id.Entry);
        return EventOption.FromRelic(relic, neow, OnChosen, textKey);

        async Task OnChosen()
        {
            RelicModel relicModel = await RelicCmd.Obtain(relic, neow.Owner);
            PropertyInfo customDonePageProp = typeof(AncientEventModel).GetProperty("CustomDonePage",
                BindingFlags.NonPublic | BindingFlags.Instance);
            customDonePageProp.SetValue(neow, "NEOW.pages.DONE.POSITIVE.description");
        
            MethodInfo doneMethod = typeof(AncientEventModel).GetMethod("Done",
                BindingFlags.NonPublic | BindingFlags.Instance);
            doneMethod.Invoke(neow, null);

        }
    }
}

[HarmonyPatch(typeof(Neow), "AllPossibleOptions", MethodType.Getter)]
public class AddAllNeowOptionsPatch
{
    public static void Postfix(Neow __instance, ref IEnumerable<EventOption> __result)
    {
        if (__instance is not Neow neow)
            return;
        List<EventOption> options = __result.ToList();
        options.Add(RelicOption<UnfamiliarDeckbox>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        __result = options;
    }
    
    protected static EventOption RelicOption<T>(string pageName = "INITIAL", string? customDonePage = null, Neow neow = null) where T : RelicModel
    {
        return RelicOption(ModelDb.Relic<T>().ToMutable(), pageName, neow: neow);
    }

    protected static EventOption RelicOption(RelicModel relic, string pageName = "INITIAL", string? customDonePage = null, Neow neow = null)
    {
        relic.AssertMutable();
        relic.Owner = neow.Owner;

        string textKey = $"{StringHelper.Slugify(neow.GetType().Name)}.pages.{pageName}.options.{relic.Id.Entry}";
        //string textKey = neow.OptionKey(pageName, relic.Id.Entry);
        return EventOption.FromRelic(relic, neow, OnChosen, textKey);

        async Task OnChosen()
        {
            RelicModel relicModel = await RelicCmd.Obtain(relic, neow.Owner);
            PropertyInfo customDonePageProp = typeof(AncientEventModel).GetProperty("CustomDonePage",
                BindingFlags.NonPublic | BindingFlags.Instance);
            customDonePageProp.SetValue(neow, "NEOW.pages.DONE.POSITIVE.description");
        
            MethodInfo doneMethod = typeof(AncientEventModel).GetMethod("Done",
                BindingFlags.NonPublic | BindingFlags.Instance);
            doneMethod.Invoke(neow, null);

        }
    }
}
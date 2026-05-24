using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using MoreNeow.MoreNeowCode.Relics.Complex;
using MoreNeow.MoreNeowCode.Relics.Simple;

namespace MoreNeow.MoreNeowCode.Patches;

[HarmonyPatch(typeof(AncientEventModel), "GenerateInitialOptionsWrapper")]
public class ModifyNeowOptionsPatch
{
    public static void Postfix(AncientEventModel __instance, ref IReadOnlyList<EventOption> __result)
    {
        if (__instance is not Neow neow)
            return;
            
        if (neow.Owner.RunState.Modifiers.Count > 0)
            return;

        List<EventOption> options = __result.ToList();
        IEnumerable<EventOption> customSimpleOptions =
        [
            RelicOption<BlueTierMembership>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow),
            RelicOption<HuntersGuide>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow),
            RelicOption<Inro>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow),
            RelicOption<ShiftingBlade>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow),
            RelicOption<WindChimes>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow),
            RelicOption<WrigglingInk>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow),
            RelicOption<PicnicBasket>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow),
            RelicOption<JaggedCoral>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow)
        ];
        IEnumerable<EventOption> customComplexOptions =
        [
            RelicOption<AncientLink>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow),
            RelicOption<SpectersGrin>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow),
            RelicOption<IronCrown>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow),
            RelicOption<RegalScepter>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow)
        ];

        IEnumerable<EventOption> allCustomOptions = customSimpleOptions.Concat(customComplexOptions);
        allCustomOptions.AddItem(RelicOption<UnfamiliarDeckbox>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow));
        
        bool alwaysOfferNewOption = MoreNeowConfig.AlwaysOfferNewOption;
        bool alwaysOfferDeckbox = MoreNeowConfig.AlwaysOfferDeckbox;
        bool onlyOfferNewOptions = MoreNeowConfig.OnlyOfferNewOptions;

        if (onlyOfferNewOptions)
        {
            options[0] = neow.Rng.NextItem(customSimpleOptions);
            options[1] = neow.Rng.NextItem(customSimpleOptions);
            options[2] = neow.Rng.NextItem(customComplexOptions);
        }
        
        if (UnfamiliarDeckbox.DoesCharacterHaveDeck(neow.Owner.Character))
        {
            if ((neow.Rng.NextBool() && neow.Rng.NextBool()) || alwaysOfferDeckbox)
            {
                options[1] = options[2];
                options[2] = RelicOption<UnfamiliarDeckbox>(customDonePage: "NEOW.pages.DONE.POSITIVE.description", neow: neow);
            }
        }

        if (alwaysOfferNewOption)
        {
            if (!options.Intersect<EventOption>(allCustomOptions).Any())
            {
                if (neow.Rng.NextBool() && neow.Rng.NextBool())
                {
                    options[2] = neow.Rng.NextItem(customComplexOptions);
                }
                else
                {
                    if (neow.Rng.NextBool())
                    {
                        options[0] = neow.Rng.NextItem(customSimpleOptions);
                    }
                    else
                    {
                        options[1] = neow.Rng.NextItem(customSimpleOptions);
                    }
                }
            }
        }
        
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
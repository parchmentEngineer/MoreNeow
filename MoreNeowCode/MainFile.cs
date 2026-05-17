using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Timeline.Stories;
using MoreNeow.MoreNeowCode.Cards.AltStarters;
using MoreNeow.MoreNeowCode.Cards.Scrapped;
using MoreNeow.MoreNeowCode.Relics.Complex;

namespace MoreNeow.MoreNeowCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "MoreNeow"; //Used for resource filepath

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();
        
        UnfamiliarDeckbox.AddCharacterDeck(ModelDb.GetId<Ironclad>(), ModelDb.GetId<IroncladAttack>(), ModelDb.GetId<IroncladSkill>());
        UnfamiliarDeckbox.AddCharacterDeck(ModelDb.GetId<Silent>(), ModelDb.GetId<SilentAttack>(), ModelDb.GetId<SilentSkill>());
        UnfamiliarDeckbox.AddCharacterDeck(ModelDb.GetId<Regent>(), ModelDb.GetId<RegentAttack>(), ModelDb.GetId<RegentSkill>());
        UnfamiliarDeckbox.AddCharacterDeck(ModelDb.GetId<Necrobinder>(), ModelDb.GetId<NecrobinderAttack>(), ModelDb.GetId<NecrobinderSkill>());
        UnfamiliarDeckbox.AddCharacterDeck(ModelDb.GetId<Defect>(), ModelDb.GetId<DefectAttack>(), ModelDb.GetId<DefectSkill>());
        
        /*var deckboxType = AccessTools.TypeByName("MoreNeow.MoreNeowCode.Relics.Complex.UnfamiliarDeckbox");
        if (deckboxType != null)
        {
            var addMethod = AccessTools.DeclaredMethod(deckboxType, "AddCharacterDeck");
            addMethod.Invoke(null, [ModelDb.GetId<Silent>(), ModelDb.GetId<SilentAttack>(), ModelDb.GetId<SilentSkill>()]);
        }*/
    }
    
    
}
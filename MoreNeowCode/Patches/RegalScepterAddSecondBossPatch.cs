using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MoreNeow.MoreNeowCode.Relics.Complex;

namespace MoreNeow.MoreNeowCode.Patches;

[HarmonyPatch(typeof(RunManager), "GenerateRooms")]
public class RegalScepterAddSecondBossPatch
{
    public static void Postfix(RunManager __instance)
    {
        bool hasRegalScepter = false;
        RunState state = __instance.State;
        //RunState state = Traverse.Create(__instance).Field("State").GetValue() as RunState;
        
        foreach (Player player in state.Players)
        {
            foreach (RelicModel relic in player.Relics)
            {
                if (relic.Id == ModelDb.GetId<RegalScepter>())
                {
                    hasRegalScepter = true;
                }
            }
        }

        if (hasRegalScepter)
        {
            ActModel act = state.Acts[0];
            EncounterModel encounter = state.Rng.UpFront.NextItem(act.AllBossEncounters.Where(e => e.Id != act.BossEncounter.Id));
            act.SetSecondBossEncounter(encounter);
        }
    }
}
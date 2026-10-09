using HarmonyLib;
using Multiplayer.Compat;
using Verse;

namespace MultiplayerCommonSensePatch.Source.Mods;

/// <summary>
///     Determinism fix for wandering. PointlessWanderPatches.cs Postfix does
///     <c>__result.targetC = new IntVec3(UnityEngine.Random.Range(400, 800), 0, 0)</c>,
///     so Unity RNG is replaced with Verse Rand (no Push/Pop, matches old compat behavior).
/// </summary>
public partial class CommonSense
{
    private static void PatchWanderRng()
    {
        // PointlessWanderPatches.cs Postfix does:
        // __result.targetC = new IntVec3(UnityEngine.Random.Range(400, 800), 0, 0);
        // Replace Unity RNG with Verse Rand (no Push/Pop, matches old compat behavior).
        if (AccessTools.Method("CommonSense.JobGiver_Wander_TryGiveJob_CommonSensePatch:Postfix") == null)
        {
            Log.Warning(
                $"{LogPrefix} Method not found (skipped): CommonSense.JobGiver_Wander_TryGiveJob_CommonSensePatch:Postfix. The mod may have updated.");
            return;
        }

        PatchingUtilities.PatchUnityRand("CommonSense.JobGiver_Wander_TryGiveJob_CommonSensePatch:Postfix", false);
        Log.Message($"{LogPrefix} Patched Unity RNG in JobGiver_Wander_TryGiveJob_CommonSensePatch.Postfix.");
    }
}
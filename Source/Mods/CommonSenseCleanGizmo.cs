using HarmonyLib;
using Multiplayer.Compat;
using Verse;

namespace MultiplayerCommonSensePatch.Source.Mods;

/// <summary>
///     Clean gizmo toggle sync. DoCleanComp.cs CompGetGizmosExtra builds a
///     <c>Command_Toggle</c> where lambda 0 is <c>isActive</c> and lambda 1 is the
///     <c>toggleAction</c> flipping <c>Active</c>; syncing lambda 1 replicates the
///     flip on all clients.
/// </summary>
public partial class CommonSense
{
    private static void PatchCleanGizmo()
    {
        // DoCleanComp.cs CompGetGizmosExtra:
        // lambda 0 is `isActive = (() => Active)`, lambda 1 is `toggleAction = delegate () { Active = !Active; }`.
        // Syncing lambda 1 replicates the Active flip on all clients.
        var compType = AccessTools.TypeByName("CommonSense.DoCleanComp");
        if (compType == null)
        {
            Log.Warning($"{LogPrefix} Type not found (skipped): CommonSense.DoCleanComp.");
            return;
        }

        try
        {
            MpCompat.RegisterLambdaMethod("CommonSense.DoCleanComp", "CompGetGizmosExtra", 1);
            Log.Message($"{LogPrefix} Synced CommonSense.DoCleanComp.CompGetGizmosExtra lambda 1 (Active toggle).");
        }
        catch (Exception exception)
        {
            Log.Warning($"{LogPrefix} Failed syncing DoCleanComp gizmo lambda 1: {exception.Message}");
        }
    }
}
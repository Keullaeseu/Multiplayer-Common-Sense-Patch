using HarmonyLib;
using Multiplayer.API;
using Multiplayer.Compat;
using Verse;

namespace MultiplayerCommonSensePatch.Source.Mods;

/// <summary>
///     Gear-tab manual-unload buttons. <c>CommonSense.Utility.DrawThingRow</c> draws the
///     unload/unload-cancel buttons (also used by
///     <c>ITab_Pawn_Gear_DrawThingRow_CommonSensePatch</c> and
///     <c>AwesomeInventory_CommonSensePatch</c> via <c>Utility.DrawThingRow</c>).
///     Button clicks flip <c>CompUnloadChecker.ShouldUnload</c> directly in UI, so the
///     comp is watched around the whole row draw and MP syncs the field change.
///     Depends on the sync state registered in CommonSenseCompSync.cs.
/// </summary>
public partial class CommonSense
{
    private static void PatchDrawThingRowWatch()
    {
        // Utility.DrawThingRow draws the unload/unload-cancel buttons in the gear tab
        // (also used by ITab_Pawn_Gear_DrawThingRow_CommonSensePatch and
        // AwesomeInventory_CommonSensePatch via Utility.DrawThingRow).
        // Button clicks flip CompUnloadChecker.ShouldUnload directly in UI, so watch
        // the comp around the whole row draw and let MP sync the field change.
        var drawThingRow = AccessTools.Method("CommonSense.Utility:DrawThingRow");
        if (drawThingRow == null)
        {
            Log.Warning(
                $"{LogPrefix} Method not found (skipped): CommonSense.Utility:DrawThingRow. The mod may have updated.");
            return;
        }

        if (shouldUnloadSyncField == null || getCheckerMethod == null)
        {
            Log.Warning(
                $"{LogPrefix} Comp sync not registered, skipping DrawThingRow watch (ShouldUnload field or GetChecker missing).");
            return;
        }

        MpCompat.harmony.Patch(drawThingRow,
            new HarmonyMethod(typeof(CommonSense), nameof(CommonSensePatchPrefix)),
            new HarmonyMethod(typeof(CommonSense), nameof(CommonSensePatchPostfix)));

        Log.Message($"{LogPrefix} Watching CommonSense.Utility.DrawThingRow for ShouldUnload changes.");
    }

    private static void CommonSensePatchPrefix(Thing thing, ref bool __state)
    {
        if (!MP.IsInMultiplayer)
            return;

        if (getCheckerMethod == null || shouldUnloadSyncField == null)
            return;

        if (thing == null)
            return;

        try
        {
            var unloadComp = getCheckerMethod(thing, false, false);
            if (unloadComp == null)
                return;

            MP.WatchBegin();
            shouldUnloadSyncField.Watch(unloadComp);
            wasInInventorySyncField?.Watch(unloadComp);
            __state = true;
        }
        catch (Exception exception)
        {
            Log.Warning($"{LogPrefix} CommonSensePatchPrefix watch failed: {exception.Message}");
            try
            {
                if (__state)
                    MP.WatchEnd();
            }
            catch (Exception endException)
            {
                Log.Warning(
                    $"{LogPrefix} CommonSensePatchPrefix WatchEnd after failure failed: {endException.Message}");
            }

            __state = false;
        }
    }

    private static void CommonSensePatchPostfix(bool __state)
    {
        if (!__state)
            return;

        try
        {
            MP.WatchEnd();
        }
        catch (Exception exception)
        {
            Log.Warning($"{LogPrefix} CommonSensePatchPostfix WatchEnd failed: {exception.Message}");
        }
    }
}
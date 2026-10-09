using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace MultiplayerCommonSensePatch.Source.Mods;

/// <summary>
///     CompUnloadChecker sync. The comp is created dynamically via
///     <c>CommonSense.CompUnloadChecker.GetChecker</c>, so it needs a custom sync
///     worker resolving it by parent <see cref="Thing" /> (creating it on read if
///     missing), plus sync fields for <c>ShouldUnload</c>/<c>WasInInventory</c>.
/// </summary>
public partial class CommonSense
{
    private static void RegisterCompSync()
    {
        var compType = AccessTools.TypeByName("CommonSense.CompUnloadChecker");
        if (compType == null)
        {
            Log.Warning(
                $"{LogPrefix} Type not found (skipped): CommonSense.CompUnloadChecker. The mod may have updated.");
            return;
        }

        var shouldUnloadField = AccessTools.Field(compType, "ShouldUnload");
        if (shouldUnloadField == null)
        {
            Log.Warning($"{LogPrefix} Field not found (skipped): CommonSense.CompUnloadChecker.ShouldUnload.");
            return;
        }

        var wasInInventoryField = AccessTools.Field(compType, "WasInInventory");
        if (wasInInventoryField == null)
            Log.Warning(
                $"{LogPrefix} Field not found (continuing without it): CommonSense.CompUnloadChecker.WasInInventory.");

        var getCheckerInfo = AccessTools.Method(compType, "GetChecker");
        if (getCheckerInfo == null)
        {
            Log.Warning($"{LogPrefix} Method not found (skipped): CommonSense.CompUnloadChecker.GetChecker.");
            return;
        }

        try
        {
            getCheckerMethod = AccessTools.MethodDelegate<GetCheckerDelegate>(getCheckerInfo);
        }
        catch (Exception exception)
        {
            Log.Warning($"{LogPrefix} Failed creating GetChecker delegate: {exception.Message}");
            return;
        }

        if (getCheckerMethod == null)
        {
            Log.Warning($"{LogPrefix} GetChecker delegate is null, skipping comp sync.");
            return;
        }

        // The comp is created dynamically via GetChecker, so it needs a custom sync worker
        // that resolves it by parent Thing (creating it on read if missing).
        MP.RegisterSyncWorker<ThingComp>(SyncComp, compType);
        shouldUnloadSyncField = MP.RegisterSyncField(shouldUnloadField);
        if (wasInInventoryField != null)
            wasInInventorySyncField = MP.RegisterSyncField(wasInInventoryField);

        Log.Message($"{LogPrefix} Synced CommonSense.CompUnloadChecker (ShouldUnload/WasInInventory).");
    }

    private static void SyncComp(SyncWorker sync, ref ThingComp thingComp)
    {
        if (sync.isWriting)
        {
            sync.Write(thingComp.parent);
        }
        else
        {
            // Registered only when getCheckerMethod is non-null; guard anyway.
            if (getCheckerMethod == null)
                return;

            thingComp = getCheckerMethod(sync.Read<Thing>(), false, false);
        }
    }
}
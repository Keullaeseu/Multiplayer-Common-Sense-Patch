using Multiplayer.API;
using Multiplayer.Compat;
using Verse;

namespace MultiplayerCommonSensePatch.Source.Mods;

/// <summary>
///     Multiplayer Patch for Common Sense by avilmask,
///     Last Update: 2 Oct @ 10:44pm 2026
///     <see href="https://steamcommunity.com/sharedfiles/filedetails/?id=1561769193" />
///     <see href="https://github.com/catgirlfighter/RimWorld_CommonSense" />
///     Ported from Multiplayer-Compatibility Source/Mods/CommonSense.cs and
///     verified against CommonSense 1.6 source (Source/CommonSense16/CommonSense)
///     and References/CommonSense.dll:
///     - CompUnloadChecker (ShouldUnload/WasInInventory + GetChecker) still exists in
///     UnloadChecker.cs, Utility.DrawThingRow still calls GetChecker(thing, false, true),
///     DoCleanComp.CompGetGizmosExtra toggle lambda still exists,
///     JobGiver_Wander_TryGiveJob_CommonSensePatch.Postfix still uses
///     UnityEngine.Random.Range(400, 800).
///     Entry point and shared sync state; the feature patches live in the other
///     CommonSense*.cs files (same partial class).
///     Scope note: the RPG Style Inventory popup (PatchRPG.cs) is intentionally not
///     synced — only relevant when that separate mod is installed.
/// </summary>
[MpCompatFor("avilmask.CommonSense")]
public partial class CommonSense
{
    private const string LogPrefix = "[Multiplayer Common Sense Patch]";

    private static ISyncField shouldUnloadSyncField;
    private static ISyncField wasInInventorySyncField;
    private static GetCheckerDelegate getCheckerMethod;

    public CommonSense(ModContentPack content)
    {
        Log.Message($"{LogPrefix} Initializing...");

        try
        {
            RegisterCompSync();
        }
        catch (Exception exception)
        {
            Log.Error($"{LogPrefix} RegisterCompSync failed: {exception}");
        }

        try
        {
            PatchWanderRng();
        }
        catch (Exception exception)
        {
            Log.Error($"{LogPrefix} PatchWanderRng failed: {exception}");
        }

        try
        {
            PatchCleanGizmo();
        }
        catch (Exception exception)
        {
            Log.Error($"{LogPrefix} PatchCleanGizmo failed: {exception}");
        }

        LongEventHandler.ExecuteWhenFinished(LatePatch);
    }

    private static void LatePatch()
    {
        Log.Message($"{LogPrefix} LatePatch initializing...");

        try
        {
            PatchDrawThingRowWatch();
        }
        catch (Exception exception)
        {
            Log.Error($"{LogPrefix} PatchDrawThingRowWatch failed: {exception}");
        }

        Log.Message($"{LogPrefix} Initialized.");
    }

    private delegate ThingComp GetCheckerDelegate(Thing thing, bool initShouldUnload, bool initWasInInventory);
}
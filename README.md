# Multiplayer Common Sense Patch

A RimWorld Multiplayer compatibility patch for [Common Sense](https://steamcommunity.com/sharedfiles/filedetails/?id=1561769193).

This mod is designed to improve multiplayer synchronization when playing with the [Common Sense](https://steamcommunity.com/sharedfiles/filedetails/?id=1561769193) mod and RimWorld Multiplayer.

## Features

- Adds multiplayer compatibility for [Common Sense](https://steamcommunity.com/sharedfiles/filedetails/?id=1561769193).
- Syncs manual-unload marking (`CompUnloadChecker.ShouldUnload` / `WasInInventory`) from the gear-tab row buttons, so unload marks set by one player replicate to everyone.
- Syncs the clean-area gizmo toggle (`DoCleanComp` active state).
- Replaces the `UnityEngine.Random` call in the wander job giver with Verse `Rand` for deterministic behavior across clients.

## Requirements

- RimWorld
- [Harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077)
- RimWorld Multiplayer
  - [GitHub version](https://github.com/rwmt/Multiplayer) or [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2606448745) version
- [Common Sense](https://steamcommunity.com/sharedfiles/filedetails/?id=1561769193)

The host and every connected player must use compatible versions of all required mods.

## Installation

### Steam Workshop

Subscribe to the required mods and add them to your RimWorld mod list in the following order:

1. Harmony
2. Core
3. Royalty, Ideology, Biotech, and Anomaly, if applicable
4. RimWorld Multiplayer
5. [Common Sense](https://steamcommunity.com/sharedfiles/filedetails/?id=1561769193)
6. [Multiplayer Common Sense Patch](https://github.com/Keullaeseu/Multiplayer-Common-Sense-Patch/releases/latest)

The patch should load after RimWorld Multiplayer and [Common Sense](https://steamcommunity.com/sharedfiles/filedetails/?id=1561769193).

### Manual Installation

1. Download the latest release from the [**Releases**](https://github.com/Keullaeseu/Multiplayer-Common-Sense-Patch/releases/latest) section.
2. Extract the mod folder into your RimWorld `Mods` directory.
3. Enable the required mods in RimWorld.
4. Use the recommended load order listed above.
5. Make sure every multiplayer player has the same mod list, configuration, and load order.

## Multiplayer Usage

All players should have the following mods installed and enabled:

- RimWorld Multiplayer
- [Common Sense](https://steamcommunity.com/sharedfiles/filedetails/?id=1561769193)
- [Multiplayer Common Sense Patch](https://github.com/Keullaeseu/Multiplayer-Common-Sense-Patch/releases/latest)
- All other required Common Sense dependencies

The host and all connected clients should use the same:

- RimWorld version
- RimWorld Multiplayer version
- Common Sense version
- Multiplayer Common Sense Patch version
- Mod configuration
- Mod load order

Do not add, remove, update, or reorder mods while players are connected to the same multiplayer session.

## Compatibility

This patch is intended to provide multiplayer compatibility for [Common Sense](https://steamcommunity.com/sharedfiles/filedetails/?id=1561769193)
itself (manual unload marking in the gear tab, clean-area gizmo toggle, wander-job
randomness).

It does not replace:

- [RimWorld Multiplayer](https://steamcommunity.com/sharedfiles/filedetails/?id=2606448745)
- [Common Sense](https://steamcommunity.com/sharedfiles/filedetails/?id=1561769193)

## Known Limitations

- The RPG Style Inventory unload popup is intentionally not synced; it only matters when that separate mod is installed alongside Common Sense.
- Compatibility may be affected by future RimWorld updates.
- Compatibility may be affected by future updates to RimWorld Multiplayer or Common Sense.

## Credits

- [RimWorld Multiplayer on GitHub](https://github.com/rwmt/Multiplayer)
- [RimWorld Multiplayer on Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2606448745)
- [Common Sense on Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=1561769193)
- [Common Sense source on GitHub](https://github.com/catgirlfighter/RimWorld_CommonSense)
- [Multiplayer Common Sense Patch](https://github.com/Keullaeseu)

# SCP-261

![Downloads](https://img.shields.io/github/downloads/MedveMarci/Scp261/total)
[![Version](https://img.shields.io/github/v/release/MedveMarci/Scp261?&label=Version&color=blue)](https://github.com/MedveMarci/Scp261/releases/latest)
![Framework](https://img.shields.io/badge/.NET-4.8-purple)
![License](https://img.shields.io/badge/license-MIT-green)

> **SCP: Secret Laboratory LabAPI plugin** that spawns a Vending Machine (SCP-261), allowing players to use it for random items or effects in exchange for a coin.

## Support

<a href='https://discord.gg/KmpA8cfaSA'><img src='https://www.allkpop.com/upload/2021/01/content/262046/1611711962-discord-button.png' height="80"></a>

---

## How It Works

Players holding a **coin** can interact with the SCP-261 vending machine. The coin is consumed and one of the following outcomes is randomly selected based on configurable weights:

| Outcome | Default Weight | Description |
|---------|---------------|-------------|
| Item | 50 | Dispenses a random item from the stock list |
| Effect | 30 | Applies a random status effect for a random duration |
| Size change | 5 | Shrinks the player (gnomed) until their next role change |
| Teleport | 10 | Teleports the player to a random unlocked door |
| Flashbang | 5 | Spawns a flash grenade at the player's feet |
| Tantrum | 9 | Spawns a tantrum hazard at the player's location |
| Explosion | 1 | Triggers a grenade explosion on the player |

> Weights do **not** need to add up to 100. Any module can be individually disabled — when disabled, it falls back to dispensing a random item instead.

---

## Installation

1. Download the latest release from [GitHub Releases](https://github.com/MedveMarci/Scp261/releases/latest):
   - `Scp261.dll`
2. Place the `.dll` in your server's plugins folder:
   - Linux: `~/.config/SCP Secret Laboratory/LabAPI/plugins/global/`
   - Windows: `%appdata%/SCP Secret Laboratory/LabAPI/plugins/global/`
3. Start the server — the config file is generated automatically on first launch.

---

## Configuration

The config file is generated at:
- Linux: `~/.config/SCP Secret Laboratory/LabAPI/configs/`
- Windows: `%appdata%/SCP Secret Laboratory/LabAPI/configs/`

### Key settings

| Option | Default | Description |
|--------|---------|-------------|
| `enable_effects` | `true` | Enable the effects module |
| `enable_size_change` | `true` | Enable the size change (gnomed) module |
| `enable_explode` | `true` | Enable the explosion module |
| `enable_flashbang` | `true` | Enable the flashbang module |
| `enable_tantrum` | `true` | Enable the tantrum module |
| `random_teleport` | `true` | Enable the random teleport module |
| `item_chance` | `50` | Weight for dispensing an item |
| `effect_chance` | `30` | Weight for applying an effect |
| `size_change_chance` | `5` | Weight for the size change |
| `explode_chance` | `1` | Weight for the explosion |
| `flashbang_chance` | `5` | Weight for the flashbang |
| `tantrum_chance` | `9` | Weight for the tantrum |
| `random_teleport_chance` | `10` | Weight for the teleport |
| `min_effect_duration` | `10` | Minimum effect duration (seconds) |
| `max_effect_duration` | `25` | Maximum effect duration (seconds) |
| `gnomed_size` | `(1.13, 0.5, 1.13)` | Player scale when gnomed |
| `vending_machine_stock` | *(see below)* | List of items the machine can dispense |
| `effect_list` | *(see below)* | List of status effects the machine can apply |

### Default item stock

`KeycardChaosInsurgency`, `KeycardContainmentEngineer`, `KeycardFacilityManager`, `GunA7`, `GunFSP9`, `GunLogicer`, `GunE11SR`, `GunRevolver`, `GunCrossvec`, `GunCom45`, `Jailbird`, `ParticleDisruptor`, `SCP018`, `SCP207`, `AntiSCP207`, `KeycardJanitor`, `KeycardScientist`, `KeycardZoneManager`, `KeycardGuard`, `MicroHID`, `SCP500`, `SCP268`, `SCP1853`

### Default effects

`Invigorated`, `AntiScp207`, `Scp207`, `Asphyxiated`, `Flashed`, `Scp1853`, `Hypothermia`, `MovementBoost`, `Invisible`

---

## Credits

- Developed by **MedveMarci**

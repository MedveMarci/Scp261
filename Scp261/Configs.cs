using System.Collections.Generic;
using System.ComponentModel;
using CustomPlayerEffects;
using InventorySystem.Items.Usables.Scp244.Hypothermia;
using MapGeneration;
using UnityEngine;

namespace Scp261;

public class Configs
{
    [Description("This option is for SCP-261 spawn locations")]
    public List<CustomRoomLocationData> SpawnLocations { get; set; } =
    [
        new()
        {
            RoomName = RoomName.EzOfficeSmall,
            Position = new Vector3(6.816f, -1.428f, 6.550f),
            Rotation = new Vector3(0, -180, 0),
            Chance = 100f
        }
    ];
    
    public class CustomRoomLocationData
    {
        public RoomName RoomName { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public float Chance { get; set; }
    }
    
    [Description("Enable the effects module. When disabled, the vending machine will not apply effects to players.")]
    public bool EnableEffects { get; set; } = true;
    
    [Description("Enable the size change module. When disabled, the vending machine will not change player size.")]
    public bool EnableSizeChange { get; set; } = true;
    
    [Description("Enable the explode module. When disabled, the vending machine will not explode.")]
    public bool EnableExplode { get; set; } = true;
    
    [Description("Enable the flashbang module. When disabled, the vending machine will not dispense flashbangs.")]
    public bool EnableFlashbang { get; set; } = true;
    
    [Description("Enable the tantrum module. When disabled, the vending machine will not trigger tantrums.")]
    public bool EnableTantrum { get; set; } = true;
    
    [Description("Enable the random teleport module. When disabled, the vending machine will not teleport players.")]
    public bool RandomTeleport { get; set; } = true;

    [Description("Module chance weights: (They don't need to add up to 100.)")]
    public int ItemChance { get; set; } = 50;

    [Description("Chance for effects module to be selected. (They don't need to add up to 100.)")]
    public int EffectChance { get; set; } = 30;
    
    [Description("Chance for size change module to be selected. (They don't need to add up to 100.)")]
    public int SizeChangeChance { get; set; } = 5;
    
    [Description("Chance for explode module to be selected. (They don't need to add up to 100.)")]
    public int ExplodeChance { get; set; } = 1;
    
    [Description("Chance for flashbang module to be selected. (They don't need to add up to 100.)")]
    public int FlashbangChance { get; set; } = 5;
    
    [Description("Chance for tantrum module to be selected. (They don't need to add up to 100.)")]
    public int TantrumChance { get; set; } = 9;
    
    [Description("Chance for random teleport module to be selected. (They don't need to add up to 100.)")]
    public int RandomTeleportChance { get; set; } = 10;
    
    [Description("Message displayed when the vending machine successfully dispenses an item.")]
    public string InteractionSuccessfulItems { get; set; } =
        "<b>The vending machine <color=#42f57b>dispensed something.</color></b>";

    [Description("Message displayed when the vending machine successfully applies effects.")]
    public string InteractionSuccessfulEffects { get; set; } =
        "<b>The vending machine <color=#42f57b>shook violently.</color></b>";

    [Description("Message displayed when the vending machine successfully changes player size.")]
    public string InteractionSuccessfulSize { get; set; } =
        "<b>The vending machine <color=#42f57b>gnomed you.</color></b>";

    [Description("Message displayed when the vending machine successfully explodes.")]
    public string InteractionSuccessfulExplode { get; set; } =
        "<b>The vending machine <color=#42f57b>exploded.</color></b>";

    [Description("Message displayed when the vending machine successfully dispenses a flashbang.")]
    public string InteractionSuccessfulFlashbang { get; set; } =
        "<b>The vending machine <color=#42f57b>dispensed a weird can.</color></b>";

    [Description("Message displayed when the vending machine triggers a tantrum effect.")]
    public string InteractionSuccessfulTantrum { get; set; } = "<b>You <color=#42f57b>shit yourself.</color></b>";
    
    [Description("Message displayed when the vending machine teleports the player.")]
    public string InteractionSuccessfulTeleport { get; set; } = "<b>You teleported to somewhere.</b>";
    
    [Description("Message displayed when the player does not have a coin to use the vending machine.")]
    public string InteractionFailedMessage { get; set; } = "<b>You aren't <color=red>holding a coin!</color></b>";

    [Description("List of effects that the vending machine can apply:")]
    public List<string> EffectList { get; set; } =
    [
        nameof(Invigorated),
        nameof(AntiScp207),
        nameof(Scp207),
        nameof(Asphyxiated),
        nameof(Flashed),
        nameof(Scp1853),
        nameof(Hypothermia),
        nameof(MovementBoost),
        nameof(Invisible)
    ];


    [Description("List of items that the vending machine has in stock:")]
    public List<ItemType> VendingMachineStock { get; set; } =
    [
        ItemType.KeycardChaosInsurgency,
        ItemType.KeycardContainmentEngineer,
        ItemType.KeycardFacilityManager,
        ItemType.GunA7,
        ItemType.GunFSP9,
        ItemType.GunLogicer,
        ItemType.GunE11SR,
        ItemType.GunRevolver,
        ItemType.GunCrossvec,
        ItemType.GunCom45,
        ItemType.Jailbird,
        ItemType.ParticleDisruptor,
        ItemType.SCP018,
        ItemType.SCP207,
        ItemType.AntiSCP207,
        ItemType.KeycardJanitor,
        ItemType.KeycardScientist,
        ItemType.KeycardZoneManager,
        ItemType.KeycardGuard,
        ItemType.MicroHID,
        ItemType.SCP500,
        ItemType.SCP268,
        ItemType.SCP1853
    ];

    [Description("Minimum duration for effects applied by the vending machine:")]
    public int MinEffectDuration { get; set; } = 10;

    [Description("Maximum duration for effects applied by the vending machine:")]
    public int MaxEffectDuration { get; set; } = 25;

    [Description("Size change settings:")] 
    public Vector3 GnomedSize { get; set; } = new(1.13f, 0.5f, 1.13f);
}
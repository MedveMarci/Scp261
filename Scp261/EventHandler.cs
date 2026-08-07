using System;
using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles.FirstPersonControl;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using Scp261.ApiFeatures;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Scp261;

internal static class EventHandler
{
    private static readonly List<SchematicObject> Schematic = [];

    public static void EnableEvents()
    {
        PlayerEvents.InteractedToy += Interacted;
        PlayerEvents.ChangedRole += OnChangedRole;
        ServerEvents.WaitingForPlayers += OnWaitingForPlayers;
    }

    public static void DisableEvents()
    {
        PlayerEvents.InteractedToy -= Interacted;
        PlayerEvents.ChangedRole -= OnChangedRole;
        ServerEvents.WaitingForPlayers -= OnWaitingForPlayers;
    }

    private static void OnWaitingForPlayers()
    {
        VersionManager.CheckForUpdates();
        foreach (var schematic in Schematic)
        {
            if (schematic == null)
                continue;
            schematic.Destroy();
        }

        Schematic.Clear();

        var spawnLocations = Scp261.Singleton.Config.SpawnLocations;
        if (spawnLocations == null || spawnLocations.Count == 0)
            return;

        if (Scp261.Singleton.Config.SpawnOnlyOnce)
        {
            var pickedLocation = PickWeightedLocation(spawnLocations);
            if (pickedLocation != null)
                SpawnSchematic(pickedLocation);
            return;
        }

        foreach (var spawnLocation in spawnLocations)
        {
            var chanceRoll = Random.Range(0f, 100f);
            if (chanceRoll > spawnLocation.Chance)
                continue;

            SpawnSchematic(spawnLocation);
        }
    }
    
    private static Configs.CustomRoomLocationData PickWeightedLocation(
        List<Configs.CustomRoomLocationData> spawnLocations)
    {
        var totalWeight = spawnLocations.Sum(location => Mathf.Max(0f, location.Chance));
        if (totalWeight <= 0f)
            return null;

        var roll = Random.Range(0f, totalWeight);
        foreach (var location in spawnLocations)
        {
            roll -= Mathf.Max(0f, location.Chance);
            if (roll <= 0f)
                return location;
        }

        return spawnLocations[spawnLocations.Count - 1];
    }

    private static void SpawnSchematic(Configs.CustomRoomLocationData spawnLocation)
    {
        var room = Room.List.FirstOrDefault(room => room.Name == spawnLocation.RoomName);
        if (room == null)
        {
            LogManager.Warn($"Pickup spawn aborted: room '{spawnLocation.RoomName}' not found.");
            return;
        }

        var position = room.Transform.TransformPoint(spawnLocation.Position);
        var rotation = room.Transform.rotation * Quaternion.Euler(spawnLocation.Rotation);

        var schematic = ObjectSpawner.SpawnSchematic("Scp261", position, rotation);
        Schematic.Add(schematic);
    }

    private static void OnChangedRole(PlayerChangedRoleEventArgs ev)
    {
        if (ev.Player.Scale == Scp261.Singleton.Config.GnomedSize) ev.Player.Scale = Vector3.one;
    }
    
    private static void GiveRandomItem(Player player)
    {
        var stock = Scp261.Singleton.Config.VendingMachineStock;
        if (stock == null || stock.Count == 0)
            return;

        var item = player.AddItem(stock[Random.Range(0, stock.Count)]);
        if (!Scp261.Singleton.Config.SpawnWeaponsWithAmmo && item is FirearmItem firearm)
        {
            if (firearm.StoredAmmo > 0) firearm.StoredAmmo = 0;
            if (firearm.ChamberedAmmo > 0) firearm.ChamberedAmmo = 0;
        }

        player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulItems, 5f);
    }

    private static void Interacted(PlayerInteractedToyEventArgs ev)
    {
        var player = Player.Get(ev.Player.ReferenceHub);
        if (!ev.Interactable.GameObject.name.Equals("Vending")) return;

        var activationItems = Scp261.Singleton.Config.ActivationItems;
        if (player.CurrentItem == null || activationItems == null ||
            !activationItems.Contains(player.CurrentItem.Type))
        {
            player.SendHint(Scp261.Singleton.Config.InteractionFailedMessage, 5f);
            return;
        }

        player.RemoveItem(player.CurrentItem);
        var weightedChances = new WeightedChanceExecutor(
            new WeightedChanceParam(() =>
            {
                try
                {
                    GiveRandomItem(player);
                }
                catch (Exception ex)
                {
                    LogManager.Error($"Item chance action failed: {ex}");
                }
            }, Scp261.Singleton.Config.ItemChance),
            new WeightedChanceParam(() =>
            {
                try
                {
                    if (Scp261.Singleton.Config.EnableEffects)
                    {
                        var effectName =
                            Scp261.Singleton.Config.EffectList[
                                Random.Range(0, Scp261.Singleton.Config.EffectList.Count)];
                        var effectType = Type.GetType($"CustomPlayerEffects.{effectName}");
                        if (effectType != null && typeof(StatusEffectBase).IsAssignableFrom(effectType))
                            player.EnableEffect((StatusEffectBase)Activator.CreateInstance(effectType), 0,
                                Random.Range(Scp261.Singleton.Config.MinEffectDuration,
                                    Scp261.Singleton.Config.MaxEffectDuration));
                        player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulEffects, 5f);
                    }
                    else
                    {
                        GiveRandomItem(player);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Error($"Effect chance action failed: {ex}");
                }
            }, Scp261.Singleton.Config.EffectChance),
            new WeightedChanceParam(() =>
            {
                try
                {
                    if (Scp261.Singleton.Config.EnableSizeChange)
                    {
                        player.Scale = Scp261.Singleton.Config.GnomedSize;
                        player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulSize, 5f);
                    }
                    else
                    {
                        GiveRandomItem(player);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Error($"Size change action failed: {ex}");
                }
            }, Scp261.Singleton.Config.SizeChangeChance),
            new WeightedChanceParam(() =>
            {
                try
                {
                    if (Scp261.Singleton.Config.EnableExplode)
                    {
                        ExplosionUtils.ServerExplode(player.ReferenceHub, ExplosionType.Grenade);
                        player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulExplode, 5f);
                    }
                    else
                    {
                        GiveRandomItem(player);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Error($"Explode action failed: {ex}");
                }
            }, Scp261.Singleton.Config.ExplodeChance),
            new WeightedChanceParam(() =>
            {
                try
                {
                    if (Scp261.Singleton.Config.EnableFlashbang)
                    {
                        TimedGrenadeProjectile.SpawnActive(player.Position, ItemType.GrenadeFlash, null, 2f);
                        player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulFlashbang, 5f);
                    }
                    else
                    {
                        GiveRandomItem(player);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Error($"Flashbang action failed: {ex}");
                }
            }, Scp261.Singleton.Config.FlashbangChance),
            new WeightedChanceParam(() =>
            {
                try
                {
                    if (Scp261.Singleton.Config.EnableTantrum)
                    {
                        TantrumHazard.Spawn(player.Position, player.Rotation, Vector3.one);
                        player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulTantrum, 5f);
                    }
                    else
                    {
                        GiveRandomItem(player);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Error($"Tantrum action failed: {ex}");
                }
            }, Scp261.Singleton.Config.TantrumChance),
            new WeightedChanceParam(() =>
            {
                try
                {
                    if (Scp261.Singleton.Config.RandomTeleport)
                    {
                        var blockLcz = Decontamination.IsDecontaminating;
                        var randomDoor = Door.List
                            .Where(d => !d.IsLocked && (!blockLcz || d.Zone != FacilityZone.LightContainment))
                            .OrderBy(_ => Guid.NewGuid())
                            .FirstOrDefault();
                        if (randomDoor != null) player.Position = EnsurePositionSafety(randomDoor.Transform);
                        player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulTeleport, 5f);
                    }
                    else
                    {
                        GiveRandomItem(player);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Error($"Teleport action failed: {ex}");
                }
            }, Scp261.Singleton.Config.RandomTeleportChance)
        );
        weightedChances.Execute();
    }

    private static Vector3 EnsurePositionSafety(Transform doorTransform)
    {
        var position = doorTransform.position + Vector3.up;
        while (Physics.CheckSphere(position, 0.35f, FpcStateProcessor.Mask)) position += doorTransform.forward * 0.35f;
        return position;
    }
}
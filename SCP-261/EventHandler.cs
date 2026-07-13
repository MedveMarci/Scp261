using System;
using System.Linq;
using CustomPlayerEffects;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles.FirstPersonControl;
using Scp261.ApiFeatures;
using UnityEngine;
using Utils;
using Random = System.Random;

namespace Scp261;

internal static class EventHandler
{
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
    }
    
    private static void OnChangedRole(PlayerChangedRoleEventArgs ev)
    {
        if (ev.Player.Scale == Scp261.Singleton.Config.GnomedSize) ev.Player.Scale = Vector3.one;
    }
    
    private static void Interacted(PlayerInteractedToyEventArgs ev)
    {
        var player = Player.Get(ev.Player.ReferenceHub);
        if (!ev.Interactable.GameObject.name.Equals("Vending")) return;
        if (player.CurrentItem == null)
        {
            player.SendHint(Scp261.Singleton.Config.InteractionFailedMessage, 5f);
            return;
        }

        if (player.CurrentItem.Type == ItemType.Coin)
        {
            player.RemoveItem(player.CurrentItem);
            var weightedChances = new WeightedChanceExecutor(
                new WeightedChanceParam(() =>
                {
                    try
                    {
                        var random = new Random();
                        var randomItem = Scp261.Singleton.Config.VendingMachineStock[random.Next(Scp261.Singleton.Config.VendingMachineStock.Count)];
                        player.AddItem(randomItem);
                        player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulItems, 5f);
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
                            var random = new Random();
                            var effectName = Scp261.Singleton.Config.EffectList[random.Next(Scp261.Singleton.Config.EffectList.Count)];
                            var effectType = Type.GetType($"CustomPlayerEffects.{effectName}");
                            if (effectType != null && typeof(StatusEffectBase).IsAssignableFrom(effectType))
                                player.EnableEffect((StatusEffectBase)Activator.CreateInstance(effectType), 0,
                                    random.Next(Scp261.Singleton.Config.MinEffectDuration, Scp261.Singleton.Config.MaxEffectDuration));
                            player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulEffects, 5f);
                        }
                        else
                        {
                            var random = new Random();
                            var randomItem = Scp261.Singleton.Config.VendingMachineStock[random.Next(Scp261.Singleton.Config.VendingMachineStock.Count)];
                            player.AddItem(randomItem);
                            player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulItems, 5f);
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
                            var random = new Random();
                            var randomItem = Scp261.Singleton.Config.VendingMachineStock[random.Next(Scp261.Singleton.Config.VendingMachineStock.Count)];
                            player.AddItem(randomItem);
                            player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulItems, 5f);
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
                            var random = new Random();
                            var randomItem = Scp261.Singleton.Config.VendingMachineStock[random.Next(Scp261.Singleton.Config.VendingMachineStock.Count)];
                            player.AddItem(randomItem);
                            player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulItems, 5f);
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
                            var random = new Random();
                            var randomItem = Scp261.Singleton.Config.VendingMachineStock[random.Next(Scp261.Singleton.Config.VendingMachineStock.Count)];
                            player.AddItem(randomItem);
                            player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulItems, 5f);
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
                            var random = new Random();
                            var randomItem = Scp261.Singleton.Config.VendingMachineStock[random.Next(Scp261.Singleton.Config.VendingMachineStock.Count)];
                            player.AddItem(randomItem);
                            player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulItems, 5f);
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
                            var randomDoor = Door.List
                                .Where(d => !d.IsLocked && (!Decontamination.IsDecontaminating ||
                                                            d.Zone != FacilityZone.LightContainment))
                                .OrderBy(_ => Guid.NewGuid())
                                .FirstOrDefault();
                            if (randomDoor != null) player.Position = EnsurePositionSafety(randomDoor.Transform);
                            player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulTeleport, 5f);
                        }
                        else
                        {
                            var random = new Random();
                            var randomItem = Scp261.Singleton.Config.VendingMachineStock[random.Next(Scp261.Singleton.Config.VendingMachineStock.Count)];
                            player.AddItem(randomItem);
                            player.SendHint(Scp261.Singleton.Config.InteractionSuccessfulItems, 5f);
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
        else
        {
            player.SendHint(Scp261.Singleton.Config.InteractionFailedMessage, 5f);
        }
    }

    private static Vector3 EnsurePositionSafety(Transform doorTransform)
    {
        var position = doorTransform.position + Vector3.up;
        while (Physics.CheckSphere(position, 0.35f, FpcStateProcessor.Mask)) position += doorTransform.forward * 0.35f;
        return position;
    }
}
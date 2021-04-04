using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Linq;
using UnityEngine;
using Rocket.API.Collections;
using Rocket.API;
using System.Collections;
using Logger = Rocket.Core.Logging.Logger;
using System;
using Rocket.Unturned;

namespace SuperRestrictor
{
    public class SuperRestrictor : RocketPlugin<SuperRestrictorConfiguration>
    {
        public static SuperRestrictor Instance { get; private set; }

        protected override void Load()
        {
            Instance = this;

            ChatManager.onChatted += onChat;
            U.Events.OnPlayerConnected += OnPlayerConnected;
            UnturnedPlayerEvents.OnPlayerInventoryAdded += OnInventoryUpdated;
            UnturnedPlayerEvents.OnPlayerWear += OnWear;
            VehicleManager.onEnterVehicleRequested += OnEnterVehicleRequested;

            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");
            Logger.LogWarning($"[{Name}] has been loaded! ");
            Logger.LogWarning("Dev: MQS#7816");
            Logger.LogWarning("Join this Discord for Support: https://discord.gg/Ssbpd9cvgp");
            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");
        }



        protected override void Unload()
        {
            Instance = null;

            ChatManager.onChatted += onChat;
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            UnturnedPlayerEvents.OnPlayerInventoryAdded -= OnInventoryUpdated;
            UnturnedPlayerEvents.OnPlayerWear -= OnWear;
            VehicleManager.onEnterVehicleRequested -= OnEnterVehicleRequested;

        }


        private void OnPlayerConnected(UnturnedPlayer player)
        {
            var playername = player.CharacterName;

            RestrictedName username = Configuration.Instance.RestrictedNames.FirstOrDefault(n => playername.ToLower().Contains(n.name.ToLower()));

            if ((player.IsAdmin && Configuration.Instance.IgnoreAdmins) || player.GetPermissions().Any(x => x.Name == "ignore.*"))
                return;

            if (username != null && !player.GetPermissions().Any(x => x.Name == username.Bypass))
            {
                if (username.Message == null)
                {
                    player.Kick(Instance.Translate("NameBlacklist"));
                }

                else
                {
                    player.Kick(username.Message);
                }
            }
        }

        private void onChat(SteamPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            var converted = UnturnedPlayer.FromSteamPlayer(player);

            RestrictedWord message = Configuration.Instance.RestrictedWords.FirstOrDefault(w => text.ToLower().Contains(w.name.ToLower()));

            if ((converted.IsAdmin && Configuration.Instance.IgnoreAdmins) || converted.GetPermissions().Any(x => x.Name == "ignore.*"))
                return;

            if (message != null && !converted.GetPermissions().Any(x => x.Name == message.Bypass))
            {
                isVisible = false;
                {
                    if (message.Message == null)
                    {
                        UnturnedChat.Say(converted, Instance.Translate("WordBlacklist"), Color.red);
                    }

                    else
                    {
                        UnturnedChat.Say(converted, message.Message, Color.red);
                    }
                }
            }
        }

        private void OnEnterVehicleRequested(Player player, InteractableVehicle vehicle, ref bool shouldAllow)
        {
            var driver = UnturnedPlayer.FromPlayer(player);

            RestrictedVehicle car = Configuration.Instance.RestrictedVehicles.FirstOrDefault(x => x.VehicleId == vehicle.id);

            if ((driver.IsAdmin && Configuration.Instance.IgnoreAdmins) || driver.GetPermissions().Any(x => x.Name == "ignore.*"))
                return;

            if (car != null && !driver.GetPermissions().Any(x => x.Name == car.Bypass))
            {
                shouldAllow = false;
                if (car.Message == null)
                {
                    UnturnedChat.Say(driver, Instance.Translate("VehicleBlacklist"), Color.red);
                }

                else
                {
                    UnturnedChat.Say(driver, car.Message, Color.red);
                }
            }

        }

        private void OnInventoryUpdated(UnturnedPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P)
        {
            RestrictedItem item = Configuration.Instance.RestrictedItems.FirstOrDefault(x => x.Id == P.item.id);

            if ((player.IsAdmin && Configuration.Instance.IgnoreAdmins) || player.GetPermissions().Any(x => x.Name == "ignore.*"))
                return;

            if (item != null && !player.GetPermissions().Any(x => x.Name == item.Bypass))
            { 
                player.Inventory.removeItem((byte)inventoryGroup, inventoryIndex);
                if (item.Message == null)
                {
                    UnturnedChat.Say(player, Instance.Translate("ItemBlacklisted"), Color.red);
                }

                else
                {
                    UnturnedChat.Say(player, item.Message, Color.red);
                }
            }
        }

        private void OnWear(UnturnedPlayer player, UnturnedPlayerEvents.Wearables wear, ushort id, byte? quality)
        {
            RestrictedItem item = Configuration.Instance.RestrictedItems.FirstOrDefault(x => x.Id == id);

            if ((player.IsAdmin && Configuration.Instance.IgnoreAdmins) || player.GetPermissions().Any(x => x.Name == "ignore.*"))
                return;

            if (item != null && !player.GetPermissions().Any(x => x.Name == item.Bypass))
            {
                switch (wear)
                {
                    #region WearSwitch
                    case UnturnedPlayerEvents.Wearables.Backpack:
                        StartCoroutine(InvokeOnNextFrame(() =>
                        player.Player.clothing.askWearBackpack(0, 0, new byte[0], true)));
                        break;
                    case UnturnedPlayerEvents.Wearables.Glasses:
                        StartCoroutine(InvokeOnNextFrame(() =>
                        player.Player.clothing.askWearGlasses(0, 0, new byte[0], true)));
                        break;
                    case UnturnedPlayerEvents.Wearables.Hat:
                        StartCoroutine(InvokeOnNextFrame(() =>
                        player.Player.clothing.askWearHat(0, 0, new byte[0], true)));
                        break;
                    case UnturnedPlayerEvents.Wearables.Mask:
                        StartCoroutine(InvokeOnNextFrame(() =>
                        player.Player.clothing.askWearMask(0, 0, new byte[0], true)));
                        break;
                    case UnturnedPlayerEvents.Wearables.Pants:
                        StartCoroutine(InvokeOnNextFrame(() =>
                        player.Player.clothing.askWearPants(0, 0, new byte[0], true)));
                        break;
                    case UnturnedPlayerEvents.Wearables.Shirt:
                        StartCoroutine(InvokeOnNextFrame(() =>
                        player.Player.clothing.askWearShirt(0, 0, new byte[0], true)));
                        break;
                    case UnturnedPlayerEvents.Wearables.Vest:
                        StartCoroutine(InvokeOnNextFrame(() =>
                        player.Player.clothing.askWearVest(0, 0, new byte[0], true)));
                        break;
                    #endregion
                }
            }
        }

        private IEnumerator InvokeOnNextFrame(System.Action action)
        {
            yield return new WaitForFixedUpdate();
            action();
        }

        public override TranslationList DefaultTranslations =>
            new TranslationList
            {
                { "WordBlacklisted", "You said a word that is on the blacklist. Please moderate yourself!" },
                { "ItemBlacklisted", "You are not allowed to loot that item." },
                { "NameBlacklist", "Your name is in the Blacklist. Please change it." },
                { "VehicleBlacklist", "You are not allowed to drive this vehicle. The vehicle is in the Blacklist."}
            };
    }
}

using Comfort.Common;
using EFT;
using EFT.Communications;
using EFT.InputSystem;
using EFT.UI.BattleTimer;
using EFT.Weather;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TellTheTime.Patches
{
    internal class WatchPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GamePlayerOwner), nameof(GamePlayerOwner.TranslateCommand));
        }

        [PatchPostfix]
        static void Postfix(ECommand command)
        {
            if (command != ECommand.DisplayTimer)
            {
                return;
            }

            string time = Singleton<GameWorld>.Instance.GameDateTime.Calculate().ToString("HH:mm:ss");

            NotificationManagerClass.DisplayMessageNotification($"Current time is: {time}", ENotificationDurationType.Default, ENotificationIconType.EntryPoint);

            if (Plugin.EnableLocalTime.Value)
            {
                string localTime = DateTime.Now.ToString("HH:mm:ss");
                NotificationManagerClass.DisplayMessageNotification($"IRL time is: {localTime}", ENotificationDurationType.Default, ENotificationIconType.EntryPoint);
            }
        }
    }
}

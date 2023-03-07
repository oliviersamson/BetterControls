using BepInEx.Configuration;
using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MuckSettings;

namespace BetterControls
{
    public static class ControlsConfig
    {
        public static ConfigFile Config = new ConfigFile(Path.Combine(Paths.ConfigPath, "bettercontrols.cfg"), true);

        [HarmonyPatch(typeof(MuckSettings.Settings), "Controls")]
        [HarmonyPrefix]
        static void Controls(MuckSettings.Settings.Page page)
        {
            page.AddControlSetting("Ping", NewInputs.Ping);
            page.AddControlSetting("Open Chat", NewInputs.Chat);
            page.AddControlSetting("Last Selected Hotbar Cell", NewInputs.Hotbar.LastSelected);

            for (int i = 0; i < NewInputs.Hotbar.Cells.Length; i++)
            {
                page.AddControlSetting($"Hotbar Cell {i + 1}", NewInputs.Hotbar.Cells[i]);
            }
        }
    }
}

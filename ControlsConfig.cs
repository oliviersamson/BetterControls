using BepInEx.Configuration;
using BepInEx;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

            if (NewInputs.Rotate.DefaultValue == null)
            {
                NewInputs.Rotate.BoxedValue = KeyCode.R;
            }
            
            page.AddControlSetting("Rotate Build", (KeyCode)NewInputs.Rotate.BoxedValue, 
                (keyCode) => {

                    NewInputs.Rotate.BoxedValue = keyCode;

                    if (SceneManager.GetActiveScene().name == "GameAfterLobby")
                    {
                        TextMeshProUGUI rotateText = BuildManager.Instance.rotateText.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                        rotateText.SetText($"Press " + keyCode.ToString() + " to rotate build");
                    }
                });

            page.AddControlSetting("Drop Item", NewInputs.Drop);

            page.AddControlSetting("Last Selected Hotbar Cell", NewInputs.Hotbar.LastSelected);

            for (int i = 0; i < NewInputs.Hotbar.Cells.Length; i++)
            {
                page.AddControlSetting($"Hotbar Cell {i + 1}", NewInputs.Hotbar.Cells[i]);
            }
        }
    }
}

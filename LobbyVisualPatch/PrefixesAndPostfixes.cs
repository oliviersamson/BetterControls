using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

namespace BetterControls.LobbyVisualPatch
{
    public class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(LobbyVisuals), "Awake")]
        [HarmonyPostfix]
        static void AwakePostfix()
        {
            EscapeUI escapeUI = LobbyVisuals.Instance.gameObject.AddComponent<EscapeUI>();
            MenuUI menuUi = LobbyVisuals.Instance.menuUi;

            escapeUI.backBtn = menuUi.lobbyUi.transform.Find("LobbyDetails").Find("BackBtn").GetComponent<Button>();
        }
    }
}

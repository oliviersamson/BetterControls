using BepInEx.Configuration;
using UnityEngine;

namespace BetterControls
{
    public static class NewInputs
    {
        public static ConfigEntry<KeyCode> Ping = ControlsConfig.Config.Bind<KeyCode>("Ping", "ping", KeyCode.Mouse2, "Player ping.");
        public static ConfigEntry<KeyCode> Chat = ControlsConfig.Config.Bind<KeyCode>("Chat", "openChat", KeyCode.Return, "Open chat.");
        public static ConfigEntry<KeyCode> Rotate = ControlsConfig.Config.Bind<KeyCode>("Build", "rotateBuild", KeyCode.R, "Rotate build.");
        public static ConfigEntry<KeyCode> Drop = ControlsConfig.Config.Bind<KeyCode>("Drop", "dropItem", KeyCode.G, "Drop item.");

        public static class Hotbar
        {
            public static ConfigEntry<KeyCode> LastSelected = ControlsConfig.Config.Bind<KeyCode>("Hotbar", "lastSelected", KeyCode.Q, "Last selected hotbar cell.");

            public static ConfigEntry<KeyCode>[] Cells =
            {
                ControlsConfig.Config.Bind<KeyCode>("Hotbar", "cell1", KeyCode.Alpha1, "Hotbar cell 1."),
                ControlsConfig.Config.Bind<KeyCode>("Hotbar", "cell2", KeyCode.Alpha2, "Hotbar cell 2."),
                ControlsConfig.Config.Bind<KeyCode>("Hotbar", "cell3", KeyCode.Alpha3, "Hotbar cell 3."),
                ControlsConfig.Config.Bind<KeyCode>("Hotbar", "cell4", KeyCode.Alpha4, "Hotbar cell 4."),
                ControlsConfig.Config.Bind<KeyCode>("Hotbar", "cell5", KeyCode.Alpha5, "Hotbar cell 5."),
                ControlsConfig.Config.Bind<KeyCode>("Hotbar", "cell6", KeyCode.Alpha6, "Hotbar cell 6."),
                ControlsConfig.Config.Bind<KeyCode>("Hotbar", "cell7", KeyCode.Alpha7, "Hotbar cell 7.")
            };
        }
    }
}

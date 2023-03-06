using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterControls
{
    public static class NewInputs
    {
        public static ConfigEntry<KeyCode> Ping = ControlsConfig.Config.Bind<KeyCode>("Ping", "ping", KeyCode.Mouse2, "Player ping.");
    }
}

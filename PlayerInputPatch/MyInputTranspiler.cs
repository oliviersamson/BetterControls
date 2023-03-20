using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterControls.PlayerInputPatch
{
    [HarmonyPatch(typeof(PlayerInput), "MyInput")]
    class MyInputTranspiler
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            CodeMatcher codeMatcher = new(instructions);

            codeMatcher.MatchForward(false, new CodeMatch(OpCodes.Ldc_I4_S));

            codeMatcher.SetInstruction(Transpilers.EmitDelegate<Func<KeyCode>>(
                () =>
                {
                    return NewInputs.Rotate.Value;
                }).WithLabels(codeMatcher.Labels));

            return codeMatcher.InstructionEnumeration();
        }
    }
}

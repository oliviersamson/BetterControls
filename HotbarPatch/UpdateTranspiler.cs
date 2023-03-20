using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace BetterControls.HotbarPatch
{
    [HarmonyPatch(typeof(Hotbar), "Update")]
    class UpdateTranspiler
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new CodeMatcher(instructions);

            codeMatcher = codeMatcher.MatchForward(false, new CodeMatch(OpCodes.Ldstr));

            List<Label> labels = codeMatcher.InstructionAt(0).ExtractLabels();

            codeMatcher = codeMatcher.RemoveInstructions(5);

            codeMatcher = codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_1).WithLabels(labels));

            codeMatcher = codeMatcher.InsertAndAdvance(Transpilers.EmitDelegate<Func<int,bool>>(
                (cellIndex) => { 

                    return Input.GetKeyDown(NewInputs.Hotbar.Cells[cellIndex - 1].Value);
                }));

            return codeMatcher.InstructionEnumeration();
        }
    }
}

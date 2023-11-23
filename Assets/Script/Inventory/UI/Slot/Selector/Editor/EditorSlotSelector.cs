using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Minefarm.Inventory.UI.Slot.EditorInspector
{
    [CustomEditor(typeof(SlotSelector))]
    public class EditorSlotSelector : Editor
    {
        SlotSelector script;
        private void OnEnable()
        {
            script = (SlotSelector)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Rename Slot")) script.RenameSlots();
        }
    }
}

using UnityEditor;
using UnityEngine;

namespace Game
{
    [InitializeOnLoad]
    public class HierarchyExtension
    {
        static HierarchyExtension() => EditorApplication.hierarchyWindowItemOnGUI += HierarchWindowOnGui;

        private static void HierarchWindowOnGui(int instanceID, Rect selectionRect)
        {
            // make rectangle
            var r = new Rect(selectionRect);
            r.x = 35;
            r.width = 18;

            // get objects
            Object o = EditorUtility.InstanceIDToObject(instanceID);
            var obj = o as GameObject;

            // drag toggle gui
            if (obj != null)
            {
                bool prev = obj.activeSelf;
                bool next = GUI.Toggle(r, obj.activeSelf, string.Empty);

                if (next != prev)
                {
					Undo.RecordObject(obj, $"Record {obj.name}");
					obj.SetActive(next);
					EditorUtility.SetDirty(obj);
				}
			}
        }
    }
}
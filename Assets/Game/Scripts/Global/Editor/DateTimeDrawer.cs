using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateTimeDrawer : OdinValueDrawer<DateTime>
{
	private string display;
	private object key = new object();

	protected override void DrawPropertyLayout(GUIContent label)
	{
		display = this.ValueEntry.SmartValue.ToString();

		SirenixEditorGUI.BeginShakeableGroup(key);

		display = SirenixEditorFields.TextField(label, display);

		if (DateTime.TryParse(display, out DateTime result))
		{
			this.ValueEntry.SmartValue = result;
		}
		else
		{
			SirenixEditorGUI.StartShakingGroup(key);
		}

		SirenixEditorGUI.EndShakeableGroup(key);
	}
}
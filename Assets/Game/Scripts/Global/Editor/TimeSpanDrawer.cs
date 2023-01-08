using System;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Sirenix.Utilities;

namespace Sirenix.OdinInspector.Editor.Drawers
{
	[OdinDrawer]
	public sealed class TimeSpanDrawer : OdinValueDrawer<TimeSpan>
	{
		protected override void DrawPropertyLayout(GUIContent label)
		{
			GUILayout.BeginHorizontal();
			if (label != null)
			{
				EditorGUI.PrefixLabel(GUIHelper.GetCurrentLayoutRect(), label);
				GUILayout.Space(EditorGUIUtility.labelWidth - 8);
			}

			GUIHelper.PushLabelWidth(42f);

			GUIHelper.PushIndentLevel(0);

			GUILayout.BeginVertical();
			{
				int days, hours, minutes, seconds;

				EditorGUI.BeginChangeCheck();
				{

					GUILayout.BeginHorizontal();
					{
						days = SirenixEditorFields.IntField(GUIHelper.TempContent("Days"), this.ValueEntry.SmartValue.Days);
						hours = SirenixEditorFields.IntField(GUIHelper.TempContent("Hours"), this.ValueEntry.SmartValue.Hours);
					}

					GUILayout.EndHorizontal();


					GUILayout.BeginHorizontal();
					{
						minutes = SirenixEditorFields.IntField(GUIHelper.TempContent("Mins"), this.ValueEntry.SmartValue.Minutes);
						seconds = SirenixEditorFields.IntField(GUIHelper.TempContent("Secs"), this.ValueEntry.SmartValue.Seconds);
					}

					GUILayout.EndHorizontal();
				}

				if (EditorGUI.EndChangeCheck())
				{
					this.ValueEntry.SmartValue = new TimeSpan(days, hours, minutes, seconds);
				}
			}

			GUILayout.EndVertical();
			GUIHelper.PopIndentLevel();
			GUIHelper.PopLabelWidth();
			GUILayout.EndHorizontal();
		}
	}
}
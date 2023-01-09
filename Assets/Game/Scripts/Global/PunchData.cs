using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PunchData", menuName = "Game/Effects/Punch")]
public class PunchData : ScriptableObject
{
	[HideLabel]
	public PunchSettings settings;
}
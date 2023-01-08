using Sirenix.OdinInspector;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers.LevelManager
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Game/Level")]
    public class LevelSettings : ScriptableObject
    {
		[ShowInInspector]
		public TimeSpan estimatePlatinaTime;
        [Space]
		[ShowInInspector]
		public TimeSpan estimateGoldTime;
        [Space]
        [ShowInInspector]
		public TimeSpan estimateSilverTime;
        [Space]
		[ShowInInspector]
		public TimeSpan estimateCooperTime;

		//bonus targets
	}
}
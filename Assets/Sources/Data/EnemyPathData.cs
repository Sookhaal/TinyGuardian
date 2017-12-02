using System.Collections.Generic;
using Components;
using DG.Tweening;
using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "EnemyPathData", menuName = "Data/EnemyPathData", order = 0)]
	public class EnemyPathData : ScriptableObject {
		public Vector3[] Waypoints;
		public float Duration;
		public PathType PathType;
	}
}
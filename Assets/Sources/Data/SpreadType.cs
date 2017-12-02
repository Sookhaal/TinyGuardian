using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Data {
	[CreateAssetMenu(fileName = "SpreadType", menuName = "Data/SpreadType", order = 0)]
	public class SpreadType : ScriptableObject {
		public List<Vector3> Offsets;
		public float VelocityCoef = 1f;
		public bool Sin;
	}
}
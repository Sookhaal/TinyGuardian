using System.Collections.Generic;
using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "SpreadType", menuName = "Data/SpreadType", order = 0)]
	public class SpreadType : ScriptableObject {
		public Spreads Type;
		public List<Vector3> Offsets;
		public float VelocityCoef = 1f;
		public bool Sin;
		public FloatData RateOfFire;
		public SpreadType ExplosionPattern;
	}

	public enum Spreads {
		Straight,
		Sin,
		Bomb,
		Explosion
	}
}
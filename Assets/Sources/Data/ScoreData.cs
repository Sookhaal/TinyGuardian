using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "ScoreData", menuName = "Data/ScoreData", order = 0)]
	public class ScoreData : ScriptableObject {
		public FloatData Score;
		public float Multiplier = 1f;
	}
}
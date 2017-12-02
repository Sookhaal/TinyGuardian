using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 0)]
	public class PlayerData : ScriptableObject {
		public SpreadType SpreadTypeStart;
		public SpreadType SpreadType;
		public FloatData HP;

		private void OnEnable() {
			SpreadType = SpreadTypeStart;
		}
	}
}
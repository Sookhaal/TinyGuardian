using System.Collections.Generic;
using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 0)]
	public class PlayerData : ScriptableObject {
		public SpreadType SpreadTypeStart;
		public SpreadType SpreadType;
		public List<SpreadType> SpreadTypesStart;
		public List<SpreadType> SpreadTypes;
		public FloatData HP;

		private void OnEnable() {
			SpreadType = SpreadTypeStart;
			SpreadTypes = new List<SpreadType>();
			foreach (var spreadType in SpreadTypesStart) {
				SpreadTypes.Add(spreadType);
			}
		}
	}
}
using System.Collections.Generic;
using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 0)]
	public class PlayerData : ScriptableObject {
		public List<SpreadType> SpreadTypesStart;
		public List<SpreadType> SpreadTypes;
		public FloatData HP;
		public FloatData MaxSpeed;

		private void OnEnable() {
			SpreadTypes = new List<SpreadType>();
			foreach (var spreadType in SpreadTypesStart) {
				SpreadTypes.Add(spreadType);
			}
		}
	}
}
using System.Collections.Generic;
using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 0)]
	public class PlayerData : ScriptableObject {
		public List<SpreadType> SpreadTypesStart;
		public List<SpreadType> SpreadTypes;
		public FloatData HP;
		public FloatData CurrentSpeed;
		public FloatData MinimumSpeed;
		public FloatData MaximumSpeed;

		private void OnEnable() {
			SpreadTypes = new List<SpreadType>();
			foreach (var spreadType in SpreadTypesStart) {
				SpreadTypes.Add(spreadType);
			}
		}

		public void AddToCurrentSpeed(float amount) {
			if (CurrentSpeed.Value + amount < MinimumSpeed.Value) {
				CurrentSpeed.SetValue(MinimumSpeed);
				return;
			}
			if (CurrentSpeed.Value + amount > MaximumSpeed.Value) {
				CurrentSpeed.SetValue(MaximumSpeed);
				return;
			}
			CurrentSpeed.ApplyChange(amount);
		}
	}
}
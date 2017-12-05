using System.Collections.Generic;
using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 0)]
	public class PlayerData : ScriptableObject {
		public List<SpreadType> SpreadTypesStart;
		public List<SpreadType> SpreadTypes;
		public float Acceleration;
		public float Deceleration;
		public FloatData HP;
		public FloatData CurrentSpeed;
		public FloatData MinimumSpeed;
		public FloatData MaximumSpeed;
		public int SelectedWeapon;
		public BulletType CurrentType;

		private void OnEnable() {
			SpreadTypes = new List<SpreadType>();
			foreach (var spreadType in SpreadTypesStart) {
				SpreadTypes.Add(spreadType);
			}
		}

		public void SetCurrentType(BulletType bulletType) {
			CurrentType = bulletType;
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
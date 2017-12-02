using System.Collections.Generic;
using Components;
using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "BulletsPool", menuName = "Data/BulletsPool", order = 0)]
	public class BulletsPool : ScriptableObject {
		public List<WeaponData> Weapons;
		[HideInInspector]
		public Bullet[] Bullets;
		public bool PoolSetupDone;

		public void ResetPool() {
			var index = 0;
			foreach (var weapon in Weapons) {
				for (var i = 0; i < weapon.MaxBullets; i++) {
					index++;
				}
			}

			PoolSetupDone = false;
			Bullets = new Bullet[index];
		}

		public void SetupPool() {
			var index = 0;
			foreach (var weapon in Weapons) {
				for (var i = 0; i < weapon.MaxBullets; i++) {
					var bullet = (Bullet) Instantiate(weapon.BulletPrefab);
					bullet.gameObject.SetActive(false);
					Bullets[index] = bullet;
					index++;
				}
			}

			PoolSetupDone = true;
		}
	}
}
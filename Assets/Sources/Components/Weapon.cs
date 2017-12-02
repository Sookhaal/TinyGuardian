using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Components {
	public class Weapon : MonoBehaviour {
		public List<WeaponData> Weapons;
		private int _selectedWeapon;
		[SerializeField]
		private BulletsPool _bulletsPool;
		private Transform _transform;
		private List<Bullet> _bulletsToShoot;

		private void Awake() {
			_transform = GetComponent<Transform>();
			_bulletsToShoot = new List<Bullet>();
		}

		private Bullet GetBullet() {
			if (!_bulletsPool.PoolSetupDone) {
				return null;
			}

			var bulletArrayOffset = 0;
			for (var i = 0; i < _selectedWeapon; i++) {
				bulletArrayOffset += Weapons[i].MaxBullets;
			}

			for (var i = 0 + bulletArrayOffset; i < _bulletsPool.Bullets.Length; i++) {
				var bullet = _bulletsPool.Bullets[i];
				if (!bullet.gameObject.activeInHierarchy && i < bulletArrayOffset + Weapons[_selectedWeapon].MaxBullets)
					return bullet;
			}
			return null;
		}

		public void SelectWeapon(int index) {
			_selectedWeapon = index;
		}

		public void EnemyShoot(EnemyData enemyData) {
			_bulletsToShoot.Clear();

			if (enemyData.ShootingPattern != null) {
				foreach (var spreadTypeOffset in enemyData.ShootingPattern.Offsets) {
					var bullet = GetBullet();
					if (bullet == null)
						continue;

					bullet.Sin = enemyData.ShootingPattern.Sin;
					bullet.SinCoef.y = enemyData.ShootingPattern.VelocityCoef * spreadTypeOffset.y;
					bullet.SinCoef.x = spreadTypeOffset.x;

					bullet.transform.position = _transform.position + (bullet.Sin ? Vector3.zero : spreadTypeOffset);
					bullet.StartingVelocity.y = spreadTypeOffset.y * enemyData.ShootingPattern.VelocityCoef;
					bullet.StartingVelocity.x = Weapons[_selectedWeapon].BulletVelocity;
					bullet.StartingVelocity.Normalize();
					bullet.StartingVelocity *= Weapons[_selectedWeapon].BulletVelocity;
					bullet.gameObject.SetActive(true);
					_bulletsToShoot.Add(bullet);
				}

			}

			foreach (var bullet in _bulletsToShoot) {
				bullet.StartingVelocity.x *= -1f;
				bullet.ShootTheBullet();
			}
		}

		public void Shoot(PlayerData playerData) {
			_bulletsToShoot.Clear();

			if (playerData.SpreadType == null) {
				var bullet = GetBullet();
				if (bullet == null)
					return;

				bullet.transform.position = _transform.position;
				bullet.gameObject.SetActive(true);
				_bulletsToShoot.Add(bullet);
			}

			if (playerData.SpreadType != null) {
				foreach (var spreadTypeOffset in playerData.SpreadType.Offsets) {
					var bullet = GetBullet();
					if (bullet == null)
						continue;

					bullet.Sin = playerData.SpreadType.Sin;
					bullet.SinCoef.y = playerData.SpreadType.VelocityCoef * spreadTypeOffset.y;
					bullet.SinCoef.x = spreadTypeOffset.x;

					bullet.transform.position = _transform.position + (bullet.Sin ? Vector3.zero : spreadTypeOffset);
					bullet.StartingVelocity.y = spreadTypeOffset.y * playerData.SpreadType.VelocityCoef;
					bullet.StartingVelocity.x = Weapons[_selectedWeapon].BulletVelocity;
					bullet.StartingVelocity.Normalize();
					bullet.StartingVelocity *= Weapons[_selectedWeapon].BulletVelocity;
					bullet.gameObject.SetActive(true);
					_bulletsToShoot.Add(bullet);
				}

			}

			foreach (var bullet in _bulletsToShoot) {
				bullet.ShootTheBullet();
			}
		}
	}
}
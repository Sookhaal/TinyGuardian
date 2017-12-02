using System.Collections.Generic;
using Assets.Sources.Data;
using UnityEngine;

namespace Assets.Sources.Components {
	public class Weapon : MonoBehaviour {
		[SerializeField]
		private List<WeaponData> _weapons;
		[SerializeField]
		private int _selectedWeapon;
		private List<Bullet> _bullets;
		private Transform _transform;
		private List<Bullet> _bulletsToShoot;

		private void Awake() {
			_transform = GetComponent<Transform>();
			_bulletsToShoot = new List<Bullet>();

			// Pooling
			_bullets = new List<Bullet>();
			foreach (var weapon in _weapons) {
				for (var i = 0; i < weapon.MaxBullets; i++) {
					var bullet = (Bullet) Instantiate(weapon.BulletPrefab);
					bullet.gameObject.SetActive(false);
					_bullets.Add(bullet);
				}
			}
		}

		private Bullet GetBullet() {
			var bulletArrayOffset = 0;
			for (var i = 0; i < _selectedWeapon; i++) {
				bulletArrayOffset += _weapons[i].MaxBullets;
			}

			for (var i = 0 + bulletArrayOffset; i < _bullets.Count; i++) {
				var bullet = _bullets[i];
				if (!bullet.gameObject.activeInHierarchy && i < bulletArrayOffset + _weapons[_selectedWeapon].MaxBullets)
					return bullet;
			}
			return null;
		}

		public void SelectWeapon(int index) {
			_selectedWeapon = index;
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

					bullet.transform.position = _transform.position + spreadTypeOffset;
					bullet.StartingVelocity.y = spreadTypeOffset.y;
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
using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Data;
using UnityEngine;

namespace Assets.Sources.Components {
	public class Weapon : MonoBehaviour {
		[SerializeField]
		private WeaponData _weaponData;
		private List<Bullet> _bullets;
		private Transform _transform;

		private void Awake() {
			_transform = GetComponent<Transform>();

			// Pooling
			_bullets = new List<Bullet>();
			for (var i = 0; i < _weaponData.MaxBullets; i++) {
				var bullet = (Bullet) Instantiate(_weaponData.BulletPrefab);
				bullet.gameObject.SetActive(false);
				_bullets.Add(bullet);
			}
		}

		private Bullet GetBullet() {
			return _bullets.FirstOrDefault(bullet => !bullet.gameObject.activeInHierarchy);
		}

		public void Shoot() {
			var bullet = GetBullet();
			if (bullet == null)
				return;

			bullet.transform.position = _transform.position;
			bullet.gameObject.SetActive(true);
			bullet.ShootTheBullet();
		}
	}
}
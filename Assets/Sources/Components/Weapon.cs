using System.Collections;
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
		private bool _canShoot = true;
		private SpreadType _spreadType;

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

		public void Shoot(SpreadType spreadType, bool comeFromEnemy) {
			_spreadType = spreadType;
			_bulletsToShoot.Clear();
			if (!_canShoot) {
				return;
			}
			_canShoot = false;

			switch (spreadType.Type) {
			case Spreads.Straight:
			case Spreads.Sin:
			case Spreads.Bomb:
				foreach (var spreadTypeOffset in spreadType.Offsets) {
					var bullet = GetBullet();
					if (bullet == null)
						continue;

					bullet.Sin = spreadType.Sin;
					bullet.SinCoef.y = spreadType.VelocityCoef * spreadTypeOffset.y;
					bullet.SinCoef.x = spreadTypeOffset.x;

					bullet.transform.position = _transform.position + (bullet.Sin ? Vector3.zero : spreadTypeOffset);
					bullet.StartingVelocity.y = spreadTypeOffset.y * spreadType.VelocityCoef;
					bullet.StartingVelocity.x = Weapons[_selectedWeapon].BulletVelocity;
					bullet.StartingVelocity.Normalize();
					bullet.StartingVelocity *= Weapons[_selectedWeapon].BulletVelocity;
					bullet.gameObject.SetActive(true);
					bullet.CanHurtPlayer = comeFromEnemy;
					if (spreadType.Type == Spreads.Bomb) {
						StartCoroutine(ExplodeBomb(bullet, bullet.CanHurtPlayer));
					}
					_bulletsToShoot.Add(bullet);
				}
				break;
			case Spreads.Explosion:
				foreach (var spreadTypeOffset in spreadType.Offsets) {
					var bullet = GetBullet();
					if (bullet == null)
						continue;

					bullet.transform.position = _transform.position;
					bullet.StartingVelocity = spreadTypeOffset;
					bullet.StartingVelocity.Normalize();
					bullet.StartingVelocity *= Weapons[_selectedWeapon].BulletVelocity;
					bullet.gameObject.SetActive(true);
					bullet.CanHurtPlayer = comeFromEnemy;
					_bulletsToShoot.Add(bullet);
				}
				break;
			default:
				break;
			}

			foreach (var bullet in _bulletsToShoot) {
				if (comeFromEnemy) {
					bullet.StartingVelocity.x *= -1f;
				}
				bullet.ShootTheBullet();
			}

			StartCoroutine(RateLimiter(spreadType.RateOfFire.Value));
		}

		public void Shoot(PlayerData playerData) {
			Shoot(playerData.SpreadType, false);
		}

		private IEnumerator RateLimiter(float seconds) {
			yield return new WaitForSeconds(seconds);
			_canShoot = true;
		}

		private IEnumerator ExplodeBomb(Bullet bullet, bool canHurtPlayer) {
			var go = new GameObject();
			var dupeWeapon = (Weapon) go.AddComponent(typeof(Weapon));
			dupeWeapon.Weapons = Weapons;
			dupeWeapon._spreadType = _spreadType;
			dupeWeapon._bulletsPool = _bulletsPool;
			dupeWeapon._selectedWeapon = _selectedWeapon;
			yield return new WaitForSeconds(1f);
			bullet.gameObject.SetActive(false);
			dupeWeapon._transform.position = bullet.transform.position;
			dupeWeapon.Shoot(dupeWeapon._spreadType.ExplosionPattern, canHurtPlayer);
			Destroy(dupeWeapon.gameObject);
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Components {
	public class Weapon : MonoBehaviour {
		public List<WeaponData> Weapons;
		public Spreads SpreadType;

		[SerializeField]
		private BulletsPool _bulletsPool;
		private int _selectedWeapon;
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

		private GameObject GetClosestEnemy() {
			var enemies = GameObject.FindGameObjectsWithTag("Enemy");
			if (enemies.Length == 0) {
				return null;
			}

			var closest = enemies[0];
			var distance = Mathf.Infinity;
			var position = _transform.position;
			foreach (var enemy in enemies) {
				var diff = enemy.transform.position - position;
				var curDistance = diff.sqrMagnitude;
				if (curDistance >= distance)
					continue;
				closest = enemy;
				distance = curDistance;
			}

			return closest;
		}

		public void Shoot(SpreadType spreadType, bool comeFromEnemy) {
			_spreadType = spreadType;
			_bulletsToShoot.Clear();
			if (!_canShoot) {
				return;
			}
			_canShoot = false;

			switch (spreadType.Type) {
			default:
				foreach (var spreadTypeOffset in spreadType.Offsets) {
					var bullet = GetBullet();
					if (bullet == null)
						continue;

					bullet.Sin = spreadType.Sin;
					bullet.SinCoef.y = spreadType.VelocityCoef * spreadTypeOffset.y;
					bullet.SinCoef.x = spreadTypeOffset.x;

					bullet.transform.position = _transform.position + (bullet.Sin ? Vector3.zero : spreadTypeOffset);
					switch (spreadType.Type) {
					case Spreads.LockOn:
						bullet.LockOnTarget = GetClosestEnemy();
						if (bullet.LockOnTarget) {
							bullet.StartingVelocity = bullet.LockOnTarget.transform.position - _transform.position;
							bullet.BulletVelocity = Weapons[_selectedWeapon].BulletVelocity;
						} else {
							bullet.StartingVelocity.y = spreadTypeOffset.y * spreadType.VelocityCoef;
							bullet.StartingVelocity.x = Weapons[_selectedWeapon].BulletVelocity;
						}
						break;
					case Spreads.AutoAim:
						var target = comeFromEnemy ? GameObject.FindGameObjectWithTag("Player") : GetClosestEnemy();
						bullet.StartingVelocity = target.transform.position - _transform.position;
						bullet.BulletVelocity = Weapons[_selectedWeapon].BulletVelocity;
						break;
					default:
						bullet.StartingVelocity.y = spreadTypeOffset.y * spreadType.VelocityCoef;
						bullet.StartingVelocity.x = Weapons[_selectedWeapon].BulletVelocity;
						break;
					}
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
			}

			foreach (var bullet in _bulletsToShoot) {
				if (comeFromEnemy && spreadType.Type != Spreads.AutoAim && spreadType.Type != Spreads.LockOn) {
					bullet.StartingVelocity.x *= -1f;
				}
				bullet.ShootTheBullet();
			}

			StartCoroutine(RateLimiter(spreadType.RateOfFire.Value));
		}

		public void Shoot(PlayerData playerData) {
			foreach (var spreadType in playerData.SpreadTypes) {
				if (spreadType == null || spreadType.Type != SpreadType) {
					continue;
				}

				Shoot(spreadType, false);
			}
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

		public void CheckHasPowerup(PlayerData playerData) {
			foreach (var spreadType in playerData.SpreadTypes) {
				if (spreadType.Type == SpreadType) {
					gameObject.SetActive(true);
					return;
				}
				gameObject.SetActive(false);
			}
		}
	}
}
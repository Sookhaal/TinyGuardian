using System.Collections;
using Data;
using UnityEngine;

namespace Components {
	public class Enemy : MonoBehaviour {
		public EnemyData EnemyData;
		public float HP;

		private Rigidbody2D _rigidbody2D;
		private bool _canShoot = true;
		[SerializeField]
		private Weapon Weapon;

		private void Awake() {
			_rigidbody2D = GetComponent<Rigidbody2D>();
			HP = EnemyData.StartingHP;
			Weapon.SelectWeapon(EnemyData.SelectedWeapon);
		}

		private void Update() {
			if (_canShoot) {
				Weapon.EnemyShoot(EnemyData);
				_canShoot = false;
				StartCoroutine(RateLimiter());
			}
		}

		private IEnumerator RateLimiter() {
			yield return new WaitForSeconds(EnemyData.TimeBetweenShot);
			_canShoot = true;
		}
	}
}
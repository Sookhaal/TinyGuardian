using System.Collections;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Components {
	public class Enemy : MonoBehaviour {
		public EnemyData EnemyData;
		public float HP;

		private Transform _transform;
		private bool _canShoot = true;
		[SerializeField]
		private Weapon Weapon;
		[SerializeField]
		private EnemyPathData _pathData;
		private Vector3 _basePosition;
		private TweenerCore<Vector3, Path, PathOptions> _pathTweener;

		private void Awake() {
			_transform = GetComponent<Transform>();
			_basePosition = _transform.position;
			HP = EnemyData.StartingHP;
			Weapon.SelectWeapon(EnemyData.SelectedWeapon);
			if (_pathData) {
				_pathTweener = _transform.DOPath(_pathData.Waypoints, _pathData.Duration, _pathData.PathType);
			}
		}

		private void Update() {
			if (_canShoot) {
				Weapon.Shoot(EnemyData.ShootingPattern, true);
				_canShoot = false;
				StartCoroutine(RateLimiter());
			}
		}

		public void DoThePath() {
			if (_pathTweener == null)
				return;

			_pathTweener.Kill();
			_transform.position = _basePosition;
			if (_pathData) {
				_pathTweener = _transform.DOPath(_pathData.Waypoints, _pathData.Duration, _pathData.PathType);
			}
		}

		public void Die() {
			Destroy(gameObject);
		}

		private IEnumerator RateLimiter() {
			yield return new WaitForSeconds(EnemyData.TimeBetweenShot);
			_canShoot = true;
		}
	}
}
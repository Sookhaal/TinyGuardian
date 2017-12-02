using System;
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
		private Vector3[] _waypoints;

		private void Awake() {
			_transform = GetComponent<Transform>();
			_basePosition = _transform.position;
			HP = EnemyData.StartingHP;
			Weapon.SelectWeapon(EnemyData.SelectedWeapon);
			DoThePath();
		}

		private void Update() {
			if (_canShoot) {
				Weapon.Shoot(EnemyData.ShootingPattern, true);
				_canShoot = false;
				StartCoroutine(RateLimiter());
			}
		}

		public void DoThePath() {
			_pathTweener?.Kill();

			_transform.position = _basePosition;
			_waypoints = new Vector3[_pathData.Waypoints.Length];

			for (var i = 0; i < _waypoints.Length; i++) {
				_waypoints[i] = _basePosition + _pathData.Waypoints[i];
			}

			if (_pathData) {
				_pathTweener = _transform.DOPath(_waypoints, _pathData.Duration, _pathData.PathType);
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
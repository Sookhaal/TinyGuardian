using System;
using System.Collections;
using System.Runtime.InteropServices;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Components {
	public class Enemy : MonoBehaviour {
		public Enemy FollowThisGuy;
		public EnemyData EnemyData;
		public float HP;
		public float Delay;

		private Transform _transform;
		private bool _canShoot;
		[SerializeField]
		private Weapon Weapon;
		[SerializeField]
		private EnemyPathData _pathData;
		private Vector3 _basePosition;
		private TweenerCore<Vector3, Path, PathOptions> _pathTweener;
		private Vector3[] _waypoints;
		private ScoreData _score;
		private GameEvent _checkBlockDone;

		private void Awake() {
			_transform = GetComponent<Transform>();
			_basePosition = _transform.position;
			_score = (ScoreData) Resources.Load("Global/Data/ScoreData");
			_checkBlockDone = (GameEvent) Resources.Load("EnemyBlocks/Events/CheckBlockDone");
			HP = EnemyData.StartingHP;
			Weapon.SelectWeapon(EnemyData.SelectedWeapon);
			if (!FollowThisGuy) {
				DoThePath();
			} else {
				_basePosition = FollowThisGuy.transform.position;
				Delay += FollowThisGuy.Delay;
				DoThePath();
			}

			StartCoroutine(WaitDelay());
		}

		private void Update() {
			if (!_canShoot)
				return;
			Weapon.Shoot(EnemyData.ShootingPattern, true);
			_canShoot = false;
			StartCoroutine(RateLimiter());
		}

		public void DoThePath() {
			_pathTweener?.Kill();

			_transform.position = _basePosition;
			_waypoints = new Vector3[_pathData.Waypoints.Length];

			for (var i = 0; i < _waypoints.Length; i++) {
				_waypoints[i] = _basePosition + _pathData.Waypoints[i];
			}

			_pathTweener = _transform.DOPath(_waypoints, _pathData.Duration, _pathData.PathType)
				.SetDelay(Delay)
				.OnComplete(Die);
		}

		public void AddScore(bool useBonus = false) {
			if (useBonus) {
				_score.Score.ApplyChange((EnemyData.ScoreValue + EnemyData.BonusScore) * _score.Multiplier);
			} else {
				_score.Score.ApplyChange(EnemyData.ScoreValue * _score.Multiplier);
			}

			Die();
		}

		public void Die() {
			_checkBlockDone.Raise();
			Destroy(gameObject);
		}

		private void OnTriggerExit2D(Collider2D collider) {
			if (collider.tag == "Boundaries") {
				gameObject.SetActive(false);
			}
		}

		private IEnumerator WaitDelay() {
			yield return new WaitForSeconds(Delay);
			_canShoot = true;
		}

		private IEnumerator RateLimiter() {
			yield return new WaitForSeconds(EnemyData.TimeBetweenShot);
			_canShoot = true;
		}
	}
}
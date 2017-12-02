using Data;
using UnityEngine;

namespace Components {
	public class Bullet : MonoBehaviour {
		public BulletType BulletType;
		public Vector2 StartingVelocity;
		public bool Sin;
		public Vector2 SinCoef;
		public bool CanHurtPlayer;

		private Vector3 _startingPosition;
		private Rigidbody2D _rigidbody2D;
		private Transform _transform;

		private void Awake() {
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_transform = GetComponent<Transform>();
			StartingVelocity = new Vector2(0f, 0f);
		}

		public void ShootTheBullet() {
			_startingPosition = _transform.position;
			_rigidbody2D.velocity = StartingVelocity;
		}

		private void FixedUpdate() {
			if (!Sin) {
				return;
			}

			StartingVelocity.y = Mathf.Cos(_startingPosition.x - transform.position.x / SinCoef.x) * SinCoef.y;
			_rigidbody2D.velocity = StartingVelocity;
		}

		private void Update() {
			var v = _rigidbody2D.velocity;
			var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
			_transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}

		private void OnTriggerExit2D(Collider2D collider) {
			if (collider.tag == "Boundaries") {
				gameObject.SetActive(false);
			}

			switch (CanHurtPlayer) {
			case true:
				if (collider.tag == "Player") {
					var player = collider.GetComponent<Player>();
					player.PlayerData.HP.ApplyChange(-1f);
				}
				break;
			case false:
				if (collider.tag == "Enemy") {
					var enemy = collider.GetComponent<Enemy>();
					enemy.HP -= 1.1f;
					if (enemy.HP < 0f) {
						enemy.AddScore(enemy.EnemyData.EnemyType.BonusDamageType == BulletType);
					}
				}
				break;
			default:
				break;
			}
		}
	}
}
﻿using Data;
using DG.Tweening;
using UnityEngine;

namespace Components {
	public class Bullet : MonoBehaviour {
		public BulletType BulletType;
		public Vector2 StartingVelocity;
		public bool Sin;
		public Vector2 SinCoef;
		public bool CanHurtPlayer;
		public GameObject LockOnTarget;
		public float BulletVelocity;
		public float Damage;

		private Vector3 _startingPosition;
		private Rigidbody2D _rigidbody2D;
		private Transform _transform;
		private SpriteRenderer _renderer;

		private void Awake() {
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_transform = GetComponent<Transform>();
			_renderer = GetComponent<SpriteRenderer>();
			StartingVelocity = new Vector2(0f, 0f);
		}

		public void ShootTheBullet() {
			_startingPosition = _transform.position;
			_rigidbody2D.velocity = StartingVelocity;
		}

		private void FixedUpdate() {
			if (LockOnTarget) {
				StartingVelocity = LockOnTarget.transform.position - _transform.position;
				StartingVelocity.Normalize();
				StartingVelocity *= BulletVelocity;
				_rigidbody2D.velocity = StartingVelocity;
				return;
			}

			if (!Sin) {
				return;
			}

			StartingVelocity.y = Mathf.Cos(_startingPosition.x - transform.position.x * SinCoef.x) * SinCoef.y;

			StartingVelocity.x = SinCoef.x * Mathf.Sign(StartingVelocity.x);
			_rigidbody2D.velocity = StartingVelocity;
		}

		private void Update() {
			var v = _rigidbody2D.velocity;
			var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
			_transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			if (!_renderer.IsVisibleFrom(Camera.main)) {
				gameObject.SetActive(false);
			}
		}

		private void OnTriggerExit2D(Collider2D collider) {
			if (collider.tag == "Boundaries") {
				gameObject.SetActive(false);
			}
		}

		private void OnTriggerEnter2D(Collider2D collider) {
			switch (CanHurtPlayer) {
			case true:
				if (collider.tag == "Player") {
					var player = collider.GetComponent<Player>();
					if (player.PlayerData.CurrentType != BulletType) {
						// Damage
						player.PlayerData.HP.ApplyChange(-Damage);
					} else {
						if (player.PlayerData.HP.Value + Damage * 0.25f > player.PlayerData.HP.StartingValue) {
							// Set to max
							player.PlayerData.HP.SetValue(player.PlayerData.HP.StartingValue);
						} else {
							// Heal
							player.PlayerData.HP.ApplyChange(Damage * 0.25f);
						}
					}

					player.CheckDeath();
					gameObject.SetActive(false);
				}
				break;
			case false:
				if (collider.tag == "Enemy") {
					var enemy = collider.GetComponent<Enemy>();
					if (BulletType.BonusDamageType == enemy.EnemyData.EnemyType) {
						enemy.HP -= Damage * 2f;
					} else {
						enemy.HP -= Damage;
					}

					if (enemy.HP < 0f) {
						enemy.AddScore(BulletType.BonusDamageType == enemy.EnemyData.EnemyType);
					}
					gameObject.SetActive(false);
				}
				break;
			default:
				break;
			}
		}
	}
}
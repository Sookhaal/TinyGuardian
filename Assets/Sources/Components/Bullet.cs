using System.Collections;
using Assets.Sources.Data;
using UnityEngine;

namespace Assets.Sources.Components {
	public class Bullet : MonoBehaviour {
		public Vector2 StartingVelocity;
		public bool Sin;
		public Vector2 SinCoef;

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
		}
	}
}
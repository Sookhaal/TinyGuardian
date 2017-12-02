using System.Collections;
using Assets.Sources.Data;
using UnityEngine;

namespace Assets.Sources.Components {
	public class Bullet : MonoBehaviour {
		public Vector2 StartingVelocity;

		private Rigidbody2D _rigidbody2D;

		private void Awake() {
			_rigidbody2D = GetComponent<Rigidbody2D>();
			StartingVelocity = new Vector2(10f, 0f);
		}

		public void ShootTheBullet() {
			_rigidbody2D.velocity = StartingVelocity;
		}

		private void OnTriggerExit2D(Collider2D collider) {
			if (collider.tag == "Boundaries") {
				gameObject.SetActive(false);
			}
		}
	}
}
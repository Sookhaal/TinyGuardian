using UnityEngine;

namespace Components {
	public class Enemy : MonoBehaviour {
		private Rigidbody2D _rigidbody2D;

		private void Awake() {
			_rigidbody2D = GetComponent<Rigidbody2D>();
		}
	}
}
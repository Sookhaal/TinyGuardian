using UnityEngine;

namespace Assets.Sources.Components {
	public class Player : MonoBehaviour {
		[SerializeField]
		private FloatData _maxSpeed;
		[SerializeField]
		private GameEvent _playerShoot;
		private Transform _transform;
		private Rigidbody2D _rigidbody2D;
		private Vector2 _input;

		private void Awake() {
			_transform = GetComponent<Transform>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_input = new Vector2();
		}

		private void Update() {
			_input.x = Input.GetAxis("Horizontal") * _maxSpeed.Value;
			_input.y = Input.GetAxis("Vertical") * _maxSpeed.Value;

			if (Input.GetButtonDown("Space")) {
				_playerShoot.Raise();
			}
		}

		private void FixedUpdate() {
			_rigidbody2D.velocity = _input;
		}
	}
}
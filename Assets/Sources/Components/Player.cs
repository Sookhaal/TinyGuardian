using System.Collections;
using UnityEngine;

namespace Assets.Sources.Components {
	public class Player : MonoBehaviour {
		[SerializeField]
		private FloatData _timeBetweenShot;
		[SerializeField]
		private FloatData _maxSpeed;
		[SerializeField]
		private GameEvent _playerShoot;

		// Weapon Selection
		[SerializeField]
		private GameEvent _selectEarthWeapon;
		[SerializeField]
		private GameEvent _selectFireWeapon;
		[SerializeField]
		private GameEvent _selectThunderWeapon;
		[SerializeField]
		private GameEvent _selectWaterWeapon;

		private Transform _transform;
		private Rigidbody2D _rigidbody2D;
		private Vector2 _input;
		private bool _canShoot = true;

		private void Awake() {
			_transform = GetComponent<Transform>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_input = new Vector2();
		}

		private void Update() {
			_input.x = Input.GetAxis("Horizontal") * _maxSpeed.Value;
			_input.y = Input.GetAxis("Vertical") * _maxSpeed.Value;

			if (Input.GetButton("Space") && _canShoot) {
				_playerShoot.Raise();
				_canShoot = false;
				StartCoroutine(RateLimiter());
			}

			if (Input.GetButtonDown("SelectEarthWeapon")) {
				_selectEarthWeapon.Raise();
			}

			if (Input.GetButtonDown("SelectFireWeapon")) {
				_selectFireWeapon.Raise();
			}

			if (Input.GetButtonDown("SelectThunderWeapon")) {
				_selectThunderWeapon.Raise();
			}

			if (Input.GetButtonDown("SelectWaterWeapon")) {
				_selectWaterWeapon.Raise();
			}
		}

		private IEnumerator RateLimiter() {
			yield return new WaitForSeconds(_timeBetweenShot.Value);
			_canShoot = true;
		}

		private void FixedUpdate() {
			_rigidbody2D.velocity = _input;
		}
	}
}
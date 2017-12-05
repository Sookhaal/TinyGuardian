using System.Collections;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Components {
	public class Player : MonoBehaviour {
		public PlayerData PlayerData;
		[SerializeField]
		private UnityEvent _pause;
		[SerializeField]
		private GameEvent _deathEvent;
		[SerializeField]
		private Sprite[] _gogoSprites;
		[SerializeField]
		private FloatData _timeBetweenShot;
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

		[SerializeField]
		private BulletsPool _bulletsPool;
		[SerializeField]
		private Weapon _weapon;

		private Rigidbody2D _rigidbody2D;
		private SpriteRenderer _spriteRenderer;
		private Vector2 _input;
		private bool _canShoot = true;
		private int _gogoFatness;
		private bool _dead;
		private bool _paused;
		private Transform _transform;
		private Vector3 _correctedPosition;

		private void Awake() {
			_transform = GetComponent<Transform>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_input = new Vector2();
			_correctedPosition = new Vector3();
			_bulletsPool.SetupPool();
			SceneManager.LoadSceneAsync("GameUI", LoadSceneMode.Additive);
		}

		private void Start() {
			PlayerData.SelectedWeapon = 0;
			if (PlayerData.SelectedWeapon == 0) {
				_selectFireWeapon.Raise();
			} else {
				_selectWaterWeapon.Raise();
			}
		}

		private void Update() {
			if (Input.GetAxis("Horizontal") > 0f || Input.GetAxis("Horizontal") < 0f)
				_input.x = Mathf.Lerp(_input.x, Input.GetAxis("Horizontal") * PlayerData.CurrentSpeed.Value, Time.deltaTime * PlayerData.Acceleration);
			else
				_input.x = Mathf.Lerp(_input.x, Input.GetAxis("Horizontal") * PlayerData.CurrentSpeed.Value, Time.deltaTime * PlayerData.Deceleration);
			if (Input.GetAxis("Horizontal") > 0f || Input.GetAxis("Horizontal") < 0f)
				_input.y = Mathf.Lerp(_input.y, Input.GetAxis("Vertical") * PlayerData.CurrentSpeed.Value, Time.deltaTime * PlayerData.Acceleration);
			else
				_input.y = Mathf.Lerp(_input.y, Input.GetAxis("Vertical") * PlayerData.CurrentSpeed.Value, Time.deltaTime * PlayerData.Deceleration);

			if (Input.GetButton("Space") && _canShoot) {
				_playerShoot.Raise();
				_canShoot = false;
				StartCoroutine(RateLimiter());
			}

			if (Input.GetButtonDown("SelectEarthWeapon") || Input.GetButtonDown("SelectFireWeapon") || Input.GetButtonDown("SelectThunderWeapon") || Input.GetButtonDown("SelectWaterWeapon")) {
				PlayerData.SelectedWeapon = PlayerData.SelectedWeapon == 0 ? 1 : 0;
				if (PlayerData.SelectedWeapon == 0) {
					_selectFireWeapon.Raise();
				} else {
					_selectWaterWeapon.Raise();
				}
			}

			if (_transform.position.x > 10f) {
				_correctedPosition.x = 10f;
				_correctedPosition.y = _transform.position.y;
				_correctedPosition.z = _transform.position.z;
				_transform.position = _correctedPosition;
			}

			if (_transform.position.x < -10f) {
				_correctedPosition.x = -10f;
				_correctedPosition.y = _transform.position.y;
				_correctedPosition.z = _transform.position.z;
				_transform.position = _correctedPosition;
			}

			if (_transform.position.y < -5.5f) {
				_correctedPosition.x = _transform.position.x;
				_correctedPosition.y = -5.5f;
				_correctedPosition.z = _transform.position.z;
				_transform.position = _correctedPosition;
			}

			if (_transform.position.y > 5.5f) {
				_correctedPosition.x = _transform.position.x;
				_correctedPosition.y = 5.5f;
				_correctedPosition.z = _transform.position.z;
				_transform.position = _correctedPosition;
			}

			if (!Input.GetButtonDown("Pause") || _paused) {
				return;
			}

			_paused = true;
			_pause.Invoke();
		}

		private IEnumerator RateLimiter() {
			yield return new WaitForSeconds(_timeBetweenShot.Value);
			_canShoot = true;
		}

		private void FixedUpdate() {
			_rigidbody2D.velocity = _input;
		}

		public void FeedGogo(int amount) {
			if (_gogoFatness + amount < 0 || _gogoFatness + amount >= _gogoSprites.Length) {
				return;
			}
			_gogoFatness += amount;
			_spriteRenderer.sprite = _gogoSprites[_gogoFatness];
		}

		public void CheckDeath() {
			if (PlayerData.HP.Value > 0f || _dead)
				return;
			_dead = true;
			_deathEvent.Raise();
		}
	}
}
﻿using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components {
	public class Player : MonoBehaviour {
		public PlayerData PlayerData;
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

		private Transform _transform;
		private Rigidbody2D _rigidbody2D;
		private SpriteRenderer _spriteRenderer;
		private Vector2 _input;
		private bool _canShoot = true;
		private int _gogoFatness;

		private void Awake() {
			_transform = GetComponent<Transform>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_input = new Vector2();
			_bulletsPool.SetupPool();
			SceneManager.LoadSceneAsync("GameUI", LoadSceneMode.Additive);
		}

		private void Update() {
			_input.x = Input.GetAxis("Horizontal") * PlayerData.CurrentSpeed.Value;
			_input.y = Input.GetAxis("Vertical") * PlayerData.CurrentSpeed.Value;

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

		public void FeedGogo(int amount) {
			if (_gogoFatness + amount < 0 || _gogoFatness + amount >= _gogoSprites.Length) {
				return;
			}
			_gogoFatness += amount;
			_spriteRenderer.sprite = _gogoSprites[_gogoFatness];
		}
	}
}
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using Random = System.Random;

namespace Components {
	public class PowerupManager : MonoBehaviour {
		[SerializeField]
		private EnemyBlock _blockManager;
		private List<PowerUp> _powerUps;
		private Random _rng;
		private int _powerUpIndex;

		private void Awake() {
			_rng = new Random();
			_powerUps = Resources.LoadAll<PowerUp>("Powerups/Prefabs").ToList();
			_powerUps = _powerUps.OrderBy(x => _rng.Next()).ToList();
			_blockManager.SpawnBlock();
		}

		public void SpawnPowerup() {
			var _powerup = Instantiate(_powerUps[_powerUpIndex]);
			_powerup.transform.position = new Vector3(12f, 0f, 0f);
			_powerup.GetComponent<Rigidbody2D>().velocity = new Vector2(-3f, 0f);
			_powerUpIndex++;

			if (_powerUpIndex < _powerUps.Count) {
				return;
			}

			_powerUpIndex = 0;
			_powerUps = _powerUps.OrderBy(x => _rng.Next()).ToList();
		}
	}
}
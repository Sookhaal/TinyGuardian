using System;
using System.Linq;
using Data;
using UnityEngine;

namespace Components {
	public class PowerUp : MonoBehaviour {
		public PowerupType PowerupType;
		public PowerupData PowerupData;
		public GameEvent DoPowerupEvent;
		public GameEvent DoPowerdownEvent;

		private void OnTriggerEnter2D(Collider2D collider) {
			if (collider.tag != "Player")
				return;

			var player = collider.GetComponent<Player>();
			for (var i = 0; i < player.PlayerData.SpreadTypes.Count; i++) {
				if (!player.PlayerData.SpreadTypes[i]) {
					continue;
				}

				if (player.PlayerData.SpreadTypes[i].Type != PowerupData.SpreadProgression[0].Type) {
					if (i == player.PlayerData.SpreadTypes.Count) {
						player.PlayerData.SpreadTypes.Add(PowerupData.SpreadProgression[0]);
					}

					continue;
				}
				player.PlayerData.SpreadTypes[i] = PowerupData.SpreadProgression[0];
			}

			player.PlayerData.SpreadTypes[0] = PowerupData.SpreadProgression[0];
			switch (PowerupType) {
			case PowerupType.UP:
				DoPowerupEvent.Raise();
				break;
			case PowerupType.DOWN:
				DoPowerdownEvent.Raise();
				break;
			default:
				break;
			}
			Destroy(gameObject);
		}
	}

	public enum PowerupType {
		UP,
		DOWN
	}
}
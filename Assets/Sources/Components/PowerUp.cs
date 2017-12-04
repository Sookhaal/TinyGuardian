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
		public int ProgressionIndex;

		private void OnTriggerEnter2D(Collider2D collider) {
			if (collider.tag != "Player")
				return;

			var player = collider.GetComponent<Player>();

			for (var i = 0; i < player.PlayerData.SpreadTypes.Count; i++) {
				if (!player.PlayerData.SpreadTypes[i]) {
					continue;
				}
				if (player.PlayerData.SpreadTypes[i].Type != PowerupData.SpreadProgression[ProgressionIndex].Type) {
					if (i == player.PlayerData.SpreadTypes.Count) {
						player.PlayerData.SpreadTypes.Add(PowerupData.SpreadProgression[ProgressionIndex]);
					}

					continue;
				}
				for (var progressionIndex = 0; progressionIndex < PowerupData.SpreadProgression.Count; progressionIndex++) {
					if (player.PlayerData.SpreadTypes[i] == PowerupData.SpreadProgression[progressionIndex]) {
						ProgressionIndex = progressionIndex;
					}
				}
				switch (PowerupType) {
				case PowerupType.UP:
					ProgressionIndex++;
					break;
				case PowerupType.DOWN:
					ProgressionIndex--;
					break;
				default:
					break;
				}

				if (ProgressionIndex >= PowerupData.SpreadProgression.Count) {
					ProgressionIndex = PowerupData.SpreadProgression.Count - 1;
				}

				if (ProgressionIndex < 0) {
					ProgressionIndex = 0;
				}

				player.PlayerData.SpreadTypes[i] = PowerupData.SpreadProgression[ProgressionIndex];
			}

			player.PlayerData.SpreadTypes[0] = PowerupData.SpreadProgression[ProgressionIndex];
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

		private void OnTriggerExit2D(Collider2D collider) {
			if (collider.tag == "Boundaries") {
				Destroy(gameObject);
			}
		}
	}

	public enum PowerupType {
		UP,
		DOWN
	}
}
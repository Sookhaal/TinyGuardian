using Data;
using UnityEngine;

namespace Components {
	public class GameManager : MonoBehaviour {
		public GameStateData GameStateData;
		public GameEvent LoadSplash;

		private void Start() {
			GameStateData.UpdateState();
		}
	}
}
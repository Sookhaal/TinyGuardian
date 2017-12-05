using Data;
using UnityEngine;

namespace Components {
	public class Bubble : MonoBehaviour {
		[SerializeField]
		private Color[] _colors;
		private SpriteRenderer _spriteRender;

		private void Awake() {
			_spriteRender = GetComponent<SpriteRenderer>();
		}

		public void ChangeColor(PlayerData playerData) {
			if (playerData.SelectedWeapon < _colors.Length) {
				_spriteRender.color = _colors[playerData.SelectedWeapon];
			}
		}
	}
}
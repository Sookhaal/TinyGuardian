using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Components {
	public class ElementFader : MonoBehaviour {
		private Image _image;
		private Color _color;

		private void Awake() {
			_image = GetComponent<Image>();
		}

		public void SetColor(string hex) {
			ColorUtility.TryParseHtmlString(hex, out _color);
			_image.DOColor(_color, 0.2f);
		}
	}
}
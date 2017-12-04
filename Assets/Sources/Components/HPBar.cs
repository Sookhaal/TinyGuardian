using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Components {
	public class HPBar : MonoBehaviour {
		public FloatData HP;

		private Image _image;
		private void Awake() {
			_image = GetComponent<Image>();
		}

		private void Update() {
			_image.fillAmount = HP.Value / HP.StartingValue;
		}
	}
}
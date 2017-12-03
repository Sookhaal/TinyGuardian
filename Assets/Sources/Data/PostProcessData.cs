using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace Data {
	[CreateAssetMenu(fileName = "PostProcessData", menuName = "Data/PostProcessData", order = 0)]
	public class PostProcessData : ScriptableObject {
		public PostProcessProfile PostProcessProfile;
		public float DefaultExposure;
		public float NewExposure;

		public void SetExposure() {
			ColorGrading grading;
			PostProcessProfile.TryGetSettings<ColorGrading>(out grading);
			if (!grading) {
				return;
			}

			grading.postExposure.Override(NewExposure);
		}

		public void SetExposure(Slider slider) {
			NewExposure = slider.value;
			SetExposure();
		}

		private void OnEnable() {
			NewExposure = DefaultExposure;
			SetExposure();
		}
	}
}
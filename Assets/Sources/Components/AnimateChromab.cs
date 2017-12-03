using Data;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Components {
	public class AnimateChromab : MonoBehaviour {
		public PostProcessProfile PostProcessProfile;
		public float Speed;
		private float _value;
		private ChromaticAberration _chromaticAberration;
		private Vignette _vignette;
		private float _t;

		private void Awake() {
			PostProcessProfile.TryGetSettings<ChromaticAberration>(out _chromaticAberration);
			PostProcessProfile.TryGetSettings<Vignette>(out _vignette);
		}

		private void Update() {
			_t += Time.deltaTime;
			_chromaticAberration.intensity.Interp(0f, 1f, _t * Speed);
			_vignette.intensity.Interp(0.1f, 1f, _t * Speed * 0.2f);
		}
	}
}
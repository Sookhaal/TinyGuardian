using UnityEngine;

[CreateAssetMenu(fileName = "FadeData", menuName = "Data/FadeData", order = 0)]
public class FadeData : ScriptableObject {
	[SerializeField]
	private float _startingFadeDuration = 1f;
	[SerializeField]
	private Color _startingFadeColor;

	public bool AutoAlpha = true;
	public float FadeDuration = 1f;
	public Color FadeColor;

	private void OnEnable() {
		FadeDuration = _startingFadeDuration;
		FadeColor = _startingFadeColor;
	}

	public void SetColor(Color color) {
		FadeColor = color;
	}

	public void SetRed(int red) {
		FadeColor.r = red / 255f;
	}

	public void SetGreen(int green) {
		FadeColor.g = green / 255f;
	}

	public void SetBlue(int blue) {
		FadeColor.b = blue / 255f;
	}

	public void SetAlpha(float alpha) {
		FadeColor.a = alpha;
	}

	public void SetDuration(float duration) {
		FadeDuration = duration;
	}
}
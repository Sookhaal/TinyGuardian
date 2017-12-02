using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour {
	public FadeData FadeData;
	public GameEvent FadeEnded;

	[SerializeField]
	private float _startingAlpha = 0f;
	[SerializeField]
	private Image _fader;
	private Coroutine _fadeCoroutine;
	private Tweener _faderBlender;

	private void Awake() {
		var color = _fader.color;
		color.a = _startingAlpha;
		_fader.color = color;
	}

	public void Fade(float delay = 0f) {
		if (_fadeCoroutine != null) {
			StopCoroutine(_fadeCoroutine);
		}
		if (_faderBlender != null) {
			_faderBlender.Kill();
		}

		_fadeCoroutine = StartCoroutine(DoTheFade(delay));
	}

	public void FadeToColor(int r, int g, int b, float a, float delay) {
		FadeData.FadeColor.r = r / 255f;
		FadeData.FadeColor.g = g / 255f;
		FadeData.FadeColor.b = b / 255f;
		FadeData.FadeColor.a = a;
		Fade(delay);
	}

	private IEnumerator DoTheFade(float delay) {
		yield return new WaitForSeconds(delay);
		if (FadeData.FadeColor.a > 0.5f && FadeData.AutoAlpha) {
			FadeData.FadeColor.a = 0f;
		} else if (FadeData.FadeColor.a < 0.5f && FadeData.AutoAlpha) {
			FadeData.FadeColor.a = 1f;
		}

		_faderBlender = _fader.DOBlendableColor(FadeData.FadeColor, FadeData.FadeDuration);
		yield return new WaitForSeconds(FadeData.FadeDuration);
		FadeEnded.Raise();
	}

	public IEnumerator ChangeScreen(ScreenData screenData) {
		var loadScreenEvent = (GameEvent)Resources.Load($"Global/Event/Screens/Load{screenData.name}");
		loadScreenEvent.Raise();
		yield return null;
	}
}
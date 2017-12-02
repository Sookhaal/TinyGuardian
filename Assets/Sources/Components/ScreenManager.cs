using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour {
	public ScreenData CurrentScreen;
	private GameEvent _fadeStart;
	private FadeData _fadeData;

	private void Awake() {
		_fadeData = (FadeData)Resources.Load("Global/Data/BlackFadeData");
		_fadeStart = (GameEvent)Resources.Load("Global/Event/StartFade");
	}

	private void Start() {
		var loadScreenEvent = (GameEvent)Resources.Load($"Global/Screens/Event/Load{CurrentScreen.name}");
		loadScreenEvent.Raise();
	}

	public void LoadScene(ScreenData screenData) {
		StartCoroutine(LoadSceneFade(screenData));
	}

	public IEnumerator LoadSceneFade(ScreenData screenData) {
		if (!screenData.LetPreviousScreenFade) {
			SceneManager.LoadSceneAsync(screenData.name);
			yield return null;
		}

		if (screenData.AutoFadeScreen) {
			_fadeStart.Raise();
			yield return new WaitForSeconds(_fadeData.FadeDuration + 2f);
		}

		_fadeStart.Raise();
		yield return new WaitForSeconds(_fadeData.FadeDuration);

		SceneManager.LoadSceneAsync(screenData.name);
	}
}
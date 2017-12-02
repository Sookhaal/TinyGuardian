using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameStateData", menuName = "Data/GameState", order = 0)]
public class GameStateData : ScriptableObject {
	public GameEvent FadeEvent;

	public ScreenData NextScreen;
	public ScreenData CurrentScreen;

	[SerializeField]
	private ScreenData _startNextScreen;
	[SerializeField]
	private ScreenData _startCurrentScreen;

	private void OnEnable() {
		NextScreen = _startNextScreen;
		CurrentScreen = _startCurrentScreen;
	}

	public void UpdateState() {
		if (NextScreen.name != CurrentScreen.name) {
			ChangeScreen(NextScreen);
		}
	}

	public void ChangeScreen(ScreenData screenData) {
		CurrentScreen = screenData;
		SceneManager.LoadScene(screenData.name);
		var loadScreenEvent = (GameEvent)Resources.Load($"Global/Screens/Event/Load{screenData.name}");
		loadScreenEvent.Raise();
	}

	public void ExitGame() {
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}
}
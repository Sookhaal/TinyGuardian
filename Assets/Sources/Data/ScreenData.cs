using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "ScreenData", menuName = "Data/ScreenData", order = 0)]
	public class ScreenData : ScriptableObject {
		public bool LetPreviousScreenFade;
		public bool AutoFadeScreen = true;
	}
}
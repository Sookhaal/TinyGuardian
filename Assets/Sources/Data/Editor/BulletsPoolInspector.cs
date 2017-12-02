using UnityEditor;
using UnityEngine;

namespace Data {
	[CustomEditor(typeof(BulletsPool))]
	public class BulletsPoolInspector : Editor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			var pool = target as BulletsPool;
			if (GUILayout.Button("ResetPool")) {
				pool.ResetPool();
			}
		}
	}
}
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent))]
public class EventEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        GUI.enabled = Application.isPlaying;

        var evnt = target as GameEvent;
        if (GUILayout.Button("Raise")) {
            evnt.Raise();
        }
    }
}
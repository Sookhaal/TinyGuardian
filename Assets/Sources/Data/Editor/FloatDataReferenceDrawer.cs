using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FloatDataReference))]
public class FloatDataReferenceDrawer : PropertyDrawer {
    private readonly string[] _popupOptions = {"Use Constant", "Use Variable"};
    private GUIStyle _popupStyle;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (_popupStyle == null) {
            _popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            _popupStyle.imagePosition = ImagePosition.ImageOnly;
        }

        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        var useConstant = property.FindPropertyRelative("UseConstant");
        var constantValue = property.FindPropertyRelative("ConstantValue");
        var data = property.FindPropertyRelative("Data");

        var buttonRect = new Rect(position);
        buttonRect.yMin += _popupStyle.margin.top;
        buttonRect.width = _popupStyle.fixedWidth + _popupStyle.margin.right;
        position.xMin = buttonRect.xMax;

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var result = EditorGUI.Popup(buttonRect, useConstant.boolValue ? 0 : 1, _popupOptions, _popupStyle);
        useConstant.boolValue = result == 0;

        EditorGUI.PropertyField(position,
                                useConstant.boolValue ? constantValue : data,
                                GUIContent.none);
        if (EditorGUI.EndChangeCheck()){
            property.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
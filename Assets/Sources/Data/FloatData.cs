using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatData", menuName = "Data/FloatData", order = 0)]
public class FloatData : ScriptableObject {
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public float StartingValue;
    public float Value;

    private void OnEnable() {
        Value = StartingValue;
    }

    public void SetValue(float value) {
        Value = value;
    }

    public void SetValue(FloatData value) {
        Value = value.Value;
    }

    public void ApplyChange(float amount) {
        Value += amount;
    }

    public void ApplyChange(FloatData amount) {
        Value += amount.Value;
    }
}
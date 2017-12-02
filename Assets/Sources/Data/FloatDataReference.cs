using System;

[Serializable]
public class FloatDataReference {
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatData Data;

    public FloatDataReference() {}

    public FloatDataReference(float value) {
        UseConstant = true;
        ConstantValue = value;
    }

    public float Value {
        get { return UseConstant ? ConstantValue : Data.Value; }
    }

    public static implicit operator float(FloatDataReference reference) {
        return reference.Value;
    }
}
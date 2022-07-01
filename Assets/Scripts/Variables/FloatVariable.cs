using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Float", menuName = "Variables/Float")]
public class FloatVariable : ScriptableObject
{
    public UnityEvent OnValueChanged;

    float _value;
    public float Value
    {
        get { return _value; }
        set
        {
            if (value == _value)
                return;

            _value = value;
            OnValueChanged?.Invoke();
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Int", menuName = "Variables/Int")]
public class IntVariable : ScriptableObject
{
    public UnityEvent OnValueChanged;

    int _value;
    public int Value
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

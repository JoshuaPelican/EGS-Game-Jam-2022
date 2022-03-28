using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] IntVariable TimerVariable;
    [SerializeField] int MaxTime = 300;
    float floatTimer;

    private void Awake()
    {
        TimerVariable.Value = MaxTime;
    }

    private void FixedUpdate()
    {
        floatTimer += Time.deltaTime;
        TimerVariable.Value = MaxTime - (int)floatTimer;
    }
}

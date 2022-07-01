using UnityEngine;

[CreateAssetMenu(fileName ="New Level Settings", menuName ="Level Settings")]
public class LevelSettings : ScriptableObject
{
    [Header("Tornado Settings")]
    public float StartingScale = 1f;
    public float[] SizeThresholds = new float[6]
    {
        1, 1.5f, 2, 3, 5, 10
    };

    [Space]
    public float SizeGrowthFactor = 0.25f;

    [Header("Gameplay Settings")]
    [Range(0, 1)] public float DestroyedPercentCompletion = 0.95f;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TornadoScaling : MonoBehaviour
{
    [Header("Size Settings")]
    [SerializeField] float SizeGrowthFactor = 0.25f;
    [Header("Variables")]
    [SerializeField] IntVariable ScoreVariable;
    [SerializeField] FloatVariable TornadoSize;
    [SerializeField] IntVariable TornadoThreshold;

    public static UnityEvent<int> OnThresholdReached;

    float baseSize;
    float[] sizeThresholds = new float[6];

    private void OnEnable()
    {
        ScoreVariable.OnValueChanged.AddListener(OnSizeChanged);
    }


    private void OnDisable()
    {
        ScoreVariable.OnValueChanged.RemoveListener(OnSizeChanged);
    }

    private void Start()
    {
        UpdateSize();
    }

    public void SetBaseSize(float size)
    {
        baseSize = size;
    }

    public void SetSizeThresholds(float[] thresholds)
    {
        this.sizeThresholds = thresholds;
    }

    void OnSizeChanged()
    {
        UpdateSize();
        CheckThreshold();
    }


    void UpdateSize()
    {
        //Set size variable
        TornadoSize.Value = (Mathf.Sqrt(ScoreVariable.Value * SizeGrowthFactor) / 1000f) + baseSize;

        //Scale size over time
        //transform.parent.localScale = Vector3.Lerp(transform.localScale, Vector3.one * Size, 0.1f) + (Vector3.one * baseSize);

        //Scale size instantly
        transform.parent.localScale = Vector3.one * TornadoSize.Value;
    }

    void CheckThreshold()
    {
        for (int i = 0; i < sizeThresholds.Length; i++)
        {
            if(TornadoSize.Value <= sizeThresholds[i])
            {
                TornadoThreshold.Value = i;
                return;
            }
        }
    }
}

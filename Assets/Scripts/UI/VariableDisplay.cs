using UnityEngine;
using TMPro;

public class VariableDisplay : MonoBehaviour
{
    [Header("Display Settings")]
    [SerializeField] IntVariable Variable;
    [SerializeField] int LeadingZeros = 8;
    [SerializeField] string Prefix;
    [SerializeField] string Suffix;

    TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        Variable.OnValueChanged.AddListener(SetText);
    }

    private void OnDisable()
    {
        Variable.OnValueChanged.RemoveListener(SetText);
    }

    void SetText()
    {
        textMesh.SetText($"{Prefix}{Variable.Value.ToString($"D{LeadingZeros}")}{Suffix}");
    }
}

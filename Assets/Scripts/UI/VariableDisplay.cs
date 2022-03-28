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
        Variable.OnValueChanged += SetText;
    }

    void SetText()
    {
        textMesh.SetText($"{Prefix}{Variable.Value.ToString($"D{LeadingZeros}")}{Suffix}");
    }
}

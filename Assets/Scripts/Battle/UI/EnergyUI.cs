using UnityEngine;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text energyText;


    void Start()
    {
        if (EnergySystem.Instance != null)
            EnergySystem.Instance.OnEnergyChanged += updateUI;
    }


    void OnDestroy()
    {
        if (EnergySystem.Instance != null)
            EnergySystem.Instance.OnEnergyChanged -= updateUI;
        
    }


    private void updateUI(int current, int max)
    {
        energyText.text = $"{current}/{max}";
    }
}
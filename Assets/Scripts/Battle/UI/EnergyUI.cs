using UnityEngine;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text energyText;
    private EnergySystem energySystem;


    public void init(EnergySystem energySystem)
    {
        this.energySystem = energySystem;
        energySystem.OnEnergyChanged += updateUI;
        updateUI();
    }


    void OnDestroy()
    {
        if (energySystem != null)
            energySystem.OnEnergyChanged -= updateUI;
    }


    private void updateUI()
    {
        energyText.text = $"{energySystem.currentEnergy}/{energySystem.currentMaxEnergy}";
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private Image energyIconImage;

    private EnergySystem energySystem;

    public void init(EnergySystem energySystem)
    {
        this.energySystem = energySystem;
        energySystem.OnEnergyChanged += updateUI;

        Sprite icon = GameManager.Instance?.selectedCharacter?.energyIcon;
        if (energyIconImage != null && icon != null)
            energyIconImage.sprite = icon;

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
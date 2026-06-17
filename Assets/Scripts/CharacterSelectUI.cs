using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private CharacterData[] characters;
    [SerializeField] private Transform[] characterTransforms;
    [SerializeField] private Button[] characterButtons;

    [SerializeField] private float scaleMultiplier = 1.15f;
    [SerializeField] private float raiseAmount = 0.3f;
    [SerializeField] private Color selectedButtonColor = Color.yellow;

    private int selectedIndex = -1;
    private Vector3[] originalPositions;
    private Vector3[] originalScales;
    private Color[] originalButtonColors;

    void Start()
    {
        originalPositions = new Vector3[characterTransforms.Length];
        originalScales = new Vector3[characterTransforms.Length];
        originalButtonColors = new Color[characterButtons.Length];

        for (int i = 0; i < characterTransforms.Length; i++)
        {
            originalPositions[i] = characterTransforms[i].localPosition;
            originalScales[i] = characterTransforms[i].localScale;
        }

        for (int i = 0; i < characterButtons.Length; i++)
            originalButtonColors[i] = characterButtons[i].image.color;

        startButton.interactable = false;
    }

    public void selectCharacter(int index)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        if (selectedIndex >= 0)
        {
            characterTransforms[selectedIndex].localScale = originalScales[selectedIndex];
            characterTransforms[selectedIndex].localPosition = originalPositions[selectedIndex];
            characterButtons[selectedIndex].image.color = originalButtonColors[selectedIndex];
        }

        selectedIndex = index;
        characterTransforms[index].localScale = originalScales[index] * scaleMultiplier;
        characterTransforms[index].localPosition = originalPositions[index] + new Vector3(0, raiseAmount, 0);
        characterButtons[index].image.color = selectedButtonColor;

        startButton.interactable = true;
    }

    public void startGame()
    {
        if (selectedIndex < 0) return;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        GameManager.Instance.startNewRun(characters[selectedIndex]);
    }
}
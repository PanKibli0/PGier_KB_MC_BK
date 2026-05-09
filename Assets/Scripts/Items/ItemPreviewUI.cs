using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPreviewUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    private RectTransform rectTransform;
    public static ItemPreviewUI Instance;

    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        Debug.Log("ItemPreviewUI AWAKE: " + gameObject.scene.name);
        DontDestroyOnLoad(gameObject);
    }

    public void show(ItemData item)
    {
        gameObject.SetActive(true);
        rectTransform.position = Input.mousePosition;
        Debug.Log("PREVIEW SHOW: " + item.itemName);
        icon.sprite = item.icon;
        nameText.text = item.itemName;
        descriptionText.text = item.getDescription();
        Debug.Log("ACTIVE SELF: " + gameObject.activeSelf);
        Debug.Log("ACTIVE IN HIERARCHY: " + gameObject.activeInHierarchy);
        Debug.Log("POS: " + transform.position);
    }

    public void clear()
    {
        icon.sprite = null;
        nameText.text = "";
        descriptionText.text = "";
    }
}
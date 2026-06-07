using UnityEngine;
using UnityEngine.UI;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private Texture2D[] texture;
    [SerializeField] private float thickness = 3f;


    public void drawLine(Vector2 start, Vector2 end, Color color)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.SetParent(transform);

        RectTransform rect = lineObj.AddComponent<RectTransform>();
        Vector2 dir = (end - start).normalized;
        float distance = Vector2.Distance(start, end);

        rect.anchoredPosition = start + dir * (distance / 2);
        rect.sizeDelta = new Vector2(distance, thickness);
        rect.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        if (texture != null && texture.Length > 0)
        {
            RawImage rawImg = lineObj.AddComponent<RawImage>();
            rawImg.texture = texture[Random.Range(0, texture.Length)];
            rawImg.color = color;
        }
        else
        {
            Image img = lineObj.AddComponent<Image>();
            img.color = color;
        }
    }
}
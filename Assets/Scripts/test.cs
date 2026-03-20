using UnityEngine;

public class HierarchyPrinter : MonoBehaviour
{
    void Start()
    {
        PrintHierarchy(transform, 0);
    }

    void PrintHierarchy(Transform t, int level)
    {
        Debug.Log(new string(' ', level * 2) + t.name);
        foreach (Transform child in t)
            PrintHierarchy(child, level + 1);
    }
}
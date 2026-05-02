using UnityEngine;

public class MainBar : MonoBehaviour
{
    private static bool created = false;

    private void Awake()
    {
        if (created)
        {
            Destroy(gameObject);
            return;
        }
        created = true;
        DontDestroyOnLoad(gameObject);
    }
}
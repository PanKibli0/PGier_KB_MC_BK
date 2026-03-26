using UnityEngine;

public class Player : Unit
{
    public static Player Instance;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    new void Start()
    {
        unitType = UnitType.Player;

        base.Start();
    }
}
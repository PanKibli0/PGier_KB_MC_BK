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


    protected override void Start()
    {
        unitType = UnitType.Player;

        base.Start();
    }
}
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNode
{
    public List<BaseNode> connections;
    public bool isUnlocked;
    public bool isVisited;
    public Vector2Int gridPosition;


    public abstract void execute();
    public abstract string getIconPath();

    public virtual void onComplete()
    {
        isVisited = true;
        foreach (var connection in connections)
            connection.isUnlocked = true;
    }
}
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNode
{
    public List<BaseNode> connections = new List<BaseNode>();
    public bool isUnlocked = false;
    public bool isVisited = false;
    public Vector2Int gridPosition;

    public string visitedIconPath;

    public abstract void execute();
    public abstract string getIconPath();

    public virtual void onComplete()
    {
        isVisited = true;
        foreach (var connection in connections)
            connection.isUnlocked = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public string EventDescription;
}

public class CollectingItemGameEvent : GameEvent
{
    public string ItemName;

    public CollectingItemGameEvent(string name)
    {
        ItemName = name;
    }
}

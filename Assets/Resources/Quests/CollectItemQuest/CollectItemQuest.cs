using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemQuest : Quests.QuestGoal
{
    public string Item;

    public override string GetDescription() {
        return $"Collect 10 {Item}";
    }

    public override void Initialize() {
        base.Initialize();
        EventManager.Instance.AddListener<CollectingItemGameEvent>(OnCollecting);
    }

    private void OnCollecting(CollectingItemGameEvent eventInfo) {
        if(eventInfo.ItemName == Item)  {
            CurrentAmount++;
            Evaluate();
        }
    }
}
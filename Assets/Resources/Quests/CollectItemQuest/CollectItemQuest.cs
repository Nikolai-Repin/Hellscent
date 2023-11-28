using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemQuest : Quest.QuestGoal
{
    public string Item;

    public override string GetDescription() {
        return $"Collect 10 {Item}";
    }

    public override void Initialize() {
        base.Initialize();
        EventManager.Instance.AddListener<CollectingItemGameevent>(OnCollecting);
    }

    private void OnCollecting(BuildingGameEvent eventInfo) {
        if(eventInfo.ItemName == )
    }
}
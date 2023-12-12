using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingQuest : Quest
{
    [SerializeField] public UI_Items itemReward;

    void Start() {
        QuestName = "Discovering new Items";
        Description = "Find stuff";
        ItemReward = itemReward;

        Goals.Add(new CollectionGoal(this, itemReward, "Collect 3 stone", false, 0, 3));

        Goals.ForEach(g => g.Init());
    }
}

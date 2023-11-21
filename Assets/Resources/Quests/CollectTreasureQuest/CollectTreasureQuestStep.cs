using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTreasureQuestStep : QuestStep
{
    private int treasureCollected = 0;

    private int treasureToComplete = 5;

    private void OnEnable() {
        GameEventsManager.instance.miscEvents.onTreasureCollected += TreasureCollected;
    }

    private void OnDisable() {
        GameEventsManager.instance.miscEvents.onTreasureCollected -= TreasureCollected;
    }

    private void TreasureCollected() {
        if (treasureCollected < treasureToComplete) {
            treasureCollected++;
        }

        if (treasureCollected <= treasureToComplete) {
            FinishQuestStep();
        }
    }
}

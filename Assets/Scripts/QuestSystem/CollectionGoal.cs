using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal
{
    public UI_Items Item { get; set; }

    [SerializeField] public UI_Items itemToCollect;

    public CollectionGoal(Quest quest, UI_Items item, string description, bool completed, int currentAmount, int requiredAmount) {
        this.Quest = quest;
        this.Item = item;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init() {
        base.Init();
    }

    void ItemPickedUp(UI_Items item) {
        if (itemToCollect = this.Item) {
            this.CurrentAmount++;
            Evaluate();
        }
    }
}

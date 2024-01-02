using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour
{
    Player player;

    public List<Goal> Goals { get; set; } = new List<Goal>();
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ExperienceReward { get; set; }
    public int GoldReward { get; set; }
    public UI_Items ItemReward { get; set; }
    public bool Completed { get; set; }

    public void CheckGoals() {
        Completed = Goals.All(g => g.Completed);
    }

    void GiveReward() {
        if (ItemReward != null) {
            player.inventory.Add("backpack", ItemReward);
        }
    }
}

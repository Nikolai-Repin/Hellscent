using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public static QuestGiver Instance { get; set; }
    public string name;
    public bool AssignedQuest { get; set; }
    public bool Helped { get; set; }

    [SerializeField] private GameObject quests;

    [SerializeField] 
    private string questType;
    private Quest Quest { get; set; }
    
    public void AssignQuest() {
        AssignedQuest = true;
        Quest = (Quest) quests.AddComponent(System.Type.GetType(questType));
    }

    void CheckQuest() {
        if (Quest.Completed) {
            Quest.GiveReward();
            Helped = true;
            AssignedQuest = false;
            NPC.Instance.AddNewDialogue(new string[] {"Thanks for helping! Here's your reward!"}, name);
        }

        else {
            NPC.Instance.AddNewDialogue(new string[] {"You're still not done! Come back when you have finished your task."}, name);
        }
    }
    
}

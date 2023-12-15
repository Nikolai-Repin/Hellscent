using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;

    public Player player;

    public GameObject questWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI goldText;

    private void Update() {
        // Press J to Open up Quest Window
        if(Input.GetKeyDown(KeyCode.J)) {
            OpenQuestWindow();
        }
    }

    public void OpenQuestWindow() {
        if(!questWindow.activeSelf) {
            questWindow.SetActive(true);
            /* titleText.text = quest.title;
            descriptionText.text = quest.description;
            experienceText.text = quest.experienceReward.ToString();
            goldText.text = quest.goldReward.ToString();
            */
        }

        else {
            questWindow.SetActive(false);
        }
    }

    public void AcceptQuest() {
        questWindow.SetActive(false);
        player.quests = quest;
    }
    
}

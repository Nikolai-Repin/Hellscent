using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuestWindow_UI : MonoBehaviour
{
    public List<QuestSlot> questSlots = new List<QuestSlot>();

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
        }

        else {
            questWindow.SetActive(false);
        }
    }
}

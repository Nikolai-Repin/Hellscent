using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSlot : MonoBehaviour
{
    public int slotID;
    public QuestWindow_UI window;

    public Image progress;
    public GameObject checkmark;
    public TextMeshProUGUI title;

    private void Start() {
        SetEmpty();
    }

    public void SetQuestSlot() {

    }

    public void SetEmpty() {
        progress.sprite = null;
        progress.color = new Color(1, 1, 1, 0);
        title.text = "";
    }

    public void SetCheckmark(bool isComplete) {
        checkmark.SetActive(isComplete);
    }
}

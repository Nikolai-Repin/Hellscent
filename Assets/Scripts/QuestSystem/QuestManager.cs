using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questsContent;
    [SerializeField] private GameObject questHolder;

    public List<Quests> CurrentQuests;

    private void Awake() {
        foreach (var quest in CurrentQuests) {
            quest.Initialize();
            GameObject questObj = Instantiate(questPrefab, questsContent);
            questObj.transform.Find("Icon").GetComponent<Image>().sprite = quest.Information.Icon;
        }
    }
}

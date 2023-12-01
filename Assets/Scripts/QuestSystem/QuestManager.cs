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
            quest.QuestCompleted.AddListener(OnQuestCompleted);

            GameObject questObj = Instantiate(questPrefab, questsContent);
            questObj.transform.Find("Icon").GetComponent<Image>().sprite = quest.Information.Icon;

            questObj.GetComponent<Button>().onClick.AddListener(delegate 
            {
                questHolder.GetComponent<QuestWindow>().Initialize(quest);
                questHolder.SetActive(true);
            });
        }
    }

    public void Collect(string itemName) {
        EventManager.Instance.QueueEvent(new CollectingItemGameEvent(itemName));
    }

    private void OnQuestCompleted(Quests quest) {
        questsContent.GetChild(CurrentQuests.IndexOf(quest)).Find("Checkmark").gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public UI_Items[] items;

    private Dictionary<string, UI_Items> nameToItemDict = 
    new Dictionary<string, UI_Items>();

    private void Awake() {
        foreach(UI_Items item in items) {
            AddItem(item);
        }
    }

    private void AddItem(UI_Items item) {
        if(!nameToItemDict.ContainsKey(item.data.itemName)) {
            nameToItemDict.Add(item.data.itemName, item);
        }
    }

    public UI_Items GetItemByName(string key) {
        if(nameToItemDict.ContainsKey(key)) {
            return nameToItemDict[key];
        }

        return null;
    }
}
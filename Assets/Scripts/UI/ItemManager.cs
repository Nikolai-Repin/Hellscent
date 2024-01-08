using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] items;

    private Dictionary<string, Item> nameToItemDict = 
    new Dictionary<string, Item>();

    private void Awake() {
        foreach(Item item in items) {
            AddItem(item);
        }
    }

    private void AddItem(Item item) {
        if(!nameToItemDict.ContainsKey(item.data.itemName)) {
            nameToItemDict.Add(item.data.itemName, item);
        }
    }

    public Item GetItemByName(string key) {
        if(nameToItemDict.ContainsKey(key)) {
            return nameToItemDict[key];
        }

        return null;
    }
}
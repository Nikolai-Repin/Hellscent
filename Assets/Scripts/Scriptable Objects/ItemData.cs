using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Create an ItemData for every Item that is created to set the Name, Icon, and Description
of the item that would be stored in the Inventory */
[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    public string itemName = "Item Name";
    public Sprite icon;
    [TextArea]
    public string description = "Description";
}

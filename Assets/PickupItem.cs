using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private GameObject targetItem;

    void Start () {
        UpdateSprite();
    }

    public bool SetItem (GameObject newItem) {
        targetItem = newItem;
        UpdateSprite();
        return true;
    }

    public bool UpdateSprite () {
        if (targetItem == null) {return false;}
        transform.gameObject.GetComponent<SpriteRenderer>().sprite = targetItem.GetComponent<SpriteRenderer>().sprite;
        return true;
    }

    public GameObject GetItem() {
        return targetItem;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private GameObject targetItem;

    void Start () {
        if (transform.childCount > 0) {
            SetItem(transform.GetChild(0).gameObject);
            Destroy(transform.GetChild(0).gameObject);
        }
        UpdateSprite();
    }

    //I'm sorry. I could have made this better. I have no clue how. Please. Please tell me how. I need to know. There has to be a better way than duplicating the weapon and then using that duped weapon as a reference
    public bool SetItem (GameObject newItem) {
        transform.gameObject.layer = LayerMask.NameToLayer("Items");
        targetItem = Instantiate(newItem.transform.gameObject, newItem.transform.position, new Quaternion());
        targetItem.name = targetItem.name.Replace("(Clone)","").Trim();
        targetItem.transform.SetParent(transform);
        if (targetItem.GetComponent<Collider2D>() != null) {targetItem.GetComponent<Collider2D>().isTrigger = true;}
        targetItem.GetComponent<SpriteRenderer>().enabled = false;
        UpdateSprite();
        return true;
    }

    //Gets the sprite of target item and sets the sprite of the PickupItem to that sprite
    public bool UpdateSprite () {
        if (targetItem == null) {return false;}
        transform.gameObject.GetComponent<SpriteRenderer>().sprite = targetItem.GetComponent<SpriteRenderer>().sprite;
        return true;
    }

    //Returns the target item
    public GameObject GetItem() {
        return targetItem;
    }

    //Destroys the PickupItem and the TargetItem
    public void CleanUp() {
        Destroy(targetItem);
        Destroy(transform.gameObject);
    }
}

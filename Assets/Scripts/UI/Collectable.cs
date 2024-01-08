using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UI_Items))]
public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();

        if(player) {
            UI_Items item = GetComponent<UI_Items>();

            if(item != null) {
                player.inventory.Add("Backpack", item);
                Destroy(this.gameObject);
            }
        }
    }
}
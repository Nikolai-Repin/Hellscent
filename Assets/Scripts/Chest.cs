// To add weapons to the weapon/Item pool, go to the respective chest prefab, then drag whatever you want to add
// in from the inspector and into the list "Weapon/Item/Whatever pool".



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    [SerializeField] private List<GameObject> weaponPool = new();
    [SerializeField] private List<GameObject> itemPool = new();

    private bool opened = false;


    void OnTriggerEnter2D(Collider2D other) {
        if (!opened && other.CompareTag("player")) {
            opened = true;
            int count = 0;
            int index = 0;
            if (weaponPool.Count > 0) {
                while (count < 100) {
                    index = Random.Range(0, weaponPool.Count);
                    count += weaponPool[index].GetComponent<Weapon>().GetWeight();
                }
                GameObject drop = Instantiate(weaponPool[index], transform.position, new Quaternion());
            }
            count = 0;
            if (itemPool.Count > 0) {
                while (count < 100) {
                    index = Random.Range(0, itemPool.Count);
                    count += itemPool[index].GetComponent<Item>().GetWeight();
                }
                GameObject drop = Instantiate(itemPool[index], transform.position, new Quaternion());
            }
        }
    }
}

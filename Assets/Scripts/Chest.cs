// To add weapons to the weapon/Item pool, go to the respective chest prefab, then drag whatever you want to add
// in from the inspector and into the list "Weapon/Item/Whatever pool".



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    [SerializeField] private List<Weapon> weaponPool = new();
    [SerializeField] private List<Weapon> itemPool = new();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

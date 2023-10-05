using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject itemType;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(itemType, new Vector3(-1.8f, -2.5f, 0), Quaternion.identity);
        Debug.Log("test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

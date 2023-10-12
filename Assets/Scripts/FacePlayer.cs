using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public GameObject target2;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Finished Script");
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 playerPos = target2.transform.position;
        Vector3 enemy = gameObject.transform.position;
        Vector3 delta = new Vector3(playerPos.x - enemy.x, 0.0f, playerPos.z - enemy.z);
        Quaternion rotation = Quaternion.LookRotation(delta);
        
      gameObject.transform.rotation = rotation;
        Debug.Log("Finished Script");
    }
}

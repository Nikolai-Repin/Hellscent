using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Test : MonoBehaviour
{
    float moveSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)){        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
}
    }
}

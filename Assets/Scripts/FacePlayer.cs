using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public GameObject target = null;
    public Transform Player;
    Vector3 lastKnownPosition; 
    Quaternion lookAtRotation;
    float timeCount = 1F;
    // Start is called before the first frame update
    void Start()
    {
       
       SetTarget(target);
       
    }

    // Update is called once per frame
    void Update()
    {
     Vector3 direction = Player.position - transform.position;
     Quaternion rotation = Quaternion.LookRotation(direction);
     transform.rotation = Quaternion.Slerp(transform.rotation, rotation,timeCount);         
    timeCount = timeCount + Time.deltaTime; 
    }
    public void SetTarget(GameObject target){
        this.target = target;
    }
}

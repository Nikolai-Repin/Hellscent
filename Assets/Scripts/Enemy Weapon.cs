using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class EnemyWeapon : Weapon



{
    // cooldownTime is set to fire once per second
    public GameObject target;
    protected SpriteRenderer sprn;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;      
        

    }

    // Update is called once per frame
    //Used to have weapon face towards player
    void Update()
    {
        Vector3 temp = new Vector3();
        temp = parent.transform.position;
        temp.x += 1.7F; //Puts weapon on edge of player in Sample Scene; 
        transform.position = temp;
        /*Vector3 Look = transform.InverseTransformPoint(target.transform.position);
        var dir = transform.InverseTransformPoint(target.transform.position);
        float Angle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg;
        transform.Rotate(0,0, Angle);
         var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            sprn.flipY = true;
        } else {
            sprn.flipY = false;
        }
        */
        Fire();
         Debug.Log("testing");
        cooldown -= Time.deltaTime;
        
    }
}


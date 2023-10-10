using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class EnemyWeapon : Weapon



{
    // cooldownTime is set to fire once per second
    protected GameObject target;
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
        temp.x += -.5F;
        transform.position = temp;
        Vector3 Look = transform.InverseTransformPoint(target.transform.position);
        float Angle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg;
        transform.Rotate(0,0, Angle);
        Fire();
        cooldown -= Time.deltaTime;
        Debug.Log(cooldown);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class EnemyWeapon : Weapon



{
    [SerializeField] public int offset = 2;
    [SerializeField] public int ammo = -1;
    [SerializeField] public GameObject projectileType;
    // cooldownTime is set to fire once per second
    [SerializeField] public int cooldownTime = 600;
    public int cooldown;
    private GameObject parent;
    private SpriteRenderer sr;
    private Controller controller;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;      

    }

    // Update is called once per frame
    //Used to have weapon face towards player
    void Update()
    {
      transform.position = parent.transform.position * offset;
      Vector3 Look = transform.InverseTransformPoint(target.transform.position);
      float Angle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg;
      transform.Rotate(0,0, Angle);
       if(cooldown <= 5)
       {
            Fire();
       }
       cooldown--;
    }
}


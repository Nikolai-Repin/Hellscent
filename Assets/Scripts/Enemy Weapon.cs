using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour



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
        sr = GetComponent<SpriteRenderer>();
        controller = parent.GetComponent<Controller>();
        controller.heldWeapons.Add(GetComponent<Weapon>());
        controller.ChangeWeapon(controller.heldWeapons.Count-1);
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
            Attacking();
       }
       cooldown--;
    }

    //Taken from weapon.cs, might make a child of weapon.cs to just call the attack from there;
    public void Attacking()
    {
        GameObject bullet = Instantiate(projectileType, transform.position, new Quaternion());
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(bulletScript.projectileSpeed*Mathf.Cos(transform.rotation.eulerAngles.z*Mathf.Deg2Rad), bulletScript.projectileSpeed*Mathf.Sin(transform.rotation.eulerAngles.z*Mathf.Deg2Rad),0));
        bulletScript.creator = transform.gameObject;
        cooldown = cooldownTime;
    }
}


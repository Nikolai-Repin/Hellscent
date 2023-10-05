using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour



{
     [SerializeField] public int offset = 2;
    [SerializeField] public int ammo = -1;
    [SerializeField] public GameObject projectileType;
    private float cooldown;
    private GameObject parent;
    private SpriteRenderer sr;
    private Controller controller;
    public int NextAttackTimer = 5;
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
    void Update()
    {
      transform.position = parent.transform.position * offset;
      Vector3 Look = transform.InverseTransformPoint(target.transform.position);
      float Angle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg;
      transform.Rotate(0,0, Angle);
       if(NextAttackTimer > 5){
        Attacking();
       NextAttackTimer = -1;
       }
       NextAttackTimer++;
    }
public void Attacking(){

            GameObject bullet = Instantiate(projectileType, transform.position, new Quaternion());
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(bulletScript.projectileSpeed*Mathf.Cos(transform.rotation.eulerAngles.z*Mathf.Deg2Rad), bulletScript.projectileSpeed*Mathf.Sin(transform.rotation.eulerAngles.z*Mathf.Deg2Rad),0));
            bulletScript.creator = transform.gameObject;

}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomb : Entity
{
    private float fuse = 3F;

    [SerializeField] public GameObject projectileType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fuse -= Time.deltaTime;
        if (fuse <= 0) {
            int projectiles = 8;
            float rotationAmount = 359/projectiles;
            float rotationOffset = 0;
            for (int k = 1; k <= 2; k++) {
                for (int i = 0; i < projectiles; i++) {
                    GameObject bullet = Instantiate(projectileType, transform.position, new Quaternion());
                    Bullet bulletScript = bullet.GetComponent<Bullet>();
                    bulletScript.team = "Enemy";
                    Quaternion fireAngle = Quaternion.Euler(new Vector3(0, 0, (rotationAmount*i)+rotationOffset));
                    bulletScript.LaunchProjectile(fireAngle, 10/k);
                }
                rotationOffset += rotationAmount/2;
            }
            Die();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{

    [SerializeField] protected bool invulnerable;
    [SerializeField] protected bool intangible;

    [SerializeField] protected float maxHealthAmount;
    [SerializeField] public float healthAmount;

    protected UIManager uiManager;

    [SerializeField] public float knockbackMult;
    public static List<Entity> entityList = new List<Entity>();
    private RoomInfo room;
    protected SpriteRenderer sr;
    
    protected virtual void Start() {
        sr = GetComponent<SpriteRenderer>();
        Register();
        
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();

    }

    protected virtual void Update() {
        SortInRenderLayer();
    }


    //Deals damage to entity if invulnerable, returns true if damage was dealt
    public virtual bool TakeDamage(float damage) {
        if (intangible) {
            return false;
        }

        if (!invulnerable) {
            healthAmount -= damage;
            uiManager.UpdateEntityHealthBar(gameObject.GetComponent<Entity>());
            if (healthAmount <= 0) {
                Die();
            }
        }
        return true;
   }

       protected void Register() {
        Entity newEntity = transform.GetComponent<Entity>();
        entityList.Add(newEntity);
    }

    protected void FireInRings(GameObject projectile, int projectileCount, float rotationAmount, float rotationOffset, int rings) {
        float breakOutTime = Time.time + 3;
        //Outer for loop controls how many rings of projectiles
        for (int k = 1; k <= rings; k++) {
            //Inner for loop controls how many projectiles in each ring
            for (int i = 0; i < projectileCount; i++) {
                if(Time.time >= breakOutTime) {
                    break;
                }
                GameObject bullet = Instantiate(projectile, transform.position, new Quaternion());
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.team = "Enemy";
                Quaternion fireAngle = Quaternion.Euler(new Vector3(0, 0, (rotationAmount*i)+rotationOffset));
                bulletScript.LaunchProjectile(fireAngle, 10/k);
            }
            rotationOffset += rotationAmount/2;
        }
    }

    protected void CircleShot(GameObject projectile, int projectileCount, float rotationOffset, float projectileSpeed) {
        float rotationAmount = 360/projectileCount;
        for (int i = 0; i < projectileCount; i++) {
            GameObject bullet = Instantiate(projectile, transform.position, new Quaternion());
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.team = gameObject.tag;
            Quaternion fireAngle = Quaternion.Euler(new Vector3(0, 0, (rotationAmount*i)+rotationOffset));
            bulletScript.LaunchProjectile(fireAngle, projectileSpeed);
        }
    }

    protected void ArcShot(GameObject projectile, int projectileCount, float startAngle, float endAngle) {
        float rotationAmount = startAngle-endAngle/projectileCount;
        for (int i = 0; i < projectileCount; i++) {
            GameObject bullet = Instantiate(projectile, transform.position, new Quaternion());
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.team = "Enemy";
            Quaternion fireAngle = Quaternion.Euler(new Vector3(0, 0, (rotationAmount*i)+startAngle));
            bulletScript.LaunchProjectile(fireAngle);
        }
    }

    protected void SlowingLineShot(GameObject projectile, int projectileCount, float startSpeed, float endSpeed, Quaternion angle) {
        float speedChange = (startSpeed - endSpeed) / projectileCount;
        speedChange = ((startSpeed+speedChange) - endSpeed) / projectileCount;
        for (int i = 0; i < projectileCount; i++) {
            GameObject bullet = Instantiate(projectile, transform.position, new Quaternion());
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.team = "Enemy";
            float fireSpeed = startSpeed - speedChange*i;
            //Debug.Log(fireSpeed);
            bulletScript.LaunchProjectile(angle, fireSpeed);
        }
    }

    //Finds the closest game object from a array of collider2D and their distance from Vector3 origin
    public GameObject FindClosest (Collider2D[] targets, Vector3 origin) {
        if (targets.Length > 0) {
            GameObject closest = targets[0].transform.gameObject;
            float closestLen = (targets[0].transform.position - origin).sqrMagnitude;
            float curLen = closestLen;

            for (int i = 1; i < targets.Length; i++) {
                curLen = (targets[i].transform.position - origin).sqrMagnitude;
                if (curLen < closestLen) {
                    closestLen = curLen;
                    closest = targets[i].transform.gameObject;
                }
            }

            return closest;
        }
        return null;
    }

    //Finds the closest game object from a list of collider2D and their distance from Vector3 origin
    public GameObject FindClosest (List<Collider2D> targets, Vector3 origin) {
        return FindClosest(targets.ToArray(), origin);
    }

    //Destroys the entity
    public virtual void Die () {
        entityList.Remove(this);
        if (room != null) {room.RemoveEntity(this);}
        Destroy(transform.gameObject);
    }

    //Returns 0, mainly exists to be overridden in PlayerController so that weapons don't break
    public virtual float GetDamage() {
        return 0;
    }

    //Returns 0, mainly exists to be overridden in PlayerController so that weapons don't break

    public void SetRoom(RoomInfo r) {
        room = r;
    }

    public void SubHealth(float n){
        healthAmount -= n;
    }

    public virtual void Reset() {
        entityList.Remove(this);
        if (room != null) {room.RemoveEntity(this);}
        Destroy(transform.gameObject);
    }

    public static void ResetAll() {
        for (int i = entityList.Count-1; i >= 0; i--) {
            if (entityList[i] != null) {
                entityList[i].Reset();
            } else {
                Debug.Log("Entity is null");
            }
            
        }
    }

    public void SortInRenderLayer() {
        Vector3 tmpPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        sr.sortingOrder = Mathf.RoundToInt(1/tmpPos.y*100);
    }

    public void AddMaxHP(float bonusMaxHP) {
        maxHealthAmount += bonusMaxHP;
    }

    public void RestoreHP(float bonusHP) {
        healthAmount = (healthAmount + bonusHP > maxHealthAmount) ? maxHealthAmount : healthAmount + bonusHP;
    }

    public float GetMaxHealthAmount() {
        return maxHealthAmount;
    }

    public float GetHealthAmount() {
        return healthAmount;
    }

    public virtual void LastEntityEvent() {
        return;
    }

}

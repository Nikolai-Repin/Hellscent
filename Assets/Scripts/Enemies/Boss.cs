using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boss : Enemy
{
    private Vector2 arenaCenter;
    [SerializeField] bool spawnPortal;
    [SerializeField] Vector2 arenaSize;
    [SerializeField] int numBombs;
    [SerializeField] public GameObject projectileType;
    private GameObject bombPrefab;
    private GameObject minionPrefab;
    private Animator animator;
    private CinemachineVirtualCameraBase vCamera;
    private int firedBombs;
    private float lastAttackTime;
    
    public enum Phase
    {
        Death = -1,
        Sleep = 0, //Before player enters boss area, does nothing
        Awakening = 1,
        Wander = 2, //Wanders, while in this phase, will pick a new phase at random
        Sink = 3, //Sinks into the floor, becoming invincible
        Chase = 4, //Quickly chases to the player, switches to Emerge once on top of the player
        Emerge = 5, //Bursts up, spawning sharks, goes to wander
        Bombs = 6 //Fires bombs around the room, after they've been fired, returns to wander
    }
    public float lastPhaseChange;
    public float phaseCooldown = 10F;
    private float phaseCooldownRandom;
    public Phase curPhase;

    // Start is called before the first frame update
    void Start()
    {
        phaseCooldownRandom = phaseCooldown;
        curPhase = Phase.Sleep;
        arenaCenter = transform.position;
        bombPrefab = Resources.Load<GameObject>("Prefabs/Entities/PirateBomb/PirateBomb");
        minionPrefab = Resources.Load<GameObject>("Prefabs/Enemies/PirateMinion/PirateMinion");
        vCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCameraBase>();
        dealDamageOnContact = false;
        invulnerable = true;
        intangible = true;

        animator = GetComponent<Animator>();
        animator.SetInteger("Phase", 0);
        
        base.Start();
        Register();
    }

    // Update is called once per frame
    new void Update()
    {
        switch (curPhase) {
            case Phase.Wander: {
                if (Time.time > lastPhaseChange+phaseCooldownRandom) {
                    PickPhase();
                }
                break;
            }

            case Phase.Sink: {
                Sink();
                lastAttackTime = Time.time + 5;
                curPhase = Phase.Chase;
                break;
            }

            case Phase.Chase: {
                float curLen;
                curLen = (trackerController.target.transform.position - transform.position).sqrMagnitude;
                if (curLen <= 82 || Time.time > lastAttackTime) {
                    curPhase = Phase.Emerge;
                    lastAttackTime = Time.time + 0.75F;
                    trackerController.aiPath.maxSpeed = 2;
                }
                break;
            }

            case Phase.Emerge: {
                if (Time.time >= lastAttackTime) {
                    Rise();
                    ReturnToWander();
                }
                break;
            }

            //Bomb attack, fills arena with bombs that detonate after a short delay, spawning rings of bullets
            case Phase.Bombs: {
                if (Time.time > lastAttackTime) {
                    lastAttackTime = lastAttackTime + 0.2F;
                    GameObject bullet = Instantiate(bombPrefab, new Vector3(Random.Range((arenaCenter.x-arenaSize.x)+3, (arenaCenter.x+arenaSize.x)-3), Random.Range((arenaCenter.y-arenaSize.y)+3, (arenaCenter.y+arenaSize.y)-3), 0), new Quaternion());
                    firedBombs++;
                    
                    //Supposed to mark attack entites as something to destroy when the boss dies, but I need to have it handle entities that have been destroyed
                    bullet.GetComponent<BossBomb>().creator = this;
                    //ClaimEntity(bullet);
                }

                if (firedBombs >= numBombs) {
                    ReturnToWander();
                }
                break;
            }

            //Pre fight, before anything happens, checks if the player is in range to begin fight
            case Phase.Sleep: {
                GameObject closestPlayer = FindClosestPlayer();
                if (closestPlayer != null) {
                    arenaCenter = transform.position;
                    trackerController.SetTarget(closestPlayer.transform);
                    lastAttackTime = Time.time + 2;
                    vCamera.Follow = transform;
                    curPhase = Phase.Awakening;
                }
                break;
            }

            //Intro to fight
            case Phase.Awakening: {
                if (Time.time > lastAttackTime) {
                    Awaken();
                }
                break;
            }

            //Death animation, cleans up the boss's attacks
            case Phase.Death: {
                if (Time.time > lastAttackTime) {
                    vCamera.Follow = trackerController.target.transform;
                    Destroy(transform.gameObject);
                    Vector2 portalOffset = new Vector2(0, arenaSize.y*0.6F);
                    GameObject portal = Resources.Load<GameObject>("Prefabs/Entities/NextAreaPortal/NextAreaPortal"); //This line is bad, lmk if there's a better way to do this, p l e a s e
                    Instantiate(portal, arenaCenter + portalOffset, new Quaternion());
                }
                break;
            }
        }

        SortInRenderLayer();
    }

    //Handling what happens once the fight begins
    public void Awaken() {
        dealDamageOnContact = true;
        invulnerable = false;
        intangible = false;
        trackerController.SetAI(TrackerController.AI.Range);
        vCamera.Follow = trackerController.target.transform;
        ReturnToWander();
    }

    //Picks the next phase
    public void PickPhase() {
        int nextPhase = (int) Random.Range(0, 2);
        Debug.Log(nextPhase);
        switch (nextPhase) {
            case 0: {
                curPhase = Phase.Sink;
                break;
            }
                
            case 1: {
                curPhase = Phase.Bombs;
                firedBombs = 0;
                lastAttackTime = Time.time + 1;
                trackerController.aiPath.maxSpeed = 0;
                animator.SetInteger("Phase", 5);
                break;
            }
        }
    }

    //Returns to wander, ensuring properties are what they're supposed to be that might have been altered during an attack
    public void ReturnToWander() {
        phaseCooldownRandom = Random.Range(phaseCooldown, phaseCooldown * 1.2F);
        curPhase = Phase.Wander;
        trackerController.aiPath.maxSpeed = 5;
        lastPhaseChange = Time.time;
        animator.SetInteger("Phase", 1);
        return;
    }

    private void Sink() {
        dealDamageOnContact = false;
        intangible = true;
        trackerController.aiPath.maxSpeed = 50;
        trackerController.aiPath.maxAcceleration = 45;
        trackerController.SetAI(TrackerController.AI.Melee);
        animator.SetInteger("Phase", 3);
    }

    //Emerges from the sunken state, spawning 3 minions and a ring of projectiles
    private void Rise() {
        dealDamageOnContact = true;
        intangible = false;
        trackerController.aiPath.maxSpeed = 5;
        trackerController.SetAI(TrackerController.AI.Range);
        int numMinions = 3;
        float rotationAmount = 6.283F/numMinions;
        GameObject minion;
        for (int i = 0; i < numMinions; i++) {
            minion = Instantiate(minionPrefab, transform.position + new Vector3(3*Mathf.Cos(rotationAmount*i), 3*Mathf.Sin(rotationAmount*i), 0), new Quaternion());

            //Supposed to mark attack entites as something to destroy when the boss dies, but I need to have it handle entities that have been destroyed
            //ClaimEntity(minion);
        }

        int projectiles = 8; //How many projectiles in each ring
        rotationAmount = 360/projectiles;
        for (int i = 0; i < projectiles; i++) {
            GameObject bullet = Instantiate(projectileType, transform.position, new Quaternion());
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.team = "Enemy";
            Quaternion fireAngle = Quaternion.Euler(new Vector3(0, 0, (rotationAmount*i)));
            bulletScript.LaunchProjectile(fireAngle, 10);

            //Supposed to mark attack entites as something to destroy when the boss dies, but I need to have it handle entities that have been destroyed
            //ClaimEntity(bulletScript);
        }
        animator.SetInteger("Phase", 1);
    }

    public override void Die() {
        dealDamageOnContact = false;
        intangible = true;
        trackerController.aiPath.maxSpeed = 0;
        curPhase = Phase.Death;
        lastAttackTime = Time.time + 1F;
        vCamera.Follow = transform;
        animator.SetInteger("Phase", -1);
    

        //Supposed to destroy when the boss dies, but I need to have it handle entities that have been destroyed
        //foreach (Entity i in ClaimedEntities) {
            //i.Die();
        //}
    }

    public void ClaimEntity(Entity e) {
        //ClaimedEntities.Add(e);
    }

    public void ClaimEntity(GameObject e) {
        //ClaimedEntities.Add(e.GetComponent<Entity>());
    }
}

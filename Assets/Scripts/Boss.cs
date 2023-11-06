using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boss : Enemy
{
    private Vector2 arenaCenter;
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
        bombPrefab = Resources.Load<GameObject>("Prefabs/PirateBomb");
        minionPrefab = Resources.Load<GameObject>("Prefabs/PirateMinion");
        vCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCameraBase>();
        dealDamageOnContact = false;
        invulnerable = true;

        animator = GetComponent<Animator>();
        animator.SetInteger("Phase", 0);
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

            case Phase.Bombs: {
                if (Time.time > lastAttackTime) {
                    lastAttackTime = lastAttackTime + 0.2F;
                    GameObject bullet = Instantiate(bombPrefab, new Vector3(Random.Range((arenaCenter.x-arenaSize.x)+3, (arenaCenter.x+arenaSize.x)-3), Random.Range((arenaCenter.y-arenaSize.y)+3, (arenaCenter.y+arenaSize.y)-3), 0), new Quaternion());
                    firedBombs++;
                }

                if (firedBombs >= numBombs) {
                    ReturnToWander();
                }
                break;
            }

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

            case Phase.Awakening: {
                if (Time.time > lastAttackTime) {
                    Awaken();
                }
                break;
            }

            case Phase.Death: {
                if (Time.time > lastAttackTime) {
                    vCamera.Follow = trackerController.target.transform;
                    Destroy(transform.gameObject);
                }
                break;
            }
        }
    }

    public void Awaken() {
        dealDamageOnContact = true;
        invulnerable = false;
        trackerController.SetAI(TrackerController.AI.Range);
        vCamera.Follow = trackerController.target.transform;
        ReturnToWander();
    }

    //Pick Phase
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

    public void ReturnToWander() {
        phaseCooldownRandom = Random.Range(phaseCooldown, phaseCooldown * 1.2F);
        curPhase = Phase.Wander;
        trackerController.aiPath.maxSpeed = 5;
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

    private void Rise() {
        dealDamageOnContact = true;
        intangible = false;
        trackerController.aiPath.maxSpeed = 5;
        trackerController.SetAI(TrackerController.AI.Range);
        int numMinions = 3;
        float rotationAmount = 6.283F/numMinions;
        for (int i = 0; i < numMinions; i++) {
            Instantiate(minionPrefab, transform.position + new Vector3(3*Mathf.Cos(rotationAmount*i), 3*Mathf.Sin(rotationAmount*i), 0), new Quaternion());
        }

        int projectiles = 8; //How many projectiles in each ring
        rotationAmount = 360/projectiles;
        for (int i = 0; i < projectiles; i++) {
            GameObject bullet = Instantiate(projectileType, transform.position, new Quaternion());
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.team = "Enemy";
            Quaternion fireAngle = Quaternion.Euler(new Vector3(0, 0, (rotationAmount*i)));
            bulletScript.LaunchProjectile(fireAngle, 10);
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
    }
}

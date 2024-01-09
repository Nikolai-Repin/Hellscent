using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WormBoss : Enemy
{
    private Vector2 arenaCenter;
    [SerializeField] bool spawnPortal;
    [SerializeField] bool page;
    [SerializeField] Vector2 arenaSize;
    [SerializeField] public GameObject projectileType;
    [SerializeField] private float difficultyModifier = 1;
    [SerializeField] private int maxTurrets;
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private int segmentCount;
    [SerializeField] private GameObject segmentPrefab;
    private Animator animator;
    private CinemachineVirtualCameraBase vCamera;
    private float lastAttackTime;
    public enum Phase
    {
        Death = -1,
        Sleep = 0, //Before player enters boss area, does nothing
        Awakening = 1,
        Wander = 2, //Wanders, while in this phase, will pick a new phase at random
        Rings = 3, //Each segment fires rings sequentially
        Turrets = 7,
    }
    public float lastPhaseChange;
    public float phaseCooldown = 10F;
    private float phaseCooldownRandom;
    public Phase curPhase;
    private List<GameObject> bodySegments;
    private List<GameObject> turrets;
    private int firingSeg;

    // Start is called before the first frame update
    void Start()
    {
        phaseCooldownRandom = phaseCooldown;
        curPhase = Phase.Sleep;
        arenaCenter = transform.position;
        vCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCameraBase>();
        dealDamageOnContact = false;
        invulnerable = true;
        intangible = true;
        bodySegments = new();
        turrets = new();
        bodySegments.Add(transform.gameObject);
        for (int i = 1; i <= segmentCount; i++) {
            GenerateSegment(i);
        }

        animator = GetComponent<Animator>();
        animator.SetInteger("Phase", 0);
        
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        switch (curPhase) {
            case Phase.Wander: {
                if (Time.time > lastPhaseChange+phaseCooldownRandom) {
                    PickPhase();
                }
                break;
            }

            case Phase.Rings: {
                if (Time.time > lastAttackTime) {
                    if (firingSeg >= bodySegments.Count) {
                        ReturnToWander();
                    } else {
                        if (firingSeg == 0) {
                            CircleShot(projectileType, 16, 180*firingSeg, 10);
                        } else {
                            bodySegments[firingSeg].GetComponent<WormSegment>().FireRing(projectileType, 16, 180*firingSeg, 10);
                        }
                        firingSeg++;
                        Debug.Log(firingSeg);
                        lastAttackTime = Time.time + 1;
                    }
                }
                
                break;
            }

            case Phase.Turrets: {
                if (turrets.Count >= maxTurrets) {
                    ReturnToWander();
                } else if (Time.time > lastAttackTime) {
                    GameObject newTur = Instantiate(turretPrefab, RandomPosInArena(), new Quaternion());
                    turrets.Add(newTur);
                    newTur.GetComponent<WormTurret>().creator = this;
                    lastAttackTime = Time.time + 1;
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
                    Destroy(transform.gameObject, 1);
                    Vector2 portalOffset = new Vector2(0, arenaSize.y*0.6F);
                    Vector2 pageOffset = new Vector2(0, arenaSize.y*0.4F);
                    GameObject portal = Resources.Load<GameObject>("Prefabs/Entities/NextAreaPortal/NextAreaPortal"); //This line is bad, lmk if there's a better way to do this, p l e a s e
                    GameObject page = Resources.Load<GameObject>("Prefabs/Items/page item");
                    Instantiate(portal, arenaCenter + portalOffset , new Quaternion());
                    Instantiate(page, arenaCenter + pageOffset , new Quaternion());
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
        trackerController.SetAI(TrackerController.AI.Melee);
        vCamera.Follow = trackerController.target.transform;
        ReturnToWander();
    }

    //Picks the next phase
    public void PickPhase() {
        int nextPhase = (int) Random.Range(0, 2);
        Debug.Log(nextPhase);
        switch (nextPhase) {
            case 0: {
                SetPhase(Phase.Rings);
                break;
            }
                
            case 1: {
                if(turrets.Count >= maxTurrets) {
                    SetPhase(Phase.Rings);
                }
                SetPhase(Phase.Turrets);
                break;
            }
        }
    }

    public void SetPhase(Phase p) {
        switch (p) {
            case (Phase.Rings): {
                firingSeg = 0;
                curPhase = Phase.Rings;
                break;
            }

            case (Phase.Turrets): {
                curPhase = Phase.Turrets;
                lastAttackTime = Time.time + 1;
                animator.SetInteger("Phase", 1);
                break;
            }
        }
    }

    //Returns to wander, ensuring properties are what they're supposed to be that might have been altered during an attack
    public void ReturnToWander() {
        phaseCooldownRandom = Random.Range(phaseCooldown, phaseCooldown * 1.2F);
        curPhase = Phase.Wander;
        trackerController.aiPath.maxSpeed = 10;
        lastPhaseChange = Time.time;
        animator.SetInteger("Phase", 1);
        return;
    }

    public override void Die() {
        dealDamageOnContact = false;
        intangible = true;
        trackerController.aiPath.maxSpeed = 0;
        curPhase = Phase.Death;
        lastAttackTime = Time.time + 1F;
        vCamera.Follow = transform;
        animator.SetInteger("Phase", -1);
        foreach (GameObject t in turrets) {
            t.GetComponent<Entity>().Die();
        }
    }

    public void GenerateSegment(int i) {
        GameObject newSeg = Instantiate(segmentPrefab, transform.position, new Quaternion());
        bodySegments.Add(newSeg);
        
        newSeg.transform.parent = transform;
        newSeg.transform.localScale = new Vector3(1, 1, 1);
        newSeg.GetComponent<WormSegment>().head = this;
        newSeg.GetComponent<Joint2D>().connectedBody = bodySegments[i-1].GetComponent<Rigidbody2D>();
    }

    private Vector3 RandomPosInArena() {
        return new Vector3(Random.Range((arenaCenter.x-arenaSize.x)+3, (arenaCenter.x+arenaSize.x)-3), Random.Range((arenaCenter.y-arenaSize.y)+3, (arenaCenter.y+arenaSize.y)-3), 0);
    }

    public void RemoveTurret(GameObject t) {
        turrets.Remove(t);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private Vector2 arenaCenter;
    [SerializeField] Vector2 arenaSize;
    [SerializeField] int numBombs;
    private GameObject bombPrefab;
    private GameObject minionPrefab;
    private Animator animator;
    private int firedBombs;
    private float lastAttackTime;
    public enum Phase
    {
        Sleep = 0, //Before player enters boss area, does nothing
        Wander = 1, //Wanders, while in this phase, will pick a new phase at random
        Sink = 2, //Sinks into the floor, becoming invincible
        Chase = 3, //Quickly chases to the player, switches to Emerge once on top of the player
        Emerge = 4, //Bursts up, spawning sharks, goes to wander
        Bombs = 5 //Fires bombs around the room, after they've been fired, returns to wander
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

        animator = GetComponent<Animator>();
        animator.SetInteger("Phase", 0);
    }

    // Update is called once per frame
    new void Update()
    {
        switch (curPhase) {
            case Phase.Wander: {
                //Debug.Log(Time.time - (lastPhaseChange + phaseCooldownRandom));
                if (Time.time > lastPhaseChange+phaseCooldownRandom) {
                    PickPhase();
                    //Debug.Log(curPhase);
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
                Debug.Log(curLen);
                if (curLen <= 82 || Time.time > lastAttackTime) {
                    curPhase = Phase.Emerge;
                    lastAttackTime = Time.time + 0.5F;
                    trackerController.aiPath.maxSpeed = 2;
                }
                break;
            }

            case Phase.Emerge: {
                if (Time.time >= lastAttackTime) {
                    Debug.Log("rising");
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
                    trackerController.SetTarget(closestPlayer.transform);
                    Awaken();
                }
                break;
            }
        }
    }

    private void FixedUpdate() {}

    public void Awaken() {
        dealDamageOnContact = true;
        invulnerable = false;
        trackerController.SetAI(TrackerController.AI.Range);
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
        invulnerable = true;
        trackerController.aiPath.maxSpeed = 50;
        trackerController.aiPath.maxAcceleration = 45;
        trackerController.SetAI(TrackerController.AI.Melee);
        animator.SetInteger("Phase", 3);
        Debug.Log(trackerController.aiPath.endReachedDistance);
    }

    private void Rise() {
        dealDamageOnContact = true;
        invulnerable = false;
        trackerController.aiPath.maxSpeed = 5;
        trackerController.SetAI(TrackerController.AI.Range);
        int numMinions = 3;
        float rotationAmount = 6.283F/numMinions;
        for (int i = 0; i < numMinions; i++) {
            Instantiate(minionPrefab, transform.position + new Vector3(3*Mathf.Cos(rotationAmount*i), 3*Mathf.Sin(rotationAmount*i), 0), new Quaternion());
        }
        animator.SetInteger("Phase", 1);
    }
}
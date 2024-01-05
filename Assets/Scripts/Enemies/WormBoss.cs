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
    private Animator animator;
    private CinemachineVirtualCameraBase vCamera;
    private int currentTurrets;
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
                curPhase = Phase.Rings;
                break;
            }
                
            case 1: {
                curPhase = Phase.Turrets;
                lastAttackTime = Time.time + 1;
                trackerController.aiPath.maxSpeed = 0.1F;
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
    }
}

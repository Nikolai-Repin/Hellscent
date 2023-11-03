using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] GameObject center;
    [SerializeField] float arenaWidth;
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
    public Phase curPhase;

    // Start is called before the first frame update
    void Start()
    {
        curPhase = Phase.Sleep;
    }

    // Update is called once per frame
    new void Update()
    {
        switch (curPhase) {
            case Phase.Wander: {
                if (Time.time < lastPhaseChange+phaseCooldown) {
                    PickPhase();
                    Debug.Log(curPhase);
                }
                break;
            }

            case Phase.Sink: {
                Sink();
                break;
            }

            case Phase.Chase: {
                float curLen = 0;
                curLen = (trackerController.target.transform.position - transform.position).sqrMagnitude;
                if (curLen <= 1) {
                    curPhase = Phase.Emerge;
                }
                break;
            }

            case Phase.Emerge: {
                Rise();
                break;
            }

            case Phase.Bombs: {
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

    public void Awaken() {
        dealDamageOnContact = true;
        invulnerable = false;
        lastPhaseChange = Time.time;
        trackerController.aiPath.maxSpeed = 5;
        curPhase = Phase.Wander;
    }

    //Pick Phase
    public void PickPhase() {
        if(phaseCooldown <= 0) {
            int nextPhase = (int) Random.Range(0, 1);
            switch (nextPhase) {
                case 0: {
                    curPhase = Phase.Sink;
                    break;
                }
                
                case 1: {
                    curPhase = Phase.Bombs;
                    break;
                }
            }
        }
    }

    private Vector2 FindChargeLocation(float targetDist) {
        if (trackerController.target != null) {
            Vector2 bestPos = new Vector2(trackerController.target.transform.position.x + targetDist, trackerController.target.transform.position.y);
            return bestPos;
        }
        return new Vector2(0, 0);
    }

    private void Sink() {
        dealDamageOnContact = false;
        invulnerable = true;
        trackerController.aiPath.maxSpeed = 15;
    }

    private void Rise() {
        dealDamageOnContact = true;
        invulnerable = false;
        trackerController.aiPath.maxSpeed = 5;
    }
}

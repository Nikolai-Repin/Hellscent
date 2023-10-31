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
        LineUp = 2, //Lines up with the player on the x or y axis, facing towards player, whatever's closest, then goes in to charge
        Charge = 3, //Stays still for a period of time, then charges forward at the player, after hitting wall, returns to Wander
        Bombs = 4 //Fires bombs around the room, after they've been fired, returns to wander
    }
    public float phaseCooldown = 10F;
    public Phase curPhase;

    // Start is called before the first frame update
    void Start()
    {
        curPhase = Phase.Sleep;
    }

    // Update is called once per frame
    void Update()
    {
        switch (curPhase) {
            case Phase.Wander: {
                break;
            }

            case Phase.LineUp: {
                break;
            }

            case Phase.Charge: {
                break;
            }

            case Phase.Bombs: {
                break;
            }

            case Phase.Sleep: {
                break;
            }
        }
    }

    public void Awaken() {
        curPhase = Phase.Wander;
    }

    //Pick Phase
    public void PickPhase() {
        if(phaseCooldown <= 0) {
            int nextPhase = (int) Random.Range(0, 1);
            switch (nextPhase) {
                case 0: {
                    curPhase = Phase.Charge;
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
}

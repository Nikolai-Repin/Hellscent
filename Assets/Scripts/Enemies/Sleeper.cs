using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleeper : Enemy
{
    [SerializeField] private float awakeSpeed;
    [SerializeField] private float awakeAcceleration;
    [SerializeField] private float awakeningTime;
    private float curSpeed;
    private float curAcceleration;
    private float accelerationIncrease;
    private float speedIncrease;
    private bool asleep;

    // Start is called before the first frame update
    void Start()
    {
        curSpeed = trackerController.aiPath.maxSpeed;
        curAcceleration = trackerController.aiPath.maxAcceleration;

        accelerationIncrease = (awakeAcceleration-curAcceleration)/awakeningTime;
        speedIncrease = (awakeSpeed-curSpeed)/awakeningTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!asleep && curSpeed != awakeSpeed && curAcceleration != awakeAcceleration) {
            curAcceleration += accelerationIncrease * Time.deltaTime;
            curSpeed += speedIncrease * Time.deltaTime;
            if (curAcceleration > awakeAcceleration) {curAcceleration = awakeAcceleration;}
            if (curSpeed > speedIncrease) {curSpeed = speedIncrease;}

            trackerController.aiPath.maxAcceleration = curAcceleration;
            trackerController.aiPath.maxSpeed = curSpeed;
        }
    }

    public override bool TakeDamage(float damage) {
        asleep = false;
        return base.TakeDamage(damage);
    }

    public override void TriggerEvent(Collider2D other) {
        if (!asleep && other.gameObject.tag == "player") {
            asleep = false;
        }
    }
}

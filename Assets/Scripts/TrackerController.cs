using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TrackerController : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private float offset;
    [SerializeField] private LayerMask walls;
    [SerializeField] private LayerMask enemies;
    [System.Flags]
    public enum AI
    {
        Melee = 1,
        Range = 2,
    }
    [SerializeField] AI ai = AI.Melee;

    [Space, Header("Pathfinder Settings")]
    [SerializeField] private float endReachedDistanceMelee;
    [SerializeField] private float endReachedDistanceRange;

    [SerializeField] public Transform target;
    private AIPath aiPath;

    private void Start()
    {

        transform.parent.GetComponent<Enemy>().trackerController = this;

        aiPath = transform.parent.GetComponent<AIPath>();
        if (ai == AI.Melee)
        {
            aiPath.endReachedDistance = endReachedDistanceMelee;
        }
        else if (ai == AI.Range)
        {
            aiPath.endReachedDistance = endReachedDistanceRange;
        }
    }

    private void Update()
    {
        if (ai == AI.Melee) 
        {
            transform.position = target.transform.position;
        }
        else if (ai == AI.Range) 
        {
            transform.position = target.transform.position;
            var dir = transform.parent.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(-angle + 90, Vector3.forward);
            transform.position += dir.normalized * offset;

            // Tests for walls in between the enemy and the player
            RaycastHit2D hit = Physics2D.Raycast(target.position, dir.normalized, offset, walls.value);
            if (hit.collider != null)
            {
                
                hit = Physics2D.Raycast(target.position, dir.normalized, offset, enemies.value | walls.value);
                if (hit.transform.tag == "Enemy")
                {
                    Debug.DrawLine(target.position, transform.position, Color.blue);
                }
                else
                {
                    Debug.DrawLine(target.position, transform.position, Color.red);
                    transform.position = target.transform.position;
                }
                
            }
            else
            {
                Debug.DrawLine(target.position, transform.position, Color.green);
            }
        }
    }

    public void SetTarget(Transform newTarget) {
        target = newTarget;
    }
}

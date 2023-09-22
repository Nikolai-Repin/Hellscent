using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offset;

    [System.Flags]
    public enum AI 
    {
        Melee = 1,
        Range = 2,
    }
    [SerializeField] AI ai = AI.Melee;

    // Update is called once per frame
    void Update()
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
        }
    }
}

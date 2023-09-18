using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPointer : MonoBehaviour
{
    void Update()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle + 90, Vector3.forward);
    }
}

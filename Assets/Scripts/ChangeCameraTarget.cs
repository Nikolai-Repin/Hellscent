using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCameraTarget : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup targetGroup = null;

    // Start is called before the first frame update
    void Start()
    {
        targetGroup = gameObject.GetComponent<CinemachineTargetGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCameraTargets(GameObject enemy) {
        if (enemy.GetComponent<Enemy>().HasHealthBar()){
            targetGroup.AddMember(enemy.transform, 2f, 0.5f);
        }
    }

    public void RemoveCameraTargets(GameObject enemy) {
        Debug.Log("Removed Target " + enemy);
        targetGroup.RemoveMember(enemy.transform);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private List<GameObject> enemyPool;
    [SerializeField] private int enemyCount;
    [SerializeField] private bool point = false;
    [SerializeField] private GameObject target;
    [SerializeField] private float spawnTime;
    [SerializeField] private bool useTargets;
    [SerializeField] private float activationChance = 1F;
    
    public IEnumerator SpawnEnemies() {
        if (Random.Range(0, 1) <= activationChance) {
            for (int i = 0; i < enemyCount; i++) {
                GameObject enemy;
                if (point) {
                    if (useTargets) {
                        GameObject tar = Instantiate(target, transform.position, Quaternion.Euler(0, 0, 0));
                        StartCoroutine(target.GetComponent<TargetSpawn>().Spawn(enemyPool[Random.Range(0, enemyPool.Count)], spawnTime, transform.parent.GetComponent<RoomInfo>(), tar));
                        transform.parent.gameObject.GetComponent<RoomInfo>().entities.Add(tar);
                    } else {
                        enemy = Instantiate(enemyPool[Random.Range(0, enemyPool.Count)], transform.position, Quaternion.Euler(0, 0, 0));
                        enemy.GetComponent<Entity>().SetRoom(transform.parent.GetComponent<RoomInfo>()); 
                        transform.parent.gameObject.GetComponent<RoomInfo>().entities.Add(enemy);
                    }
                } else {
                    Vector2 pos = Random.insideUnitCircle * radius;
                    if (useTargets) {
                        GameObject tar = Instantiate(target, transform.position, Quaternion.Euler(0, 0, 0));
                        tar.transform.Translate(pos);
                        StartCoroutine(target.GetComponent<TargetSpawn>().Spawn(enemyPool[Random.Range(0, enemyPool.Count)], spawnTime, transform.parent.GetComponent<RoomInfo>(), tar));
                        transform.parent.gameObject.GetComponent<RoomInfo>().entities.Add(tar);
                    } else {
                        enemy = Instantiate(enemyPool[Random.Range(0, enemyPool.Count)], transform.position, Quaternion.Euler(0, 0, 0));
                        enemy.GetComponent<Entity>().SetRoom(transform.parent.GetComponent<RoomInfo>());
                        enemy.transform.Translate(pos);
                        transform.parent.gameObject.GetComponent<RoomInfo>().entities.Add(enemy);
                    }
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

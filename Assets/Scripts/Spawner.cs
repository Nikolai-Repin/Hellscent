using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private List<GameObject> enemyPool;
    [SerializeField] private int enemyCount;
    [SerializeField] private bool point = false;
    
    public IEnumerator SpawnEnemies() {
        for (int i = 0; i < enemyCount; i++) {
            GameObject enemy;
            if (point) {
                enemy = Instantiate(enemyPool[Random.Range(0, enemyPool.Count)], transform.position, Quaternion.Euler(0, 0, 0));
                enemy.GetComponent<Entity>().SetRoom(transform.parent.GetComponent<RoomInfo>());
            } else {
                enemy = Instantiate(enemyPool[Random.Range(0, enemyPool.Count)], transform.position, Quaternion.Euler(0, 0, 0));
                enemy.GetComponent<Entity>().SetRoom(transform.parent.GetComponent<RoomInfo>());
                enemy.transform.Translate(Random.insideUnitCircle * radius);
            }
            transform.parent.gameObject.GetComponent<RoomInfo>().entities.Add(enemy);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

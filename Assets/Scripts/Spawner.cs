using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private List<GameObject> enemyPool;
    [SerializeField] private int enemyCount;

    public void SpawnEnemies() {
        for (int i = 0; i < enemyCount; i++) {
            float dist = Random.value * radius;
            Vector2 position = new Vector2(transform.position.x + (Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) * dist), transform.position.y + (Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) * dist));
            GameObject enemy = Instantiate(enemyPool[Random.Range(0, enemyPool.Count - 1)], position, Quaternion.Euler(0, 0, 0));
            enemy.transform.SetParent(transform.parent, false);
            transform.parent.gameObject.GetComponent<RoomInfo>().entities.Add(enemy);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

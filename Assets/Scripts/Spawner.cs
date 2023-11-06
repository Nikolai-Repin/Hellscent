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
                enemy = Instantiate(enemyPool[Random.Range(0, enemyPool.Count - 1)], transform.position, Quaternion.Euler(0, 0, 0));
            } else {
                enemy = Instantiate(enemyPool[Random.Range(0, enemyPool.Count - 1)], new Vector2(0, 0), Quaternion.Euler(0, 0, 0));
                float dist = Random.value * radius;
                Vector2 position = new(transform.position.x + (Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) * dist), transform.position.y + (Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) * dist));
                enemy.transform.Translate(position);
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

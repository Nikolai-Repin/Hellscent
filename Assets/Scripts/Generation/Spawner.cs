using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] public List<GameObject> enemyPool;
    [SerializeField] private int enemyCount;
    [SerializeField] private bool point = false;
    [SerializeField] private GameObject target;
    [SerializeField] private float spawnTime;
    [SerializeField] private bool useTargets;
    [SerializeField] private float activationChance = 1F;
    
    public IEnumerator SpawnEnemies() 
    {
        // Spawns enemies
        if (Random.Range(0, 1) <= activationChance)
        {
            for (int i = 0; i < enemyCount; i++) 
            {
                GameObject enemy;
                if (point) 
                {
                    // If the enemy is garuanteed to spawn in one spot
                    if (useTargets) 
                    {
                        GameObject tar = Instantiate(target, transform.position, Quaternion.Euler(0, 0, 0));
                        StartCoroutine(target.GetComponent<TargetSpawn>().Spawn(enemyPool[SelectEnemy()], spawnTime, transform.parent.GetComponent<RoomInfo>(), tar));
                        transform.parent.gameObject.GetComponent<RoomInfo>().entities.Add(tar);
                    } else 
                    {
                        // Spawns enemies in a certain radius
                        enemy = Instantiate(enemyPool[Random.Range(0, enemyPool.Count)], transform.position, Quaternion.Euler(0, 0, 0));
                        enemy.GetComponent<Entity>().SetRoom(transform.parent.GetComponent<RoomInfo>()); 
                        transform.parent.gameObject.GetComponent<RoomInfo>().entities.Add(enemy);
                    }
                } 
                else 
                {
                    Vector2 pos = Random.insideUnitCircle * radius;
                    if (useTargets) 
                    {
                        // Uses tagets for enemies
                        GameObject tar = Instantiate(target, transform.position, Quaternion.Euler(0, 0, 0));
                        tar.transform.Translate(pos);
                        StartCoroutine(target.GetComponent<TargetSpawn>().Spawn(enemyPool[SelectEnemy()], spawnTime, transform.parent.GetComponent<RoomInfo>(), tar));
                        transform.parent.gameObject.GetComponent<RoomInfo>().entities.Add(tar);
                    } else 
                    {
                        // Doesn't use targets in case of boss or chest
                        enemy = Instantiate(enemyPool[Random.Range(0, enemyPool.Count)], transform.position, Quaternion.Euler(0, 0, 0));
                        enemy.GetComponent<Entity>().SetRoom(transform.parent.GetComponent<RoomInfo>());
                        enemy.transform.Translate(pos);
                        transform.parent.gameObject.GetComponent<RoomInfo>().entities.Add(enemy);
                    }
                }
                // Delay between enemies
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private int SelectEnemy() 
    {
        // Randomly chooses enemies based on weight
        int addedWeight = transform.parent.parent.GetComponent<GenerateDungeon>().GetWeight();
        int count = 0;
        int index = 0;
        int i = 0;
        while (i < 200 && count < 100) 
        {
            index = Random.Range(0, enemyPool.Count);
            int weight = enemyPool[index].GetComponent<Enemy>().GetWeight();
            if (weight < 60) 
            {
                weight += addedWeight;
            }
            count += weight;
            i++;
        }
        return index;
    }

    private void OnDrawGizmosSelected() 
    {
        // Gives visualization of radius for debug in scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

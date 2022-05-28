using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy prefab;
    [SerializeField] float minTimeBetweenSpawns = 0.1f;
    [SerializeField] float maxTimeBetweenSpawns = 0.5f;
    [SerializeField] Transform[] spawnPoints;
    private ObjectPool<Enemy> pool;
    // Start is called before the first frame update
    void Start()
    {
        pool = new ObjectPool<Enemy>(Create, GetFromPool, PutBackInPool,DestroyPoolItem, false, 20,100);
        StartCoroutine(SpawnEnemies());

    }

    public IEnumerator SpawnEnemies()
    {
        while(true)
        {
            Spawn();
            yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
        }
    }

    private void Spawn()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        Enemy enemy = pool.Get();
        enemy.Spawn(KillEnemy, spawnPoint);
    }

    private void KillEnemy(Enemy enemyToKill)
    {
        pool.Release(enemyToKill);
    }

    Enemy Create()
    {
        return Instantiate(prefab);
    }
    
    void GetFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    void PutBackInPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    void DestroyPoolItem(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}

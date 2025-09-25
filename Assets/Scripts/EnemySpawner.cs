using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRadius = 20f;
    public int enemiesPerWave = 100;
    public float spawnInterval = 0.01f;
    public int maxActiveEnemies = 200;

    [Header("Pooling Settings")]
    public int poolSize = 300;  // Preload enemies
    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private int enemiesSpawnedThisWave = 0;
    public List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        
    }

    void Awake()
    {
        // Pre-instantiate enemy pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            EnemyT enemyScript = obj.GetComponent<EnemyT>();
            if (enemyScript != null)
            {
                enemyScript.spawner = this; // assign spawner reference
            }
            enemyPool.Enqueue(obj);
        }
    }

    public void StartGame()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        enemiesSpawnedThisWave = 0;
        while (enemiesSpawnedThisWave < enemiesPerWave)
        {
            if (activeEnemies.Count < maxActiveEnemies)
            {
                SpawnEnemy();
                enemiesSpawnedThisWave++;
            }
            yield return new WaitForSeconds(spawnInterval);

            // cleanup nulls (in case of manual destroys)
            activeEnemies.RemoveAll(x => x == null);
        }

        // Wait until all enemies are cleared before next wave
        while (activeEnemies.Count > 0) yield return null;

        // Repeat
        StartCoroutine(SpawnWaveRoutine());
        Debug.Log("New wave started");
    }

    void SpawnEnemy()
    {
        Vector2 pos = (Vector2)player.position + Random.insideUnitCircle.normalized * Random.Range(8f, spawnRadius);

        GameObject enemyObj = GetPooledEnemy();
        if (enemyObj != null)
        {
            enemyObj.transform.position = pos;
            enemyObj.transform.rotation = Quaternion.identity;
            enemyObj.SetActive(true);

            EnemyT enemyScript = enemyObj.GetComponent<EnemyT>();
            if (enemyScript != null) enemyScript.ResetEnemy();

            activeEnemies.Add(enemyObj);
        }
    }

    GameObject GetPooledEnemy()
    {
        if (enemyPool.Count > 0)
        {
            return enemyPool.Dequeue();
        }
        else
        {
            // Fallback (shouldn’t happen if poolSize is big enough)
            GameObject obj = Instantiate(enemyPrefab);
            EnemyT enemyScript = obj.GetComponent<EnemyT>();
            if (enemyScript != null) enemyScript.spawner = this;
            return obj;
        }
    }

    public void RemoveEnemy(GameObject e)
    {
        if (activeEnemies.Contains(e)) activeEnemies.Remove(e);

        // Instead of destroying, recycle
        e.SetActive(false);
        enemyPool.Enqueue(e);
    }

    public void ClearAllEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(false);
                enemyPool.Enqueue(enemy);
            }
        }
        activeEnemies.Clear();
    }
}

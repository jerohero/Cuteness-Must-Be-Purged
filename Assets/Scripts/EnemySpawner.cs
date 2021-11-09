using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] float minSpawnDelay = 10f; // Min delay without calculations
    [SerializeField] float strictMinSpawnDelay = 5f; // Min delay with calculations
    [SerializeField] float maxSpawnDelay = 25f;
    [SerializeField] float spawnDelayMultiplier = .4f;
    [SerializeField] Enemy[] enemyPrefabs;
    [SerializeField] int minSpawningRange = 10;
    [SerializeField] int maxEnemiesAllowed = 500;

    bool isAlive = true;
    Transform playerTransform;
    int spawnCount = 0;

    void Start() {
        playerTransform = FindObjectOfType<Player>().transform;

        StartCoroutine(WaitAndSpawn());
    }

    IEnumerator WaitAndSpawn() {
        while (isAlive) {
            float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            spawnDelay -= spawnCount * spawnDelayMultiplier;

            if (spawnDelay <= strictMinSpawnDelay) {
                spawnDelay = strictMinSpawnDelay;
            }

            yield return new WaitForSeconds(spawnDelay);

            if (ShouldSpawn()) {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy() {
        if (FindObjectsOfType<Enemy>().Length >= maxEnemiesAllowed) {
            return;
        }

        Enemy randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Instantiate(randomEnemy, transform.position, transform.rotation);

        spawnCount++;
    }

    bool ShouldSpawn() {
        float distanceFromPlayer = Vector2.Distance(playerTransform.position, transform.position);

        return (distanceFromPlayer >= minSpawningRange) && GameSession.instance.combatIsInitiated;
    }
}

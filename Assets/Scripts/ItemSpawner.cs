using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    [SerializeField] int killsForHealthSpawn = 50;
    [SerializeField] Transform[] spawnLocations;
    [SerializeField] GameObject healthItem;

    bool canSpawn = true;
    int killsAtLastHealthSpawn;

    void Update() {
        if (!canSpawn || !GameSession.instance.gameIsActive) {
            return;
        }

        HandleSpawnHealthItem();
    }

    void HandleSpawnHealthItem() {
        int currentKills = GameSession.instance.GetKills();

        if (!CanSpawnHealthItem(currentKills)) {
            return;
        }

        killsAtLastHealthSpawn = currentKills;

        SpawnHealthItem();
    }

    void SpawnHealthItem() {
        Transform spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];

        Instantiate(healthItem, spawnLocation.position, spawnLocation.rotation);
    }

    bool CanSpawnHealthItem(int currentKills) {
        bool isEnoughKillsForSpawn = currentKills % killsForHealthSpawn == 0;
        bool didNotSpawnYet = currentKills != killsAtLastHealthSpawn;

        return isEnoughKillsForSpawn && didNotSpawnYet;
    }
}

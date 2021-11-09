using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameSession : MonoBehaviour {
    [SerializeField] int maxHealth = 3;
    [SerializeField] AudioClip combatMusic;
    [SerializeField] AudioClip gameOverMusic;

    public static GameSession instance { get; private set; }
    public bool gameIsActive { get; private set; }
    public bool combatIsInitiated { get; private set; }

    int kills = 0;
    int health = 0;
    Stopwatch timer;

    void Awake() {
        instance = this;
        gameIsActive = true;

        health = maxHealth;

        timer = new Stopwatch();
    }

    public void InitiateCombat() {
        combatIsInitiated = true;

        timer.Start();

        PlayMusic(combatMusic, true);

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++) {
            enemies[i].FollowPlayer();
        }
    }

    public void GameOver() {
        gameIsActive = false;

        timer.Stop();

        PlayMusic(gameOverMusic, false);
    }

    public void AddKill() {
        kills += 1;

        UI.instance.UpdateKillText();
    }

    public void RemoveHealth(int healthToRemove) {
        if (health <= 0) {
            return;
        }

        health -= healthToRemove;

        UI.instance.UpdateHealthUI(health);
    }

    public void AddHealth(int healthToAdd) {
        if (health >= maxHealth) {
            return;
        }

        health += healthToAdd;

        UI.instance.UpdateHealthUI(health);
    }

    public int GetKills() {
        return kills;
    }

    public int GetHealth() {
        return health;
    }

    public double GetElapsedTime() {
        return timer.Elapsed.TotalSeconds;
    }

    void PlayMusic(AudioClip music, bool loop) {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = music;
        audio.loop = loop;
        audio.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour {
    [SerializeField] Transform bodyTransform;
    [SerializeField] Animator bodyAnimator;
    [SerializeField] int maxHealth = 20;

    AIPath aiPath;
    Transform playerTransform;

    int currentHealth;

    void Start() {
        aiPath = GetComponent<AIPath>();
        playerTransform = FindObjectOfType<Player>().transform;

        AIDestinationSetter aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiDestinationSetter.target = playerTransform;

        currentHealth = maxHealth;

        if (GameSession.instance.combatIsInitiated) {
            FollowPlayer();
        }
    }

    void Update() {
        if (!GameSession.instance.combatIsInitiated) {
            return;
        }

        if (currentHealth <= 0) {
            Die();
            return;
        }

        HandleMovementDirection();
    }

    public void HandleDamage(int damage) {
        if (!GameSession.instance.combatIsInitiated) {
            GameSession.instance.InitiateCombat();
        }

        currentHealth -= damage;
        bodyAnimator.SetTrigger("hit");

        SpawnBlood(60f, 16);
    }

    void HandleMovementDirection() {
        if (aiPath.desiredVelocity.x >= 0.01f) {
            bodyTransform.localScale = new Vector3(1f, 1f, 1f);
        } else {
            bodyTransform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    public void FollowPlayer() {
        aiPath.canSearch = true;
        bodyAnimator.SetBool("walking", true);
    }

    void Die() {
        GameSession.instance.AddKill();

        SpawnBlood(300f, 32);

        Destroy(gameObject);
    }

    void SpawnBlood(float bloodSpread, int bloodCount) {
        Vector3 bloodDir = (transform.position - playerTransform.position).normalized;
        BloodParticleSystemHandler.Instance.SpawnBlood(
            transform.position, bloodDir, bloodSpread, bloodCount
        );
    }
}

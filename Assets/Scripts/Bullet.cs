using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] GameObject hitEffect;
    [SerializeField] int bulletDamage = 10;
    [SerializeField] float secondsBeforeDespawn = .5f;

    BoxCollider2D bulletCollider;

    void Start() {
        bulletCollider = GetComponent<BoxCollider2D>();

        StartCoroutine(WaitAndDespawn());
    }

    IEnumerator WaitAndDespawn() {
        yield return new WaitForSeconds(secondsBeforeDespawn);

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        CreateHitEffect();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!IsTouchingEnemy()) {
            return;
        }

        DamageEnemy(collision);
        CreateHitEffect();
    }

    void CreateHitEffect() {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);
    }

    void DamageEnemy(Collider2D collision) {
        Enemy hitEnemy = collision.transform.parent.gameObject.GetComponent<Enemy>();

        hitEnemy.HandleDamage(bulletDamage);
    }

    bool IsTouchingEnemy() {
        return bulletCollider.IsTouchingLayers(LayerMask.GetMask("EnemyHitbox"));
    }
}

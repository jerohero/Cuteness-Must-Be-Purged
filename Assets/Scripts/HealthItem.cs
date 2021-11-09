using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour {
    Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!IsTouchingPlayer()) {
            return;
        }

        FindObjectOfType<Player>().GiveHealth(1);

        Destroy(gameObject);
    }

    bool IsTouchingPlayer() {
        return rb.IsTouchingLayers(LayerMask.GetMask("Player"));
    }
}

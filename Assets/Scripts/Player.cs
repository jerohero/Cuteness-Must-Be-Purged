using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] float runSpeed = 5f;
    [SerializeField] Animator bodyAnimator;
    [SerializeField] float hitRecoveryTime = .5f;

    Vector2 movement;
    bool canGetHit = true;

    Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if (!GameSession.instance.gameIsActive) {
            rb.bodyType = RigidbodyType2D.Static;
            return;
        }

        PlayerMovement();

        rb.MovePosition(rb.position + movement * runSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!IsTouchingEnemy() || !canGetHit || !GameSession.instance.gameIsActive) {
            return;
        }

        HandleHit(1);
    }

    void PlayerMovement() {
        Run();
    }

    void Run() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bodyAnimator.SetBool("running", HasVelocity());
    }

    void HandleHit(int damage) {
        GameSession.instance.RemoveHealth(damage);

        StartCoroutine(HitRecovery());

        if (GameSession.instance.GetHealth() <= 0) {
            Die();
        }
    }

    void Die() {
        bodyAnimator.SetBool("isDead", true);

        UI.instance.ShowGameOverScreen();
        GameSession.instance.GameOver();
    }

    public void GiveHealth(int healthToAdd) {
        GameSession.instance.AddHealth(healthToAdd);
    }

    IEnumerator HitRecovery() {
        canGetHit = false;

        yield return new WaitForSeconds(hitRecoveryTime);

        canGetHit = true;
    }

    bool HasVelocity() {
        return Mathf.Abs(movement.x) > Mathf.Epsilon || Mathf.Abs(movement.y) > Mathf.Epsilon;
    }

    bool IsTouchingEnemy() {
        return rb.IsTouchingLayers(LayerMask.GetMask("Enemies"));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Animator aimAnimator;
    [SerializeField] float bulletForce = 10f;
    [SerializeField] float shootCooldownTime = .3f;
    [SerializeField] AudioSource gunAudioSource;

    bool canShoot = true;

    void Update() {
        if (!GameSession.instance.gameIsActive) {
            return;
        }

        HandleShooting();
    }

    void HandleShooting() {
        if (Input.GetButtonDown("Fire1")) {
            if (canShoot) {
                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot() {
        canShoot = false;

        aimAnimator.SetTrigger("shoot");
        gunAudioSource.Play();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(shootCooldownTime);

        canShoot = true;
    }
}

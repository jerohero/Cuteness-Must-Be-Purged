using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour {
    [SerializeField] Camera cam;
    [SerializeField] Transform aimTransform;
    [SerializeField] Transform bodyTransform;
    [SerializeField] Transform firePointTransform;

    Vector3 mousePos;

    void Update() {
        if(!GameSession.instance.gameIsActive) {
            HideAim();
            return;
        }

        HandleAiming();
    }

    void HandleAiming() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDir = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
        firePointTransform.eulerAngles = new Vector3(0, 0, angle - 90);

        HandleAimingDirection(angle);
    }

    void HandleAimingDirection(float aimAngle) {
        Vector3 aimLocalScale = Vector3.one;
        if (aimAngle > 90 || aimAngle < -90) {
            aimLocalScale.y = -1f;
            FlipSprite(false);
        } else {
            aimLocalScale.y = 1f;
            FlipSprite(true);
        }
        aimTransform.localScale = aimLocalScale;
    }

    void FlipSprite(bool shouldFaceRight) {
        Vector2 scale = bodyTransform.localScale;
        scale.x = shouldFaceRight ? 1 : -1;
        bodyTransform.localScale = scale;
    }

    void HideAim() {
        aimTransform.localScale = new Vector3(0, 0, 0);
    }
}

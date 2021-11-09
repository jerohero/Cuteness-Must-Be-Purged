using System.Collections;
using UnityEngine;

public class EnemyWander : MonoBehaviour {
    [SerializeField] float speed = .2f;
    [SerializeField] Transform[] wanderWayPoints;
    [SerializeField] Animator enemyAnimator;
    [SerializeField] Transform bodyTransform;

    Transform enemy;

    int currentWayPointIndex;
    bool isWaiting;

    void Start() {
        enemy = GetComponent<Enemy>().transform;
    }

    void Update() {
        if (isWaiting || GameSession.instance.combatIsInitiated) {
            return;
        }

        int nextWayPointIndex = GetNextWayPointIndex();
        Vector2 nextWayPointPos = wanderWayPoints[nextWayPointIndex].position;

        enemy.position = Vector2.MoveTowards(enemy.position, nextWayPointPos, speed * Time.deltaTime);
        HandleMovementDirection(nextWayPointPos);

        if (Vector2.Distance(enemy.position, nextWayPointPos) <= 0) {
            StartCoroutine(WaitBeforeContinueWander());
            currentWayPointIndex = nextWayPointIndex;

            return;
        }

        enemyAnimator.SetBool("walking", true);
    }

    IEnumerator WaitBeforeContinueWander() {
        isWaiting = true;
        enemyAnimator.SetBool("walking", false);

        yield return new WaitForSeconds(Random.Range(2, 4));

        isWaiting = false;
    }

    void HandleMovementDirection(Vector2 nextWayPointPos) {
        if (nextWayPointPos.x > enemy.position.x) {
            bodyTransform.localScale = new Vector3(1f, 1f, 1f);
        } else if (nextWayPointPos.x < enemy.position.x) {
            bodyTransform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    int GetNextWayPointIndex() {
        if (currentWayPointIndex + 1 >= wanderWayPoints.Length) {
            return 0;
        }

        return currentWayPointIndex + 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @Code Monkey - Persistent Particle System (Blood, Shells, Dirt, Footprints)
// https://youtu.be/SkkxwsXwEhc

public class BloodParticleSystemHandler : MonoBehaviour {

    public static BloodParticleSystemHandler Instance { get; private set; }

    private MeshParticleSystem meshParticleSystem;
    private List<Single> singleList;

    private void Awake() {
        Instance = this;
        meshParticleSystem = GetComponent<MeshParticleSystem>();
        singleList = new List<Single>();
    }

    private void Update() {
        for (int i=0; i<singleList.Count; i++) {
            Single single = singleList[i];
            single.Update();
            if (single.IsParticleComplete()) {
                singleList.RemoveAt(i);
                i--;
            }
        }
    }

    public void SpawnBlood(Vector3 position, Vector3 direction, float spread, int amount) {
        float bloodParticleCount = amount;
        for (int i = 0; i < bloodParticleCount; i++) {
            singleList.Add(new Single(position, ApplyRotationToVector(direction, Random.Range(-spread, spread)), meshParticleSystem));
        }
    }

    public static Vector3 ApplyRotationToVector(Vector3 vec, float angle) {
        return Quaternion.Euler(0, 0, angle) * vec;
    }

    /*
     * Represents a single Dirt Particle
     * */
    private class Single {

        private MeshParticleSystem meshParticleSystem;
        private Vector3 position;
        private Vector3 direction;
        private int quadIndex;
        private Vector3 quadSize;
        private float moveSpeed;
        private float rotation;
        private int uvIndex;

        public Single(Vector3 position, Vector3 direction, MeshParticleSystem meshParticleSystem) {
            this.position = position;
            this.direction = direction;
            this.meshParticleSystem = meshParticleSystem;

            quadSize = new Vector3(.3f, .3f);
            rotation = Random.Range(0, 360f);
            moveSpeed = Random.Range(.4f, 4f);
            uvIndex = Random.Range(0, 8);

            quadIndex = meshParticleSystem.AddQuad(position, rotation, quadSize, false, uvIndex);
        }

        public void Update() {
            position += direction * moveSpeed * Time.deltaTime;
            rotation += 360f * (moveSpeed / 10f) * Time.deltaTime;

            meshParticleSystem.UpdateQuad(quadIndex, position, rotation, quadSize, false, uvIndex);

            float slowDownFactor = 3.5f;
            moveSpeed -= moveSpeed * slowDownFactor * Time.deltaTime;
        }

        public bool IsParticleComplete() {
            return moveSpeed < .1f;
        }

    }

}

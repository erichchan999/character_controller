using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Combat {
    
    /**
     * This class adds Melee behaviour to a GameObject
     */
    public class MeleeAttack : MonoBehaviour {

        [SerializeField] private bool debug;
        [SerializeField] float meleeRadius;


        private bool meleePressed;
        private LayerMask enemyLayer;
        
        protected void Start() {
            enemyLayer = LayerMask.GetMask("Enemy");
            // Debug.Log(String.Format("Enemy Layer: {0}\n", enemyLayer.value));
        }

        protected void Update() {


        }

        protected void OnDrawGizmos() {
            if (debug) {
                if (meleePressed) {
                    // Debug.Log("Drawing...");
                    // Debug.DrawRay(transform.position,
                    //     new Vector3(meleeRadius, meleeRadius, 0),
                    //     Color.red
                    // );
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(transform.position, meleeRadius);
                }
            }
        }


        /* Input System Functions */
        public void OnMelee(InputAction.CallbackContext ctx) {
            meleePressed = ctx.ReadValueAsButton();
            String s = String.Format("Phase: {0}\tMeleePressed: {1}\n", ctx.phase, meleePressed);
            Debug.Log(s);

            if (ctx.performed) {
                Debug.Log("Creating overlap circle...");
                // 1. Create a collider circle
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
                    transform.position,
                    meleeRadius
                    // enemyLayer
                );

                Debug.Log(String.Format("Enemies collided with: {0}\n", hitEnemies.Length));
                foreach (Collider2D enemy in hitEnemies) {
                    Debug.Log(String.Format("Enemy: {0} hit.\n", enemy.name));
                }
            }
        }
    }
}


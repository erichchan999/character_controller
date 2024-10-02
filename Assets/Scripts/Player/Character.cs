using UnityEngine;
using System;
/**
* Parent class for all characters. Assumes the object has a BoxCollider2D component and Rigidbody2D component,
* and that the project has "Ground" layer.
**/
namespace Characters {
    public class Character : MonoBehaviour {
        protected BoxCollider2D boxCollider2d;
        protected Rigidbody2D rBody;
        // protected SpriteRenderer spriteRenderer;
        protected LayerMask groundLayer;
        protected Animator animator;
        protected Transform positions;
        private bool isGrounded = false;
        public bool IsGrounded {
            get {return isGrounded;}
            set {isGrounded = value;}
        }
        private bool isFacingRight = true;
        public bool IsFacingRight {
            get {return isFacingRight;}
        }
        private bool isHorizontalStationary= true;
        public bool IsHorizontalStationary {
            get {return isHorizontalStationary;}
        }
        protected const string groundLayerName = "Ground";
        protected const string positionsName = "Positions";
        private const string animHorizontalSpeedName = "HorizontalSpeed";
        private const string animVerticalVelocityName = "VerticalVelocity";
        private const string animIsGroundedName = "IsGrounded";
        
        virtual protected void Start() {
            boxCollider2d = gameObject.GetComponent<BoxCollider2D>();
            rBody = gameObject.GetComponent<Rigidbody2D>();
            // spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            animator = gameObject.GetComponent<Animator>();
            groundLayer = LayerMask.GetMask(groundLayerName);

            positions = transform.Find(positionsName);
            
            rBody.freezeRotation = true;
        }

        virtual protected void Update() {
            UpdateIsFacingRight();
            FlipCharacter();
            UpdateIsHorizontalStationary();
            GroundCheck();
        }

        virtual protected void FixedUpdate() {
        }

        virtual protected void LateUpdate() {
            AnimParams();
        }

        /** Update parameters for animator component, must put in LateUpdate **/
         private void AnimParams() {
            animator.SetFloat(animHorizontalSpeedName, Mathf.Abs(rBody.velocity.x));
            animator.SetFloat(animVerticalVelocityName, rBody.velocity.y);
            animator.SetBool(animIsGroundedName, IsGrounded);
        }

        /** Update calls this to check if player is grounded.
        * 
        * Casts a box protruding slightly below player's own collider box, then
        * checks if any gameobject within the ground layer is colliding with it.
        * If they do then player is grounded. 
        **/
        private void GroundCheck() {
            float extraHeightGroundBox = 0.1f;
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f,
                Vector2.down, extraHeightGroundBox, groundLayer);
            Color rayColor;
            if (raycastHit.collider != null) {
                IsGrounded = true;
                rayColor = Color.red;
            }
            else {
                IsGrounded = false;
                rayColor = Color.blue;
            }

            // Draw the grounded box.
            // right side line
            Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0, 0),
                Vector3.down * (boxCollider2d.bounds.extents.y + extraHeightGroundBox), rayColor);
            // left side line 
            Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0, 0),
                Vector3.down * (boxCollider2d.bounds.extents.y + extraHeightGroundBox), rayColor);
            // bottom line
            Debug.DrawRay(
                boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x,
                    -(boxCollider2d.bounds.extents.y + extraHeightGroundBox), 0),
                Vector3.left * (2 * boxCollider2d.bounds.extents.x), rayColor);
        }

        private void UpdateIsFacingRight() {
            if (rBody.velocity.x > 0.01f) {
                isFacingRight = true;
            } else if (rBody.velocity.x < -0.01f) {
                isFacingRight = false;
            }
        }

        /** Flips the character when moving to the left/right. Positive transform corresponds with character facing right */
        private void FlipCharacter() {
            // if (rBody.velocity.x > 0.01f) {
            //     isFacingRight = true;
            //     spriteRenderer.flipX = false;
            //     positions.transform.localScale = new Vector3(
            //         Math.Abs(positions.transform.localScale.x),
            //         positions.transform.localScale.y,
            //         positions.transform.localScale.z);
            // } else if (rBody.velocity.x < -0.01f) {
            //     isFacingRight = false;
            //     spriteRenderer.flipX = true;
            //     positions.transform.localScale = new Vector3(
            //         -Math.Abs(positions.transform.localScale.x),
            //         positions.transform.localScale.y,
            //         positions.transform.localScale.z);
            // }
            
            if (IsFacingRight) {
                transform.localScale = new Vector3(
                    Math.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z);
            } else if (!IsFacingRight) {
                transform.localScale = new Vector3(
                    -Math.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z);
            }
        }

        private void UpdateIsHorizontalStationary() {
            if (rBody.velocity.x < 0.01f && rBody.velocity.x > -0.01f) {
                isHorizontalStationary = true;
            } else {
                isHorizontalStationary = false;
            }
        }
    }
}

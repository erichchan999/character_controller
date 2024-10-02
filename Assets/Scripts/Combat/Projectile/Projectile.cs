using UnityEngine;

namespace Combat.Projectile {
    public class Projectile : MonoBehaviour {
        // [SerializeField] float lifetime = 1.5f;
        private float initSpeed = 0;
        public float InitSpeed {
            get { return initSpeed;}
            set => initSpeed = Mathf.Abs(value); 
        }
        // TODO: public Vector2 ...


        private CircleCollider2D circleCollider2d;

        // Start is called before the first frame update
        protected virtual void Start() {
            circleCollider2d = GetComponent<CircleCollider2D>();
            circleCollider2d.isTrigger = true;
        }

        // Update is called once per frame
        void Update() {

        }

        void OnTriggerEnter2D(Collider2D collision) {
            // collision.gameObject.GetComponent<Enemy>().TakeDamage();
        }
    }
}
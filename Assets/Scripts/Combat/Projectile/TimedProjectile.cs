using System.Transactions;
using UnityEngine;

namespace Combat.Projectile {
    public class TimedProjectile : Projectile {
        [SerializeField] float lifetime = 1.5f;
        protected override void Start() {
            base.Start();
            Destroy(gameObject, lifetime);
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem.Android;

namespace Combat.Damages {
    public class DiscreteDamage : Damage {
        
        // Constrained to integer values.
        public DiscreteDamage(int dmg) : base((float) dmg) {}
    }
}
using System;
using System.Collections.Generic;
using Combat.Heals;
using Combat.Damages;
using UnityEditor.Experimental;
using UnityEngine;


namespace Combat.Health {
    /**
     * This class is responsible for managing the health of a Character.
     *
     * Please note:
     * You must set up your health system properly before calling InitialiseUIDelegate().
     * Else, outdated information may be passed down via the proxy.
     *
     * Example Usage:
     * HealthSystem hs = new HealthSystem();
     * hs.addHealthBar(new NormalHealthBar(capacity, fullHealth?)
     *
     * DiscreteDamage dd = new DiscreteDamage(5);
     * hs.TakeDamage(dd);
     * if (hs.IsEmpty) --> dead.
     * 
     */
    public class HealthSystem {
        
        /*
         * Protects the health system instance but allows for
         * necessary information to be queried.
         */
        public HealthSystemQueryProxy proxy;        // protection proxy 
        
        private bool _isEmpty;
        public bool IsEmpty { get; set; }
        
        public List<HealthBar> healthBars;

        /**
         * Constructors
         */
        public HealthSystem() {
            healthBars = new List<HealthBar>();
            proxy = new HealthSystemQueryProxy(this);
        }
        
        
        /**********************************************
         ************ Public Methods ******************
         **********************************************/
        
        
        /**
         * Finds the last health bar non-empty health bar and take health from it.
         * Supports discrete damage and float damage.
         * Discrete damage pops a Health Segment off the appropriate health bar stack.
         * Float damage peeks at the top of the appropriate health bar stack and reduce
         * its float value until its empty. If health segment's float value is 0,
         * then it is popped off the health bar stack and repeats until damage remaining is 0.
         */
        public void TakeDamage(Damage damage) {
            // take damage
            float dmgRemaining = damage.Value;
            for (int i = healthBars.Count - 1; i > -1; i--) {
                if (dmgRemaining <= 0)
                    break;
                // discrete damage
                if (damage is DiscreteDamage) {
                    damage = (DiscreteDamage) damage;
                    if (healthBars[i].isEmpty) {
                        continue;
                    }
                    dmgRemaining = TakeDiscreteDamage(dmgRemaining, healthBars[i]);
                }
                else {      // float damage
                    while (dmgRemaining > 0) {
                        if (healthBars[i].isEmpty) {
                            break;
                        }
                        dmgRemaining = TakeFloatDamage(dmgRemaining, healthBars[i]);
                    }
                }
            }
            // delegate health ui update 
            // UIDelegate.UpdateHealthUI(proxy);
        }

        private float TakeDiscreteDamage(float dmgRemaining, HealthBar hb) {
            if (dmgRemaining < hb.SegmentCount) {
                hb.Pops((int) dmgRemaining);      // TODO  - allow > 1 segments to be taken off
                return 0;
            }
            else {
                dmgRemaining -= hb.SegmentCount;   // first, recompute dmg remaining with current count
                hb.Pops(hb.SegmentCount);          // empty the healthbar
                return dmgRemaining;
            }
        }
        private float TakeFloatDamage(float dmg, HealthBar hb) {
            HealthBar.HealthSegment hs = hb.Peek();
            if (hs.CurrentHealth - dmg > 0) {
                hs.CurrentHealth -= dmg;
                return 0;
            }
            else {
                hb.Pop();
                dmg -= hs.CurrentHealth;
                return dmg;
            }
        }
        
        
        /**
         * Finds the last health bar that is not at full health and add health to it.
         * Supports discrete heal and float heal.
         * Discrete heal will push a Health Segment onto the appropriate health bar stack.
         * Float heal will extract the float value from the Health Segment at the top
         * of the stack and start filling it up until max health segment is reached, then
         * pushes a new empty Health Segment onto it and repeat until there's no heal remaining.
         */
        public void RestoreHealth(Heal heal) {
            // restores health to health bar
            float healRemaining = heal.Value;
            for (int i = 0; i < healthBars.Count; i++) {
                if (healRemaining <= 0)
                    break;
                
                if (heal is DiscreteHeal) {
                    heal = (DiscreteHeal) heal;
                    if (healthBars[i].isFull) {
                        continue;
                    }
                    healRemaining = RestoreDiscreteHealth(healRemaining, healthBars[i]);
                }
                else {
                    while (healRemaining > 0) {
                        if (healthBars[i].isFull)
                            break;
                        healRemaining = RestoreFloatHealth(healRemaining, healthBars[i]);
                    }
                }
            }
            // UIDelegate?.UpdateHealthUI(proxy);
        }

        private float RestoreDiscreteHealth(float healRemaining, HealthBar hb) {
            if (healRemaining > (hb.Capacity - hb.SegmentCount)) {
                healRemaining -= (hb.Capacity - hb.SegmentCount);
                hb.MakeFullHealth();
                return healRemaining;
            }
            else {
                hb.Pushes((int) healRemaining, true);
                healRemaining = 0;
                return healRemaining;
            }
        }

        private float RestoreFloatHealth(float healRemaining, HealthBar hb) {
            if (hb.SegmentCount <= 0)
                hb.Pushes(1, false);
            HealthBar.HealthSegment hs = hb.Peek();
            // doesn't fill all the way up, then...
            if (hs.CurrentHealth + healRemaining < hb.SegmentMaxHealth) {
                hs.CurrentHealth += healRemaining;
                return 0;
            }
            // does fill all the way up, then...
            // fill it up and start a blank one
            else {
                healRemaining -= (hb.SegmentMaxHealth - hs.CurrentHealth);
                hs.CurrentHealth = hb.SegmentMaxHealth;
                if (!hb.isFull)
                    hb.Pushes(1, false);        // start a blank for the next iteration
                return healRemaining;
            }
        }

        /**
         * Factory Methods
         */
        public void DefaultBuild() {
            healthBars.Add(new NormalHealthBar(5, true));        // defaults to 5 segments
        }
        
        /**
         * Manually construct your health system
         */

        public void AddHealthBar(HealthBar hb) {
            healthBars.Add(hb);
        }

        /**
         * This class acts as a proxy to query the Health System
         * It protects the instance by allowing only necessary information to be exposed.
         * This is passed down the UIDelegate.
         */
        public class HealthSystemQueryProxy {
            private readonly HealthSystem _healthSystem;

            public HealthSystemQueryProxy(HealthSystem hs) {
                this._healthSystem = hs;
            }
            
            // number of health bars
            public int NumberOfHealthBars {
                get => this._healthSystem.healthBars.Count; 
            }

            public int getCapacityFor(int healthBarIdx) {
                return _healthSystem.healthBars[healthBarIdx].Capacity;
            }
            
            public int getHealthSegmentsFor(int healthBarIdx) {
                return _healthSystem.healthBars[healthBarIdx].SegmentCount;
            }
            public int getFullHealthSegmentsFor(int healthBarIdx) {
                return _healthSystem.healthBars[healthBarIdx].FullSegmentsCount;
            }
        }
    }
}
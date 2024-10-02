using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Health {
    /**
     * Represents the health bar of any character.
     *
     * When instantiated, the healthbar is Full.
     */
    public class HealthBar {
        public int Capacity { get; set; } = 5;        // default
        public virtual float SegmentMaxHealth { get; set; } = 10f;
        
        public bool isEmpty {
            get => bar.Count == 0;
        }

        public bool isFull {
            get => bar.Count == Capacity && bar.Peek().CurrentHealth == SegmentMaxHealth;
        }
        public int SegmentCount {
            get => bar.Count;
        }

        public int FullSegmentsCount {
            get {
                if (isEmpty)
                    return 0;
                
                if (bar.Peek().CurrentHealth == SegmentMaxHealth) {
                    return SegmentCount;
                }
                else {
                    return SegmentCount - 1;
                }
            }
        }

        // Convenience Constructor
        protected HealthBar(int cap, bool fullHealth) : this(fullHealth) {
            this.Capacity = cap;
        }
    
        // Constructor
        protected HealthBar(bool fullHealth) {
            bar = new Stack<HealthSegment>(Capacity);
            if (fullHealth) 
                MakeFullHealth();
            Debug.Log("HealthBar instantiated: Capacity = " + Capacity);
            Debug.Log("HealthBar instantiated: Number of Health Segments = " + SegmentCount);
            Debug.Log("HealthBar instantiated: Full Health Segments = " + FullSegmentsCount);
        }
        
        /** Internals **/
        
        private protected Stack<HealthSegment> bar;
        
        
        
        /** Operations **/

        public void Push(HealthSegment hs) {
            if (bar.Count == Capacity)
                throw new InvalidOperationException("Stack is full.");
            bar.Push(hs);
            Debug.Log("Pushed: Current HealthBar Count = " + FullSegmentsCount);
            _UIdelegate?.UIPush();
        }
        public HealthSegment Pop() {
            HealthSegment hs = bar.Pop();
            Debug.Log("Popped: Current HealthBar Count = " + FullSegmentsCount);
            _UIdelegate?.UIPop();
            return hs;
        }

        public HealthSegment Peek() {
            return bar.Peek();
        }
        
        
        /** Convenience Methods **/
        
        /** Fill up the health bar to capacity **/
        public void MakeFullHealth() {
            if (bar.Count > 0) {
                bar.Peek().CurrentHealth = this.SegmentMaxHealth;
            }
            Pushes(Capacity - bar.Count, true);
        }

        public virtual void Pushes(int n, bool fullHealth) {
            float health = fullHealth ? SegmentMaxHealth : 0;
            for (int i = 0; i < n; i++) {
                Push(new HealthSegment(health));
            }
        }

        public void Pops(int n) {
            for (int i = 0; i < n; i++) {
                Pop();
            }
        }

        public class HealthSegment {
            public HealthSegment(float health) {
                this.CurrentHealth = health;
            }
            public float CurrentHealth { get; set; }
        }

        private IHealthBarUIDelegate _UIdelegate;
        public void registerUIDelegate(IHealthBarUIDelegate UIdelegate) {
            this._UIdelegate = UIdelegate;
            Debug.Log("UIDelegate registered");
            Debug.Log(this._UIdelegate);
        }
    }
}
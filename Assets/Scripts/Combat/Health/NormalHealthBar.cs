namespace Combat.Health {
    public class NormalHealthBar : HealthBar {

        public NormalHealthBar(bool fullHealth) : base(fullHealth) {
        }

        public NormalHealthBar(int capacity, bool fullHealth) : base(capacity, fullHealth) {
        }

        public override float SegmentMaxHealth { get; set; } = 10f;
        public override void Pushes(int n, bool fullHealth) {
            float health = fullHealth ? SegmentMaxHealth : 0;
            for (int i = 0; i < n; i++) {
                base.Push(new NormalHealthSegment(health));
            }
        }
        

        public class NormalHealthSegment : HealthSegment {
            public NormalHealthSegment(float health) : base(health) { }
        }
    }
}
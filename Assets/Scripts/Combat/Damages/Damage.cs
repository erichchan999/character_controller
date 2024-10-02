namespace Combat.Damages {
    public class Damage {
        public Damage(float dmg) {
            this.Value = dmg;
        }
        public string Name { get; set; }

        public float Value { get; set; }
    }
}
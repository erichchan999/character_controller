using System.Collections;
using Combat;
using Combat.Health;
using Combat.Damages;
using Combat.Heals;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assertions = UnityEngine.Assertions;

public class HealthSystemTest  {
    // Take damage with a health system of a single health bar.
    [Test]
    public void TakeFloatDamageSingleBarTestPasses() {
        // create health system
        HealthSystem hs = new HealthSystem();
        hs.AddHealthBar(new NormalHealthBar(5, true));
        int segments = hs.healthBars[0].SegmentCount;
        Debug.Log(segments);
        float dmg = 1.5f*(hs.healthBars[0].SegmentMaxHealth);
        Damage damage = new Damage(dmg);
        hs.TakeDamage(damage);
        
        // result => count = segments - 1 && top healthsegment's currenthealth = 0.5*SegmentMaxHealth
        Assertions.Assert.AreEqual(hs.healthBars[0].SegmentCount, segments - 1);
        Assertions.Assert.AreEqual(hs.healthBars[0].Peek().CurrentHealth, 0.5f * hs.healthBars[0].SegmentMaxHealth);
        // Assertions.Assert.IsTrue(hs.healthBars[0].Peek() is NormalHealthBar.NormalHealthSegment);
    }
    
    // Take damage with a health system of 2 health bars
    // damage will clear out the first bar and into the second.
    [Test]
    public void TakeFloatDamageDoubleBarTestPasses() {
        // Debug.Log("Creating Health System...");
        HealthSystem hs = new HealthSystem();
        hs.AddHealthBar(new NormalHealthBar(5, true));
        hs.AddHealthBar(new NormalHealthBar(5, true));

        // Debug.Log("Health system created with 2 normal health bars...");
        int segments = hs.healthBars[1].SegmentCount;
        float dmg = segments * (hs.healthBars[1].SegmentMaxHealth) + 0.5f * hs.healthBars[0].SegmentMaxHealth;
        Damage damage = new Damage(dmg);
        hs.TakeDamage(damage);
        
        
        // result => count = 0, count = segments && top healthsegment of hs[0] is at 0.5*SegmentMaxHealth
        Assertions.Assert.AreEqual(hs.healthBars[1].SegmentCount, 0);
        Assertions.Assert.AreEqual(hs.healthBars[0].SegmentCount, hs.healthBars[0].Capacity);
        Assertions.Assert.AreEqual(hs.healthBars[0].Peek().CurrentHealth, 0.5f * hs.healthBars[0].SegmentMaxHealth);
    }

    [Test]
    public void TakeDiscreteDamageDoubleBarTestPasses() {
        HealthSystem hs = new HealthSystem();
        hs.AddHealthBar(new NormalHealthBar(5, true));
        hs.AddHealthBar(new NormalHealthBar(5, true));

        int dmg = hs.healthBars[1].SegmentCount + 2;
        DiscreteDamage dd = new DiscreteDamage(dmg);
        hs.TakeDamage(dd);
        
        // result => count = 0, count = 3
        Assertions.Assert.AreEqual(hs.healthBars[1].SegmentCount, 0);
        Assertions.Assert.AreEqual(hs.healthBars[0].SegmentCount, 3);
    }

    [Test]
    public void RestoreDiscreteHealSingleBarTestPasses() {
        
    }
    
    [Test]
    public void RestoreDiscreteHealDoubleBarTestPasses() {
        HealthSystem hs = new HealthSystem();
        hs.AddHealthBar(new NormalHealthBar(5, false));
        hs.AddHealthBar(new NormalHealthBar(5, false));

        int heal = hs.healthBars[0].Capacity + 2;
        DiscreteHeal dh = new DiscreteHeal(heal);
        hs.RestoreHealth(dh);
        
        // result => full health, 2 health segments
        Assertions.Assert.AreEqual(hs.healthBars[0].SegmentCount, hs.healthBars[0].Capacity);
        Assertions.Assert.AreEqual(hs.healthBars[1].SegmentCount, 2);
    }
    
    [Test]
    public void RestoreFloatHealDoubleBarTestPasses() {
        HealthSystem hs = new HealthSystem();
        hs.AddHealthBar(new NormalHealthBar(5, false));
        hs.AddHealthBar(new NormalHealthBar(5, false));

        float heal = hs.healthBars[0].Capacity*hs.healthBars[0].SegmentMaxHealth + 2.5f*hs.healthBars[1].SegmentMaxHealth;
        Heal dh = new Heal(heal);
        hs.RestoreHealth(dh);
        
        // result => full health, count = 3 and last one has 0.5 health.
        Assertions.Assert.AreEqual(hs.healthBars[0].SegmentCount, hs.healthBars[0].Capacity);
        Assertions.Assert.AreEqual(hs.healthBars[1].SegmentCount, 3);
        Assertions.Assert.AreEqual(hs.healthBars[1].Peek().CurrentHealth, 0.5f*hs.healthBars[1].SegmentMaxHealth);
    }
    
    
    /* Pending Test cases   TODO - 29.05.21
     * 1. Restore overflow -> full health
     * 2. Take damage overflow -> instant death
     */
}

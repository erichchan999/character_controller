using UnityEngine;
using Combat.Projectile;
using Characters;

/**
 * This class is responsible for providing methods to exhibit range attack behaviours.
 *
 * Spawning projectiles
 */

public class RangeAttack {
    private Character rangeAttacker;        // the associated gameobject
    public Projectile projPrefab;               // prefab
    public GameObject projSpawnPosition;        // spawn position

    /* Constructor */
    public RangeAttack(
        Character rangeAttacker,
        GameObject projSpawnPosition,
        Projectile projPrefab) {
        this.rangeAttacker = rangeAttacker;
        this.projSpawnPosition = projSpawnPosition;
        this.projPrefab = projPrefab;
    }

    
    // TODO: Spawn multiple projectile -- pass projectile as argument
    public void SpawnProjectile() {
        GameObject projectile =
            UnityEngine.Object.Instantiate(projPrefab.gameObject, projSpawnPosition.transform.position, Quaternion.identity);
            Rigidbody2D rBody = projectile.GetComponent<Rigidbody2D>();
        if (rangeAttacker.IsFacingRight) {
                rBody.velocity = new Vector2(projPrefab.InitSpeed, 0);
        } else {
            rBody.velocity = new Vector2(-projPrefab.InitSpeed, 0);
        }
    }
}

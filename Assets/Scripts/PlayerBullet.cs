using UnityEngine;

public class PlayerBullet : Bullet
{


    public BossEnemy bossEnemyCs;
    

    // Handles collisions specific to the player's bullet
    protected override void HandleCollision(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

            EnemyFollow enemyScript = other.gameObject.GetComponent<EnemyFollow>();

            

            if (enemyScript.HitsTaken >= enemyScript.DamageHits)
            {
                Explode(other.transform.position, other.transform.rotation);
                Destroy(other.gameObject); // Destroy the enemy
                gunController.killsUpdate();
            }
            else
            {
                enemyScript.TakeDamage();
            }
            

            
        }

        if (other.gameObject.CompareTag("Boss"))
        {
            //Transform Tposition = bossEnemyCs.Crackpoints[Random.Range(0, bossEnemyCs.Crackpoints.Length)];

            GameObject var = Instantiate(bossEnemyCs.FireExplodePrefab, transform.position, transform.rotation);
            Destroy(var, 3f);

            bossEnemyCs.UpdateBoss();

            Vector3 pushDirection = other.transform.position - transform.position;

            bossEnemyCs.PushBack(pushDirection);
        }

        
    }
}

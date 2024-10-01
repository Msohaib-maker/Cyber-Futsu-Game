using UnityEngine;

public class EnemyBullet : Bullet
{

    public EnemySpawnManager enemySpawnManager;

    // Handles collisions specific to the enemy's bullet
    protected override void HandleCollision(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Explode(other.transform.position, other.transform.rotation);
            //Debug.Log("Player hit!"); // Add player damage logic here

            enemySpawnManager.healthUpdate(2f);
        }
    }
}

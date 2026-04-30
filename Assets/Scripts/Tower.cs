using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Économie")]
    public int purchaseCost = 100;

    [Header("Tir")]
    public int projectileCount = 1;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;
    public int projectileDamage = 10;

    [Header("Portée")]
    public float range = 8f;

    [Header("Cibles détectées")]
    public List<EnemyMovement> detectedEnemies = new List<EnemyMovement>();

    void Update()
    {
        DetectEnemies();
    }

    void DetectEnemies()
    {
        EnemyMovement[] allEnemies = FindObjectsOfType<EnemyMovement>();

        List<EnemyMovement> enemiesInRange = allEnemies
            .Where(enemy =>
                Vector2.Distance(
                    new Vector2(transform.position.x, transform.position.z),
                    new Vector2(enemy.transform.position.x, enemy.transform.position.z)
                ) <= range
            )
            .ToList();

        enemiesInRange = enemiesInRange
            .OrderBy(enemy => enemy.transform.position.z)
            .ToList();

        detectedEnemies = enemiesInRange
            .Take(projectileCount)
            .ToList();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Économie")]
    [Tooltip("Coût d'achat de la tour")]
    public int purchaseCost = 100;

    [Header("Tir")]
    [Tooltip("Nombre de projectiles tirés à chaque attaque")]
    public int projectileCount = 1;

    [Tooltip("Nombre de tirs par seconde")]
    public float fireRate = 1f;

    [Tooltip("Vitesse des projectiles")]
    public float projectileSpeed = 10f;

    [Tooltip("Dégâts infligés par projectile")]
    public int projectileDamage = 10;

    [Header("Portée")]
    [Tooltip("Distance maximale de détection")]
    public float range = 8f;

    [Header("Cibles détectées")]
    [Tooltip("Les 3 ennemis les plus bas sur l'axe Z")]
    public List<EnemyMovement> detectedEnemies = new List<EnemyMovement>();

    void Update()
    {
        DetectEnemies();
    }

    void DetectEnemies()
    {
        // Trouve tous les ennemis de la scène
        EnemyMovement[] allEnemies = FindObjectsOfType<EnemyMovement>();

        // Filtre selon la portée
        List<EnemyMovement> enemiesInRange = allEnemies
            .Where(enemy =>
                Vector3.Distance(transform.position, enemy.transform.position) <= range)
            .ToList();

        // Trie par position Z croissante
        enemiesInRange = enemiesInRange
            .OrderBy(enemy => enemy.transform.position.z)
            .ToList();

        // Garde les 3 plus bas sur l'axe Z
        detectedEnemies = enemiesInRange
            .Take(projectileCount)
            .ToList();
    }

    // Affichage de la portée dans l'éditeur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
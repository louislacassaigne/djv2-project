using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Références")]
    public Tower tower;

    [Tooltip("Prefab de projectile désactivé")]
    public Projectile projectilePrefab;

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    [Header("Spawn")]
    public Transform firePoint;

    private EnemyMovement currentTarget;

    private float fireCooldown = 0f;

    void Update()
    {
        UpdatePriorityTarget();
        RotateTowardsTarget();

        HandleFireRate();
    }

    /// <summary>
    /// Gère la cadence de tir
    /// </summary>
    void HandleFireRate()
    {
        if (tower == null)
            return;

        fireCooldown -= Time.deltaTime;

        if (tower.detectedEnemies.Count == 0)
            return;

        if (fireCooldown <= 0f)
        {
            Shoot();

            fireCooldown = 1f / tower.fireRate;
        }
    }

    /// <summary>
    /// Récupère l'ennemi prioritaire
    /// </summary>
    void UpdatePriorityTarget()
    {
        if (tower == null)
            return;

        if (tower.detectedEnemies.Count > 0)
        {
            currentTarget = tower.detectedEnemies[0];
        }
        else
        {
            currentTarget = null;
        }
    }

    /// <summary>
    /// Oriente l'arme vers la cible prioritaire
    /// </summary>
    void RotateTowardsTarget()
    {
        if (currentTarget == null)
            return;

        Vector3 direction = currentTarget.transform.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// Tire sur tous les ennemis détectés
    /// </summary>
    void Shoot()
    {
        if (tower == null || projectilePrefab == null)
            return;

        foreach (EnemyMovement enemy in tower.detectedEnemies)
        {
            if (enemy == null)
                continue;

            // Position de spawn
            Vector3 spawnPosition = firePoint != null
                ? firePoint.position
                : transform.position;

            // Création du projectile
            Projectile projectile = Instantiate(
                projectilePrefab,
                spawnPosition,
                Quaternion.identity
            );

            // Active le projectile
            projectile.gameObject.SetActive(true);

            // Configure le projectile
            projectile.target = enemy;
            projectile.speed = tower.projectileSpeed;
            projectile.damage = tower.projectileDamage;
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Cible")]
    public Transform target;

    [Header("Statistiques")]
    public float speed = 1f;
    public int maxHealth = 100;
    public int currentHealth;

    [Tooltip("Score gagné si l'ennemi est tué")]
    public int scoreValue = 10;

    [Tooltip("Monnaie gagnée si l'ennemi est tué")]
    public int reward = 5;

    private NavMeshAgent agent;
    private Animator animator;

    private bool isDeadOrFinished = false; // 🔥 IMPORTANT

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        if (agent != null)
        {
            agent.speed = speed;
        }

        if (animator != null)
        {
            animator.SetTrigger("start_running");
        }

        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    void Update()
    {
        if (isDeadOrFinished)
            return;

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            ReachGoal();
        }
    }

    // -------------------------
    // DAMAGE
    // -------------------------
    public void TakeDamage(int damage)
    {
        if (isDeadOrFinished)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // -------------------------
    // MORT
    // -------------------------
    void Die()
    {
        if (isDeadOrFinished)
            return;

        isDeadOrFinished = true;

        Debug.Log("Ennemi éliminé");

        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.EnemyKilled(scoreValue, reward);
        }

        Destroy(gameObject);
    }

    // -------------------------
    // OBJECTIF ATTEINT
    // -------------------------
    void ReachGoal()
    {
        if (isDeadOrFinished)
            return;

        isDeadOrFinished = true;

        Debug.Log("Base touchée");

        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.EnemyReachedEnd();
        }

        Destroy(gameObject);
    }
}
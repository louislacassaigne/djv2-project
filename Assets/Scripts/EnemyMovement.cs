using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Cible")]
    public Transform target;

    [Header("Statistiques")]
    public float speed = 3.5f;
    public int maxHealth = 100;
    public int currentHealth;

    [Tooltip("Score gagné si l'ennemi est tué")]
    public int scoreValue = 10;

    [Tooltip("Monnaie gagnée si l'ennemi est tué")]
    public int reward = 5;

    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Initialise les PV
        currentHealth = maxHealth;

        // Configure la vitesse de déplacement
        agent.speed = speed;

        // Lance l'animation de course
        if (animator != null)
        {
            animator.SetTrigger("start_running");
        }

        // Définit la destination
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    void Update()
    {
        // Vérifie si l'ennemi est arrivé
        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            ReachGoal();
        }
    }

    /// <summary>
    /// Inflige des dégâts à l'ennemi
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log(gameObject.name + " prend " + damage + " dégâts.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Mort de l'ennemi
    /// </summary>
    void Die()
    {
        Debug.Log("Ennemi éliminé");

        // TODO :
        // Ajouter scoreValue au score
        // Ajouter reward à la monnaie du joueur

        Destroy(gameObject);
    }

    /// <summary>
    /// L'ennemi atteint la fin du chemin
    /// </summary>
    void ReachGoal()
    {
        Debug.Log("Base touchée");

        // TODO :
        // Retirer scoreValue du score
        // Retirer des PV au joueur/base

        Destroy(gameObject);
    }
}
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Cible")]
    public EnemyMovement target;

    [Header("Statistiques")]
    public float speed = 10f;
    public int damage = 10;

    [Header("Trajectoire")]
    public float arcHeight = 2f;

    private Vector3 startPosition;
    private Vector3 previousPosition;
    private Vector3 targetPosition;

    private float journeyLength;
    private float journeyProgress = 0f;

    private bool initialized = false;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        MoveAlongArc();
    }

    void Initialize()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        startPosition = transform.position;
        targetPosition = target.transform.position;

        journeyLength = Vector3.Distance(startPosition, targetPosition);

        previousPosition = startPosition;

        initialized = true;
    }

    void MoveAlongArc()
    {
        if (!initialized || target == null)
        {
            Destroy(gameObject);
            return;
        }

        targetPosition = target.transform.position;

        journeyProgress += speed * Time.deltaTime;

        float t = journeyProgress / journeyLength;

        if (t >= 1f)
        {
            HitTarget();
            return;
        }

        // Position cible de cette frame
        Vector3 newPosition = Vector3.Lerp(
            startPosition,
            targetPosition,
            t
        );

        // Arc vertical
        newPosition.y += Mathf.Sin(t * Mathf.PI) * arcHeight;

        // Calcul direction réelle du mouvement
        Vector3 moveDirection = newPosition - previousPosition;

        // Déplacement
        transform.position = newPosition;

        // Orientation vers la trajectoire
        if (moveDirection.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection.normalized);
        }

        previousPosition = newPosition;
    }

    void HitTarget()
    {
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
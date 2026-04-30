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

    // Position actuelle
    Vector3 currentPosition = transform.position;

    // Position cible
    Vector3 newPosition = Vector3.Lerp(
        startPosition,
        targetPosition,
        t
    );

    newPosition.y += Mathf.Sin(t * Mathf.PI) * arcHeight;

    // Petite anticipation pour une direction stable
    Vector3 nextPosition = Vector3.Lerp(
        startPosition,
        targetPosition,
        t + 0.01f
    );

    nextPosition.y += Mathf.Sin((t + 0.01f) * Mathf.PI) * arcHeight;

    Vector3 direction = nextPosition - newPosition;

    // Déplacement
    transform.position = newPosition;

    // Rotation propre
    if (direction.sqrMagnitude > 0.0001f)
    {
        transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
    }
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
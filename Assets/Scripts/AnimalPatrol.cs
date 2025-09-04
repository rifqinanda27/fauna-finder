using UnityEngine;

public class AnimalPatrolChase : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float stoppingDistance = 0.2f;

    [Header("Chase Settings")]
    public bool enableChase = true;      // ⬅️ bisa diatur lewat inspector
    public float chaseSpeed = 4f;
    public float chaseRange = 5f;
    public float stopChaseDistance = 1.5f;

    [Header("Debug Info")]
    [SerializeField] private bool isChasing = false; // status runtime

    private int currentWaypoint = 0;
    private Transform player;
    private Animator animator;

    public Transform respawnPoint;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!enableChase)
        {
            // Kalau chase dimatikan, selalu patroli
            Patrol();
            isChasing = false;
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            isChasing = true;
        }
        else if (distance > chaseRange + 1f) // buffer
        {
            isChasing = false;
        }

        if (isChasing)
            ChasePlayer();
        else
            Patrol();
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 dir = (target.position - transform.position).normalized;

        transform.position += dir * patrolSpeed * Time.deltaTime;

        if (dir != Vector3.zero)
            transform.forward = dir;

        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);

        if (Vector3.Distance(transform.position, target.position) < stoppingDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < stopChaseDistance)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            return;
        }

        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        transform.position += dir * chaseSpeed * Time.deltaTime;

        if (dir != Vector3.zero)
            transform.forward = dir;

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopChaseDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 🚫 Kalau chase dimatikan, jangan respawn player
        if (!enableChase) return;

        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null && respawnPoint != null)
            {
                pc.Respawn(respawnPoint.position);
            }
        }
    }
}

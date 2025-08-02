using UnityEngine;

public class NPCPatrolInteract : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 2f;
    public float stoppingDistance = 0.2f;
    public float interactionRange = 2f;
    public KeyCode interactKey = KeyCode.E;

    private int currentWaypoint = 0;
    private bool isInteracting = false;
    private Transform player;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isInteracting)
        {
            animator.SetBool("isWalking", false); // idle saat interaksi

            if (Input.GetKeyDown(interactKey))
            {
                EndInteraction();
            }
            return;
        }

        Patrol();

        if (Vector3.Distance(transform.position, player.position) <= interactionRange)
        {
            if (Input.GetKeyDown(interactKey))
            {
                StartInteraction();
            }
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 dir = (target.position - transform.position).normalized;

        // NPC jalan
        transform.position += dir * moveSpeed * Time.deltaTime;

        // Rotasi hadap ke arah gerak
        if (dir != Vector3.zero)
            transform.forward = dir;

        animator.SetBool("isWalking", true); // animasi jalan ON

        if (Vector3.Distance(transform.position, target.position) < stoppingDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        animator.SetBool("isWalking", false); // pastikan idle saat mulai interaksi
        Debug.Log("Started interaction with NPC!");
    }

    void EndInteraction()
    {
        isInteracting = false;
        Debug.Log("Ended interaction with NPC!");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}

using UnityEngine;

public class NPCPatrolInteract : MonoBehaviour
{
    public Transform[] waypoints; // Set in Inspector
    public float moveSpeed = 2f;
    public float stoppingDistance = 0.2f;
    public float interactionRange = 2f;
    public KeyCode interactKey = KeyCode.E;

    private int currentWaypoint = 0;
    private bool isInteracting = false;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isInteracting)
        {
            // NPC is interacting, do not move
            if (Input.GetKeyDown(interactKey))
            {
                EndInteraction();
            }
            return;
        }

        Patrol();

        // Check interaction range
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

        // Move toward the current waypoint
        transform.position += dir * moveSpeed * Time.deltaTime;

        // Face direction of movement
        if (dir != Vector3.zero)
            transform.forward = dir;

        // If close to the waypoint, go to the next one
        if (Vector3.Distance(transform.position, target.position) < stoppingDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        Debug.Log("Started interaction with NPC!");
        // TODO: Trigger dialogue, animation, etc.
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

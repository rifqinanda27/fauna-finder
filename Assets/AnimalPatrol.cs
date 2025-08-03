using UnityEngine;

public class AnimalPatrolInteract : MonoBehaviour
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
        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log("Jarak ke player: " + distance); // Tambahkan ini

        if (isInteracting)
        {
            Debug.Log("Sedang interaksi");
            animator.SetBool("isWalking", false);
            LookAtPlayer();

            if (Input.GetKeyDown(interactKey))
            {
                Debug.Log("Menekan E saat interaksi");
                EndInteraction();
            }
            return;
        }

        Patrol();

        if (distance <= interactionRange)
        {
            Debug.Log("Player dalam jarak interaksi");
            if (Input.GetKeyDown(interactKey))
            {
                Debug.Log("Menekan E untuk interaksi");
                StartInteraction();
            }
        }
    }


    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 dir = (target.position - transform.position).normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;

        if (dir != Vector3.zero)
            transform.forward = dir;

        animator.SetBool("isWalking", true);

        if (Vector3.Distance(transform.position, target.position) < stoppingDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.forward = direction;
    }

    void StartInteraction()
    {
        isInteracting = true;
        animator.SetBool("isWalking", false);
        Debug.Log("Hewan sedang diperhatikan/interaksi.");
        // Optional: bisa tambahkan suara atau animasi khusus
    }

    void EndInteraction()
    {
        isInteracting = false;
        Debug.Log("Interaksi dengan hewan selesai.");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}

using UnityEngine;
using System.Collections.Generic;

public class NPCPatrolInteract : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] waypoints;
    public float moveSpeed = 2f;
    public float stoppingDistance = 0.2f;

    [Header("Interaction Settings")]
    public float interactionRange = 2f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Dialog Settings")]
    [TextArea(2, 5)]
    public List<string> dialogLines; // ⬅️ Bisa isi langsung di Inspector

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
            animator.SetBool("isWalking", false);
            LookAtPlayer();
            return; // hentikan patrol saat interaksi
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

        transform.position += dir * moveSpeed * Time.deltaTime;

        if (dir != Vector3.zero)
            transform.forward = dir;

        animator.SetBool("isWalking", true);

        if (Vector3.Distance(transform.position, target.position) < stoppingDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void StartInteraction()
    {
        if (dialogLines == null || dialogLines.Count == 0) return;

        isInteracting = true;
        animator.SetBool("isWalking", false);

        // Panggil dialog manager dan kasih callback EndInteraction
        DialogManager.Instance.StartDialog(dialogLines, EndInteraction);
    }

    void EndInteraction()
    {
        isInteracting = false;
        Debug.Log($"{gameObject.name} selesai interaksi.");
    }

    void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.forward = direction;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}

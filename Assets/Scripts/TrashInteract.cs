using UnityEngine;

public class TrashInteract : MonoBehaviour
{
    [Header("Interact Settings")]
    public KeyCode interactKey = KeyCode.E; // Tombol untuk ambil sampah
    public float interactRange = 2f;        // Jarak interaksi

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        // Cek jarak player dengan sampah
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactRange)
        {
            // Kalau player tekan tombol interact
            if (Input.GetKeyDown(interactKey))
            {
                CollectTrash();
            }
        }
    }

    void CollectTrash()
    {
        Debug.Log("Sampah diambil: " + gameObject.name);
        Destroy(gameObject); // Hapus sampah dari scene
    }

    void OnDrawGizmosSelected()
    {
        // Biar gampang lihat jarak interaksi di editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}

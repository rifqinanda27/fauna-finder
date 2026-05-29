using UnityEngine;

public class UIFloatAnim : MonoBehaviour
{
    public float floatSpeed = 2f; // Kecepatan naik turun
    public float floatHeight = 10f; // Jarak naik turun
    public float offset = 0f; // Jeda waktu (biar nggak barengan bergeraknya)

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Matematika sederhana untuk efek melayang (Sine Wave)
        float newY = startPos.y + (Mathf.Sin(Time.time * floatSpeed + offset) * floatHeight);
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}
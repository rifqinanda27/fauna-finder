using UnityEngine;

public class ButtonIdleAnim : MonoBehaviour
{
    public float scaleSpeed = 1.5f;   // kecepatan animasi
    public float scaleAmount = 0.05f; // seberapa besar skala naik turun

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
        transform.localScale = originalScale * scale;
    }
}

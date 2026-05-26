using UnityEngine;

public class TitleIdleAnim : MonoBehaviour
{
    public float moveSpeed = 2f;   // kecepatan goyang
    public float moveAmount = 10f; // seberapa jauh goyang

    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        transform.localPosition = originalPos + new Vector3(0, y, 0);
    }
}

using UnityEngine;

public class FPSCameraLook : MonoBehaviour
{
    public Transform cameraPivot; // drag FPSCamera ke sini
    public float mouseSensitivity = 2f;
    public float pitchClamp = 80f;
    public float yawClamp = 90f; // Batas rotasi horizontal (kiri-kanan)

    public bool isActive = false;

    [HideInInspector] public float yaw = 0f;
    [HideInInspector] public float pitch = 0f;
    [HideInInspector] public float initialYaw = 0f;

    void Start()
    {
        initialYaw = transform.eulerAngles.y;
        yaw = initialYaw;
        pitch = cameraPivot.localEulerAngles.x;
    }

    void Update()
    {
        if (!isActive) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        yaw = Mathf.Clamp(yaw, initialYaw - yawClamp, initialYaw + yawClamp);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f); // Yaw pada holder
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}

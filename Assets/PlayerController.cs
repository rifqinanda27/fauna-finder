using UnityEngine;
using System.IO; // untuk menyimpan file PNG

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public Transform cam; // untuk third person
    public Camera fpsCam; // FPS camera biasa
    public GameObject fpsCamHolder; // Holder untuk rotasi horizontal

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;
    public Transform groundCheck;

    private Animator animator;
    public FixedJoystick joystick;

    private bool isInPhotoMode = false;
    private FPSCameraLook fpsLook;

    // Tambahan untuk foto
    public int photoWidth = 1920;
    public int photoHeight = 1080;
    public string photoSaveFolder = "Photos"; // folder dalam Assets

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        fpsLook = fpsCamHolder.GetComponent<FPSCameraLook>();
        fpsLook.isActive = false;
        fpsCam.gameObject.SetActive(false);
        fpsLook.initialYaw = fpsCamHolder.transform.eulerAngles.y;
        fpsLook.yaw = fpsLook.initialYaw;
    }

    void Update()
    {
        // Toggle masuk mode FPS foto
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("FPS Mode: " + isInPhotoMode);
            isInPhotoMode = !isInPhotoMode;
            fpsCam.gameObject.SetActive(isInPhotoMode);
            cam.gameObject.SetActive(!isInPhotoMode);

            fpsLook.isActive = isInPhotoMode;

            Cursor.lockState = isInPhotoMode ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isInPhotoMode;

            if (isInPhotoMode)
            {
                Vector3 camEuler = cam.eulerAngles;
                fpsCamHolder.transform.eulerAngles = new Vector3(0f, camEuler.y, 0f);
                fpsCam.transform.localEulerAngles = new Vector3(camEuler.x, 0f, 0f);
                fpsLook.yaw = camEuler.y;
                fpsLook.pitch = camEuler.x;
            }

            Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * 100f, Color.red, 2f);
        }

        if (!isInPhotoMode)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            float h = joystick.Horizontal + Input.GetAxis("Horizontal");
            float v = joystick.Vertical + Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(h, 0f, v).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

                animator.SetFloat("Speed", 1f);
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        if (isInPhotoMode && Input.GetMouseButtonDown(0)) // Tombol kiri mouse
        {
            Debug.Log("Klik kiri di mode foto!");
            TakePhoto();
        }
    }

    void TakePhoto()
    {
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log("Foto mengenai: " + hit.collider.name);

            if (hit.collider.CompareTag("PhotoTarget"))
            {
                Debug.Log("Target berhasil difoto!");
                SaveScreenshot();
            }
            else
            {
                Debug.Log("Bukan target foto.");
            }
        }
        else
        {
            Debug.Log("Tidak ada objek yang difoto.");
        }
    }

    void SaveScreenshot()
    {
        RenderTexture rt = new RenderTexture(photoWidth, photoHeight, 24);
        fpsCam.targetTexture = rt;

        Texture2D photo = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);
        fpsCam.Render();
        RenderTexture.active = rt;
        photo.ReadPixels(new Rect(0, 0, photoWidth, photoHeight), 0, 0);
        photo.Apply();

        fpsCam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = photo.EncodeToPNG();
        string dirPath = Path.Combine(Application.dataPath, photoSaveFolder);
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

        string fileName = "Photo_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullPath = Path.Combine(dirPath, fileName);
        File.WriteAllBytes(fullPath, bytes);

        Debug.Log("Foto disimpan di: " + fullPath);
    }
}

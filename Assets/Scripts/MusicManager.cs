using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance; // Biar gampang dipanggil dan gak dobel

    [Header("Masukkan Audio Clip di Sini")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    private AudioSource audioSource;

    void Awake()
    {
        // Cegah ada 2 Music Manager kalau kita bolak-balik scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Objek ini akan abadi lintas scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true; // Biar musiknya ngulang terus
    }

    void OnEnable()
    {
        // Beri tahu script untuk memantau setiap kali scene baru dimuat
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cek nama scene yang lagi dibuka
        if (scene.name == "MainMenu" || scene.name == "CreditScene")
        {
            PlayMusic(menuMusic);
        }
        else if (scene.name == "SavanaTerrain") // Pastikan nama ini sesuai dengan scene gameplay-mu!
        {
            PlayMusic(gameMusic);
        }
    }

    void PlayMusic(AudioClip newClip)
    {
        // Kalau musik yang mau diputar sudah sama dengan yang lagi jalan, cuekin aja biar gak restart
        if (audioSource.clip == newClip) return;

        audioSource.clip = newClip;
        audioSource.Play();
    }
}
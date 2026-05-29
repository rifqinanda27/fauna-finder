using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // <- perlu untuk LoadScene
using UnityEngine.EventSystems;

public class JournalManager : MonoBehaviour
{
    public GameObject journalCanvas;
    public GameObject mainUIPanel;

    public GameObject panelObjectives;
    public GameObject panelPhotos;
    public GameObject panelSettings;

    public Button buttonObjectives;
    public Button buttonPhotos;
    public Button buttonSettings;

    [Header("Popup Exit")]
    public GameObject confirmExitPanel; // panel popup konfirmasi

    private bool isJournalOpen = false;

    void Start()
    {
        journalCanvas.SetActive(false);
        confirmExitPanel.SetActive(false); // pastikan popup tersembunyi

        // Pasang listener ke tombol
        buttonObjectives.onClick.AddListener(OpenObjectives);
        buttonPhotos.onClick.AddListener(OpenPhotos);
        buttonSettings.onClick.AddListener(OpenSettings);

        OpenObjectives(); // panel default
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleJournal();
        }
    }

    public void ToggleJournal()
    {
        isJournalOpen = !isJournalOpen;
        journalCanvas.SetActive(isJournalOpen);

        if (mainUIPanel != null)
        {
            mainUIPanel.SetActive(!isJournalOpen);
        }

        Time.timeScale = isJournalOpen ? 0f : 1f;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (isJournalOpen)
        {
            OpenObjectives();
        }
        else
        {
            // Reset klik kalau jurnal ditutup lewat shortcut 'J'
            EventSystem.current.SetSelectedGameObject(null); 
        }
    }

    public void CloseJournal()
    {
        isJournalOpen = false;
        journalCanvas.SetActive(false);

        if (mainUIPanel != null)
        {
            mainUIPanel.SetActive(true);
        }

        Time.timeScale = 1f;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // --- INI OBAT PENAWARNYA ---
        // Memaksa Event System mereset status klik yang nyangkut
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OpenObjectives()
    {
        panelObjectives.SetActive(true);
        panelPhotos.SetActive(false);
        panelSettings.SetActive(false);
    }

    public void OpenPhotos()
    {
        panelObjectives.SetActive(false);
        panelPhotos.SetActive(true);
        panelSettings.SetActive(false);
    }

    public void OpenSettings()
    {
        panelObjectives.SetActive(false);
        panelPhotos.SetActive(false);
        panelSettings.SetActive(true);
    }

    // === Tombol Exit di jurnal ===
    public void OnExitButtonPressed()
    {
        confirmExitPanel.SetActive(true); // tampilkan popup konfirmasi
    }

    public void OnConfirmExitYes()
    {
        Time.timeScale = 1f; // resume biar scene load normal
        SceneManager.LoadScene("MainMenu"); // ganti "MainMenu" sesuai nama scene menu kamu
    }

    public void OnConfirmExitNo()
    {
        confirmExitPanel.SetActive(false); // tutup popup
    }
}

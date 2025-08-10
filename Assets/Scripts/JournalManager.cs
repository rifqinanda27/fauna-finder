using UnityEngine;
using UnityEngine.UI;

public class JournalManager : MonoBehaviour
{
    public GameObject journalCanvas;

    public GameObject panelObjectives;
    public GameObject panelPhotos;
    public GameObject panelSettings;

    public Button buttonObjectives;
    public Button buttonPhotos;
    public Button buttonSettings;

    private bool isJournalOpen = false;

    void Start()
    {
        journalCanvas.SetActive(false);

        // Pasang listener ke tombol
        buttonObjectives.onClick.AddListener(OpenObjectives);
        buttonPhotos.onClick.AddListener(OpenPhotos);
        buttonSettings.onClick.AddListener(OpenSettings);

        OpenObjectives(); // Tampilkan panel default saat jurnal dibuka
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Tombol J ditekan");
            ToggleJournal();
        }
    }

    public void ToggleJournal()
    {
        isJournalOpen = !isJournalOpen;
        journalCanvas.SetActive(isJournalOpen);

        Time.timeScale = isJournalOpen ? 0f : 1f; // Pause game saat jurnal dibuka
        Cursor.lockState = isJournalOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isJournalOpen;

        if (isJournalOpen)
        {
            OpenObjectives(); // Default buka panel Objectives
        }
    }

    public void OpenObjectives()
    {
        panelObjectives.SetActive(true);
        panelPhotos.SetActive(false);
        panelSettings.SetActive(false);
        // HighlightButton(buttonObjectives);
    }

    public void OpenPhotos()
    {
        panelObjectives.SetActive(false);
        panelPhotos.SetActive(true);
        panelSettings.SetActive(false);
        // HighlightButton(buttonPhotos);
    }

    public void OpenSettings()
    {
        panelObjectives.SetActive(false);
        panelPhotos.SetActive(false);
        panelSettings.SetActive(true);
        // HighlightButton(buttonSettings);
    }

    public void ExitGame()
    {
        Debug.Log("Keluar dari game...");
        Application.Quit();
    }

    // Tambahan opsional: highlight tombol aktif
    // private void HighlightButton(Button selected)
    // {
    //     Color normalColor = Color.white;
    //     Color highlightColor = Color.yellow;

    //     buttonObjectives.image.color = normalColor;
    //     buttonPhotos.image.color = normalColor;
    //     buttonSettings.image.color = normalColor;

    //     selected.image.color = highlightColor;
    // }
}

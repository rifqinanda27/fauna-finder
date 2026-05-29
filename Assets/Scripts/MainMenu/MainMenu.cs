using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Panggil di OnClick() masing-masing tombol
    public void PlayGame()
    {
        SceneManager.LoadScene("SavanaTerrain"); // ganti sesuai nama scene gameplay
    }

    public void OpenCredit()
    {
        SceneManager.LoadScene("CreditScene"); // ganti sesuai nama scene credit
    }

    public void ExitGame()
    {
        Application.Quit(); // Berfungsi di build Android
        Debug.Log("Keluar Game"); // Debug di editor
    }
}

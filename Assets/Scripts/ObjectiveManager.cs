using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    private List<Objective> objectives = new List<Objective>();

    [Header("UI Reference")]
    [Tooltip("Content di ScrollView tempat list objective ditaruh")]
    public Transform objectivesContainer;

    [Tooltip("Prefab UI untuk satu item objective (isi Text/TMP)")]
    public GameObject objectivePrefab;

    // Keys untuk PlayerPrefs
    private const string PP_COUNT = "Objectives_Count";
    private const string PP_DESC_PREFIX = "Objectives_Desc_";
    private const string PP_DONE_PREFIX = "Objectives_Done_";

    void Awake()
    {
        // Singleton pattern
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        LoadObjectives();   // ⬅️ muat progress saat game mulai
    }

    void OnApplicationQuit()
    {
        SaveObjectives();   // ⬅️ simpan saat keluar game
    }

    /// <summary>
    /// Tambahkan satu objektif baru.
    /// </summary>
    public void AddObjective(string description)
    {
        if (string.IsNullOrEmpty(description)) return;

        objectives.Add(new Objective(description));
        RefreshUI();
        SaveObjectives();   // ⬅️ simpan setiap ada perubahan
    }

    /// <summary>
    /// Tambahkan beberapa objektif sekaligus.
    /// </summary>
    public void AddObjectives(List<string> descriptions)
    {
        foreach (string desc in descriptions)
        {
            if (!string.IsNullOrEmpty(desc))
                objectives.Add(new Objective(desc));
        }
        RefreshUI();
        SaveObjectives();   // ⬅️ simpan setiap ada perubahan
    }

    /// <summary>
    /// Tandai objektif selesai berdasarkan deskripsinya.
    /// </summary>
    public void CompleteObjective(string animalName)
    {
        foreach (var obj in objectives)
        {
            // cek apakah description mengandung nama hewan (case insensitive)
            if (obj.description.IndexOf(animalName, System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                obj.isCompleted = true;
                Debug.Log("✅ Objective completed: " + obj.description);
                return;
            }
        }

        Debug.Log("❌ Objective not found for: " + animalName);
    }

    /// <summary>
    /// Perbarui tampilan UI Objective di jurnal.
    /// </summary>
    private void RefreshUI()
    {
        // Bersihkan dulu semua child UI
        foreach (Transform child in objectivesContainer)
        {
            Destroy(child.gameObject);
        }

        // Spawn ulang berdasarkan daftar objective
        foreach (Objective obj in objectives)
        {
            GameObject item = Instantiate(objectivePrefab, objectivesContainer);

            // Bisa pakai Text atau TextMeshProUGUI
            Text text = item.GetComponent<Text>();
            if (text != null)
            {
                text.text = (obj.isCompleted ? "✔ " : "• ") + obj.description;
            }
        }
    }

    // =======================
    //       SAVE / LOAD
    // =======================

    private void SaveObjectives()
    {
        PlayerPrefs.SetInt(PP_COUNT, objectives.Count);

        for (int i = 0; i < objectives.Count; i++)
        {
            PlayerPrefs.SetString(PP_DESC_PREFIX + i, objectives[i].description);
            PlayerPrefs.SetInt   (PP_DONE_PREFIX + i, objectives[i].isCompleted ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    private void LoadObjectives()
    {
        objectives.Clear();

        int count = PlayerPrefs.GetInt(PP_COUNT, 0);
        for (int i = 0; i < count; i++)
        {
            string desc = PlayerPrefs.GetString(PP_DESC_PREFIX + i, string.Empty);
            bool done   = PlayerPrefs.GetInt   (PP_DONE_PREFIX + i, 0) == 1;

            if (!string.IsNullOrEmpty(desc))
            {
                // Pakai class Objective milikmu yang sudah ada
                Objective o = new Objective(desc);
                o.isCompleted = done;
                objectives.Add(o);
            }
        }

        RefreshUI();
    }

    /// <summary>
    /// Opsional: hapus semua progress tersimpan (berguna saat testing).
    /// </summary>
    public void ClearSavedObjectives()
    {
        int count = PlayerPrefs.GetInt(PP_COUNT, 0);
        for (int i = 0; i < count; i++)
        {
            PlayerPrefs.DeleteKey(PP_DESC_PREFIX + i);
            PlayerPrefs.DeleteKey(PP_DONE_PREFIX + i);
        }
        PlayerPrefs.DeleteKey(PP_COUNT);
        PlayerPrefs.Save();

        objectives.Clear();
        RefreshUI();
    }
}

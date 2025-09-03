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
    /// Tambahkan satu objektif baru (hindari duplicate).
    /// </summary>
    public void AddObjective(string description)
    {
        if (string.IsNullOrEmpty(description)) return;

        // cek dulu kalau sudah ada objektif yang sama (case insensitive)
        foreach (var obj in objectives)
        {
            if (obj.description.Equals(description, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"⚠ Objective '{description}' sudah ada, tidak ditambahkan lagi.");
                return;
            }
        }

        objectives.Add(new Objective(description));
        RefreshUI();
        SaveObjectives();   // ⬅️ simpan setiap ada perubahan
    }

    /// <summary>
    /// Tambahkan beberapa objektif sekaligus (hindari duplicate).
    /// </summary>
    public void AddObjectives(List<string> descriptions)
    {
        foreach (string desc in descriptions)
        {
            if (string.IsNullOrEmpty(desc)) continue;

            bool exists = false;
            foreach (var obj in objectives)
            {
                if (obj.description.Equals(desc, System.StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
                objectives.Add(new Objective(desc));
            else
                Debug.Log($"⚠ Objective '{desc}' sudah ada, dilewati.");
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
                RefreshUI();
                SaveObjectives();
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
            PlayerPrefs.SetInt(PP_DONE_PREFIX + i, objectives[i].isCompleted ? 1 : 0);
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
            bool done = PlayerPrefs.GetInt(PP_DONE_PREFIX + i, 0) == 1;

            if (!string.IsNullOrEmpty(desc))
            {
                Objective o = new Objective(desc);
                o.isCompleted = done;
                objectives.Add(o);
            }
        }

        RefreshUI();
    }

    /// <summary>
    /// Hapus semua progress tersimpan (berguna saat testing).
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

    /// <summary>
    /// Alias untuk editor (biar bisa dipanggil lewat tombol Inspector).
    /// </summary>
    public void ClearAllObjectives()
    {
        ClearSavedObjectives();
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class PhotoGallery : MonoBehaviour
{
    public GameObject photoPrefab; // prefab foto
    public Transform gridParent;   // Content dari Grid Layout Group

    void OnEnable()
    {
        LoadPhotos();
    }

    void LoadPhotos()
    {
        // Hapus foto lama dulu
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        string folderPath = Path.Combine(Application.streamingAssetsPath, "Photos");

        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning("Folder Photos tidak ditemukan: " + folderPath);
            return;
        }

        string[] files = Directory.GetFiles(folderPath);

        foreach (string filePath in files)
        {
            if (filePath.EndsWith(".png") || filePath.EndsWith(".jpg") || filePath.EndsWith(".jpeg"))
            {
                StartCoroutine(LoadImage(filePath));
            }
        }
    }

    IEnumerator LoadImage(string path)
    {
        using (WWW www = new WWW("file://" + path))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                GameObject photoObj = Instantiate(photoPrefab, gridParent);
                Image img = photoObj.GetComponent<Image>();

                Texture2D tex = www.texture;
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
                img.sprite = sprite;
            }
            else
            {
                Debug.LogError("Gagal memuat foto: " + path + " - " + www.error);
            }
        }
    }
}

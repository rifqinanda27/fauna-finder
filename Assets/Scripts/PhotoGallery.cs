using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.Collections;

public class PhotoGallery : MonoBehaviour
{
    public GameObject photoPrefab; // prefab foto (Image)
    public Transform gridParent;   // Content dari Grid Layout Group
    public int thumbnailWidth = 256; // lebar thumbnail
    public int thumbnailHeight = 144; // tinggi thumbnail

    void OnEnable()
    {
        StartCoroutine(LoadPhotosAsync());
    }

    IEnumerator LoadPhotosAsync()
    {
        // Hapus foto lama
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        string folderPath = Path.Combine(Application.streamingAssetsPath, "Photos");

        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning("Folder Photos tidak ditemukan: " + folderPath);
            yield break;
        }

        string[] files = Directory.GetFiles(folderPath);

        foreach (string filePath in files)
        {
            if (filePath.EndsWith(".png") || filePath.EndsWith(".jpg") || filePath.EndsWith(".jpeg"))
            {
                yield return StartCoroutine(LoadImage(filePath));
                yield return null; // jeda 1 frame agar tidak freeze
            }
        }
    }

    IEnumerator LoadImage(string path)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                Texture2D originalTex = DownloadHandlerTexture.GetContent(uwr);

                // Buat thumbnail
                Texture2D thumbnail = ResizeTexture(originalTex, thumbnailWidth, thumbnailHeight);

                // Buat prefab foto
                GameObject photoObj = Instantiate(photoPrefab, gridParent);
                Image img = photoObj.GetComponent<Image>();

                // Ubah jadi sprite
                Sprite sprite = Sprite.Create(thumbnail, new Rect(0, 0, thumbnail.width, thumbnail.height), Vector2.one * 0.5f);
                img.sprite = sprite;
            }
            else
            {
                Debug.LogError("Gagal memuat foto: " + path + " - " + uwr.error);
            }
        }
    }

    // Fungsi resize sederhana untuk buat thumbnail
    Texture2D ResizeTexture(Texture2D source, int width, int height)
    {
        Texture2D result = new Texture2D(width, height, source.format, false);
        Color[] pixels = result.GetPixels(0);
        float incX = (1.0f / width);
        float incY = (1.0f / height);
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = source.GetPixelBilinear(incX * (i % width), incY * Mathf.Floor(i / width));
        }
        result.SetPixels(pixels, 0);
        result.Apply();
        return result;
    }
}

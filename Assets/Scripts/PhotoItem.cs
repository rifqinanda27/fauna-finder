using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PhotoItem : MonoBehaviour
{
    public Image photoImage;
    private string imagePath;
    private string jsonPath;

    public void Setup(string imgPath)
    {
        imagePath = imgPath;
        jsonPath = imgPath.Replace(".png", ".json");
    }

    public void OnClick()
    {
        if (File.Exists(jsonPath))
        {
            string jsonData = File.ReadAllText(jsonPath);
            PlayerController.PhotoData data = JsonUtility.FromJson<PlayerController.PhotoData>(jsonData);

            PhotoDetailUI.Instance.ShowDetail(imagePath, data);
        }
        else
        {
            Debug.LogWarning("JSON untuk foto ini tidak ditemukan: " + jsonPath);
        }
    }
}

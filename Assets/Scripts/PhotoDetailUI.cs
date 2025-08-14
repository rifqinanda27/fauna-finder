using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PhotoDetailUI : MonoBehaviour
{
    public static PhotoDetailUI Instance;

    public GameObject detailPanel;
    public Image largePhoto;
    public Text animalNameText;
    public Text categoryText;
    public Text descriptionText;

    private void Awake()
    {
        Instance = this;
        detailPanel.SetActive(false);
    }

    public void ShowDetail(string imagePath, PlayerController.PhotoData data)
    {
        StartCoroutine(LoadAndShow(imagePath, data));
    }

    IEnumerator LoadAndShow(string path, PlayerController.PhotoData data)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);

                largePhoto.sprite = sprite;
                animalNameText.text = data.animalName;
                categoryText.text = data.category;
                descriptionText.text = data.description;

                detailPanel.SetActive(true);
            }
        }
    }

    public void CloseDetail()
    {
        detailPanel.SetActive(false);
    }
}

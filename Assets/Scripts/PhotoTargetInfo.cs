using UnityEngine;

public class PhotoTargetInfo : MonoBehaviour
{
    [Header("Informasi Hewan")]
    public string animalName;  // Nama hewan
    public string category;    // Karnivora, Herbivora, Omnivora
    [TextArea(3, 10)]
    public string description; // Deskripsi hewan
}

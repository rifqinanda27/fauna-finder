using UnityEngine;

[System.Serializable]
public class Objective
{
    public string description;
    public bool isCompleted;

    public Objective(string desc)
    {
        description = desc;
        isCompleted = false;
    }
}

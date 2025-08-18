using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    public GameObject dialogCanvas;
    public Text dialogText;
    public KeyCode nextKey = KeyCode.E;

    private List<string> lines;
    private int currentLine;
    private bool isDialogActive = false;
    private Action onDialogEnd; // callback

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        if (isDialogActive && Input.GetKeyDown(nextKey))
        {
            DisplayNextLine();
        }
    }

    public void StartDialog(List<string> dialogLines, Action endCallback = null)
    {
        lines = dialogLines;
        currentLine = 0;
        isDialogActive = true;
        onDialogEnd = endCallback;

        dialogCanvas.SetActive(true);
        DisplayNextLine();

        // Lock player movement
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null) player.SetCanMove(false);
    }

    void DisplayNextLine()
    {
        if (currentLine < lines.Count)
        {
            dialogText.text = lines[currentLine];
            currentLine++;
        }
        else
        {
            EndDialog();
        }
    }

    public void EndDialog()
    {
        isDialogActive = false;
        dialogCanvas.SetActive(false);

        // Unlock player movement
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null) player.SetCanMove(true);

        // Callback ke NPC
        if (onDialogEnd != null) onDialogEnd.Invoke();
    }

    public bool IsDialogActive()
    {
        return isDialogActive;
    }
}

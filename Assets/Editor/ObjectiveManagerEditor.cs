using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectiveManager))]
public class ObjectiveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Gambar inspector default dulu
        DrawDefaultInspector();

        ObjectiveManager manager = (ObjectiveManager)target;

        GUILayout.Space(10);

        // Tombol Clear Objectives
        if (GUILayout.Button("🗑️ Clear All Objectives"))
        {
            if (EditorUtility.DisplayDialog(
                "Clear Objectives?",
                "Apakah kamu yakin mau menghapus semua objectives?",
                "Ya", "Batal"))
            {
                manager.ClearAllObjectives();
            }
        }
    }
}

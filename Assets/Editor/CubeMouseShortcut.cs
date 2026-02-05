using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class CubeMouseShortcut
{
    static CubeMouseShortcut()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (e == null) return;

        // Mouse Button 5 (side mouse)
        if (e.type == EventType.MouseDown && e.button == 4)
        {
            GameObject[] selected = Selection.gameObjects;
        if (selected == null || selected.Length == 0) return;

        Undo.RecordObjects(
            selected.Select(go => go.GetComponent<cubeconfig>())
                    .Where(c => c != null)
                    .ToArray(),
            "Toggle Lunk (Multi)"
        );

        foreach (var go in selected)
        {
            var cube = go.GetComponent<cubeconfig>();
            if (cube == null) continue;

            cube.isLunk = !cube.isLunk;
            cube.ApplyConfiguration();
            EditorUtility.SetDirty(cube);
        }
            e.Use();
        }
    }
}

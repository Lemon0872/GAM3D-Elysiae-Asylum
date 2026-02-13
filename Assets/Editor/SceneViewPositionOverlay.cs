#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class SceneViewPositionOverlay
{
    static GUIStyle style;

    static SceneViewPositionOverlay()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;
        style.fontStyle = FontStyle.Bold;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Vector3 pos = sceneView.camera.transform.position;

        Handles.BeginGUI();

        GUI.Box(new Rect(10, 10, 260, 40), "");
        GUI.Label(
            new Rect(20, 18, 250, 30),
            $"Scene Cam Pos:\nX:{pos.x:F2}  Y:{pos.y:F2}  Z:{pos.z:F2}",
            style
        );

        Handles.EndGUI();
    }
}
#endif

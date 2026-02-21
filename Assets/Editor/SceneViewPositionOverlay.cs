#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using System.Globalization;

[InitializeOnLoad]
public static class SceneViewCameraTool
{
    static Vector3 lastPosition;
    static Quaternion lastRotation;

    static SceneViewCameraTool()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    // ==============================
    // Overlay hiển thị
    // ==============================
    static void OnSceneGUI(SceneView sceneView)
    {
        if (sceneView.camera == null)
            return;

        lastPosition = sceneView.camera.transform.position;
        lastRotation = sceneView.camera.transform.rotation;

        Handles.BeginGUI();

        GUILayout.BeginArea(new Rect(10, 10, 300, 60), GUI.skin.window);
        GUILayout.Label($"Pos: {lastPosition:F2}");
        GUILayout.Label($"Rot: {lastRotation.eulerAngles:F2}");
        GUILayout.EndArea();

        Handles.EndGUI();
    }

    // ==============================
    // Global Shortcut
    // ==============================
    [Shortcut("Tools/Copy SceneView Camera", KeyCode.RightBracket, ShortcutModifiers.Control)]
    static void CopySceneCamera()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;

        if (sceneView == null || sceneView.camera == null)
            return;

        Vector3 pos = sceneView.camera.transform.position;

        // Format chuẩn Inspector
        EditorGUIUtility.systemCopyBuffer =
    string.Format(
        CultureInfo.InvariantCulture,
        "Vector3({0},{1},{2})",
        pos.x, pos.y, pos.z
    );
        Debug.Log("Copied for Inspector paste.");
    }
}
#endif
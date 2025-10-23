using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneEditorWindow : EditorWindow
{
    private Vector2 scroll;

    [MenuItem("Tools/Scenes Switcher &#S")]
    public static void ShowWindow()
    {
        SceneEditorWindow window = GetWindow<SceneEditorWindow>();
        window.titleContent = new GUIContent("Scenes List");
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Scenes List", EditorStyles.boldLabel);

        var scenesList = EditorBuildSettings.scenes;

        if (scenesList.Length == 0)
        {
            EditorGUILayout.HelpBox("No scenes found in the build settings.", MessageType.Info);

            if (GUILayout.Button("Open Build Settings"))
            {
                ShowBuildSettings();
            }

            return;
        }

        List<EditorBuildSettingsScene> activeScenesList = new();

        foreach (var scene in scenesList)
        {
            if (!scene.enabled)
            {
                continue;
            }

            activeScenesList.Add(scene);
        }

        if (activeScenesList.Count == 0)
        {
            EditorGUILayout.HelpBox("No active scenes found in the build settings.", MessageType.Info);

            if (GUILayout.Button("Open Build Settings"))
            {
                ShowBuildSettings();
            }
            return;
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var scene in activeScenesList)
        {
            string scenePath = scene.path;
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            bool isCurrent = scenePath == EditorSceneManager.GetActiveScene().path;

            GUI.enabled = !isCurrent;

            if (GUILayout.Button((isCurrent ? "* " : "") + sceneName, GUILayout.Height(24)))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(scenePath);
                }
            }

            GUI.enabled = true;
        }

        EditorGUILayout.EndScrollView();
    }

    private void ShowBuildSettings()
    {
        EditorWindow.GetWindow(typeof(BuildPlayerWindow));
    }
}
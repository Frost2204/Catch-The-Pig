using System.IO;
using UnityEditor;
using UnityEngine;

namespace MVC_Creator_Tool
{
    public class MVCCreatorWindow : EditorWindow
    {
        private const string scriptsPathKey = "MVCCreator_ScriptsPath";
        private const string pendingSystemKey = "MVCCreator_PendingSystem";
        private string systemName = "";
        private string scriptsBasePath = "Assets/Scripts";
        private bool focusedTextField = false;

        [MenuItem("Tools/Create MVC System %#m")] // Ctrl/Cmd + Shift + M
        public static void ShowWindow()
        {
            var window = GetWindow<MVCCreatorWindow>(true, "Create MVC System");
            window.minSize = new Vector2(300, 200);
            window.maxSize = new Vector2(300, 200);
            window.ShowUtility();
        }

        private void OnGUI()
        {
            if (EditorPrefs.HasKey(scriptsPathKey))
            {
                scriptsBasePath = EditorPrefs.GetString(scriptsPathKey);
            }

            GUILayout.Label("Enter System Name:", EditorStyles.boldLabel);

            GUI.SetNextControlName("SystemNameField");
            systemName = EditorGUILayout.TextField(systemName);

            GUILayout.Label("Scripts Base Folder Path:", EditorStyles.label);
            EditorGUILayout.BeginHorizontal();
            scriptsBasePath = EditorGUILayout.TextField(scriptsBasePath);

            if (GUILayout.Button("Select", GUILayout.MaxWidth(60)))
            {
                string selected = EditorUtility.OpenFolderPanel("Select Scripts Folder", Application.dataPath, "");
                if (!string.IsNullOrEmpty(selected))
                {
                    if (selected.StartsWith(Application.dataPath))
                    {
                        scriptsBasePath = "Assets" + selected.Substring(Application.dataPath.Length);
                        EditorPrefs.SetString(scriptsPathKey, scriptsBasePath);

                    }
                    else
                    {
                        Debug.LogWarning("Selected path must be within the Assets folder.");
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            if (!focusedTextField)
            {
                EditorGUI.FocusTextInControl("SystemNameField");
                focusedTextField = true;
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Create System") && !string.IsNullOrWhiteSpace(systemName))
            {
                CreateMVCScripts(systemName);
                Close();
            }

            // Handle Enter key
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                if (!string.IsNullOrWhiteSpace(systemName))
                {
                    CreateMVCScripts(systemName);
                    Close();
                }
                Event.current.Use();
            }
        }

        private void CreateMVCScripts(string name)
        {
            string modelPath = Path.Combine(scriptsBasePath, "Model");
            string viewPath = Path.Combine(scriptsBasePath, "View");
            string controllerPath = Path.Combine(scriptsBasePath, "Controller");

            Directory.CreateDirectory(modelPath);
            Directory.CreateDirectory(viewPath);
            Directory.CreateDirectory(controllerPath);

            string rootNamespace = EditorSettings.projectGenerationRootNamespace;

            CreateScript(Path.Combine(modelPath, $"{name}Model.cs"), $"{name}Model", "Model", rootNamespace);
            CreateScript(Path.Combine(viewPath, $"{name}View.cs"), $"{name}View", "View", rootNamespace);
            CreateScript(Path.Combine(controllerPath, $"{name}Controller.cs"), $"{name}Controller", "Controller", rootNamespace);

            EditorPrefs.SetString(pendingSystemKey, name);
            AssetDatabase.Refresh();
        }

        private void CreateScript(string path, string className, string type, string ns)
        {
            if (File.Exists(path))
            {
                Debug.LogWarning($"{className}.cs already exists.");
                return;
            }

            string templatePath = $"Assets/Tools/MVC_Generator/ScriptTemplates/{type}Template.txt";

            if (!File.Exists(templatePath))
            {
                Debug.LogError($"Template not found: {templatePath}");
                return;
            }

            string template = File.ReadAllText(templatePath);
            string content = template
                .Replace("#SCRIPTNAME#", className)
                .Replace("#NAMESPACE#", string.IsNullOrWhiteSpace(ns) ? "DefaultNamespace" : ns);

            File.WriteAllText(path, content);
        }
    }
}
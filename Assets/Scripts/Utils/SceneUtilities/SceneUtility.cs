using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class SceneUtility
    {
        private const string enumPath = "Assets/Scripts/Utils/SceneUtilities/SceneData.cs";

        private static Dictionary<int, string> SceneDataDict;

        #region Helper Functions
        private static string GetStringInPascal(string text)
        {
            string result = "";
            string[] wordsArray = text.Split(' ');

            foreach (var word in wordsArray)
            {
                if (word.Length > 1)
                {
                    char firstLetter = Char.ToUpper(word[0]);
                    result += firstLetter + word[1..];
                }
                else
                {
                    result += word.ToUpper();
                }
            }

            return result;
        }
        #endregion

        #region Functions
#if UNITY_EDITOR
        static SceneUtility()
        {
            EditorBuildSettings.sceneListChanged += UpdateScenes;

            UpdateScenes();
        }

        /// <summary>
        /// Editor function that updates the scene enum
        /// </summary>
        [MenuItem("Scene Utility/Update Scenes Enum")]
        private static void UpdateScenes()
        {
            SceneDataDict = new Dictionary<int, string>();

            // Writing Enum
            StringBuilder enumBuilder = new();
            enumBuilder.AppendLine("using System.Collections.Generic;");
            enumBuilder.AppendLine("public enum Scene");
            enumBuilder.AppendLine("{");

            int index = 0;

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    string sceneName = Path.GetFileNameWithoutExtension(scene.path);
                    string validSceneName = GetStringInPascal(sceneName); // Ensure valid enum names
                    enumBuilder.AppendLine($"    {validSceneName},");

                    SceneDataDict.Add(index, sceneName);
                    index++;
                }
            }

            enumBuilder.AppendLine("}");

            // Writing Dictionary
            enumBuilder.AppendLine("public class SceneData");
            enumBuilder.AppendLine("{");
            enumBuilder.AppendLine("    public static Dictionary<int, string> SceneDataDict = new()");
            enumBuilder.AppendLine("    {");

            foreach (var scene in SceneDataDict)
            {
                enumBuilder.AppendLine($"    {{ {scene.Key}, \"{scene.Value}\"}},");
            }

            enumBuilder.AppendLine("    };");
            enumBuilder.AppendLine("}");

            File.WriteAllText(enumPath, enumBuilder.ToString());
            AssetDatabase.Refresh();
        }
#endif
        #endregion

        #region Extension Methods
        public static void LoadScene(this Scene scene)
        {
            SceneManager.LoadScene((int)scene);
        }

        public static AsyncOperation LoadSceneAsync(this Scene scene)
        {
            return SceneManager.LoadSceneAsync((int)scene);
        }

        public static int GetBuildIndex(this Scene scene)
        {
            return (int)scene;
        }

        public static string GetSceneName(this Scene scene)
        {
            return SceneData.SceneDataDict[(int)scene];
        }
        #endregion
    }
}

using UnityEditor;

namespace MVC_Creator_Tool
{
    [InitializeOnLoad]
    public static class MVCPrefabPrompt
    {
        private const string PendingSystemKey = "MVCCreator_PendingSystem";

        static MVCPrefabPrompt()
        {
            EditorApplication.update += CheckForPendingPrefab;
        }

        private static void CheckForPendingPrefab()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
                return;

            if (!EditorPrefs.HasKey(PendingSystemKey))
                return;

            string systemName = EditorPrefs.GetString(PendingSystemKey);
            if (string.IsNullOrEmpty(systemName))
                return;

            EditorPrefs.DeleteKey(PendingSystemKey);

            bool createPrefab = EditorUtility.DisplayDialog(
                "Create Prefab?",
                $"Scripts for '{systemName}' were created.\nDo you want to create a prefab now?",
                "Yes", "No");

            if (createPrefab)
            {
                MVCPrefabCreator.CreatePrefab(systemName);
            }
        }
    }
}

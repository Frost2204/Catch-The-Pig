using System.IO;
using UnityEditor;
using UnityEngine;

namespace MVC_Creator_Tool
{
    public static class MVCPrefabCreator
    {
        public static void CreatePrefab(string systemName)
        {
            string prefabDir = "Assets/Prefabs/Systems";
            Directory.CreateDirectory(prefabDir);

            GameObject parent = new GameObject(systemName + "System");

            GameObject viewGO = CreateChildWithComponent(systemName + "View", parent.transform, systemName + "View");
            GameObject controllerGO = CreateChildWithComponent(systemName + "Controller", parent.transform, systemName + "Controller");

            if (controllerGO != null && viewGO != null)
            {
                AssignViewReference(controllerGO, viewGO, $"{systemName}View");
            }

            string prefabPath = Path.Combine(prefabDir, $"{systemName}System.prefab");
            PrefabUtility.SaveAsPrefabAsset(parent, prefabPath);
            GameObject.DestroyImmediate(parent);

            AssetDatabase.Refresh();

            // Select and ping the prefab
            Object prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            Selection.activeObject = prefabAsset;
            EditorGUIUtility.PingObject(prefabAsset);
        }

        private static void AssignViewReference(GameObject controllerGO, GameObject viewGO, string viewTypeName)
        {
            Component controllerComp = controllerGO.GetComponent(GetTypeFromName(controllerGO.name));
            if (controllerComp == null) return;

            SerializedObject serializedObject = new SerializedObject(controllerComp);
            SerializedProperty iterator = serializedObject.GetIterator();

            while (iterator.NextVisible(true))
            {
                if (iterator.propertyType == SerializedPropertyType.ObjectReference &&
                    iterator.name.ToLower().Contains("view"))
                {
                    System.Type viewType = GetTypeFromName(viewTypeName);
                    if (viewType != null && viewGO.GetComponent(viewType) != null)
                    {
                        iterator.objectReferenceValue = viewGO.GetComponent(viewType);
                        serializedObject.ApplyModifiedProperties();
                        break;
                    }
                }
            }
        }

        private static GameObject CreateChildWithComponent(string objName, Transform parent, string className)
        {
            GameObject child = new GameObject(objName);
            child.transform.parent = parent;

            System.Type scriptType = GetTypeFromName(className);
            if (scriptType != null)
            {
                child.AddComponent(scriptType);
            }
            else
            {
                Debug.LogWarning($"Script '{className}' not found. Prefab will be created without that component.");
            }

            return child;
        }

        private static System.Type GetTypeFromName(string className)
        {
            foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes())
                {
                    if (type.Name == className && typeof(MonoBehaviour).IsAssignableFrom(type))
                    {
                        return type;
                    }
                }
            }
            return null;
        }
    }
}
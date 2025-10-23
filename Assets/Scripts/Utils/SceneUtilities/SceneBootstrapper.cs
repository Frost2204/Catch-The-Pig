#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Utils
{
    [InitializeOnLoad]
    public class SceneBootstrapper
    {
        private const string PREVIOUS_SCENE = "PreviousScene";
        private const string SHOULD_LOAD_BOOSTRAP = "LoadBootStrapScene";

        private const string LOAD_BOOTSTRAP_MENU = "Scene Utility/Load Bootstrap Scene on play";
        private const string DONT_LOAD_BOOTSTRAP_MENU = "Scene Utility/Don't Load Bootstrap Scene on play";

        private static string BOOTSTRAP_SCENE
        {
            get
            {
                return EditorBuildSettings.scenes[0].path;
            }
        }

        private static string PreviousScene
        {
            get => EditorPrefs.GetString(PREVIOUS_SCENE);
            set => EditorPrefs.SetString(PREVIOUS_SCENE, value);
        }

        private static bool ShouldLoadBootstrapScene
        {
            get => EditorPrefs.GetBool(SHOULD_LOAD_BOOSTRAP, true);
            set => EditorPrefs.SetBool(SHOULD_LOAD_BOOSTRAP, value);
        }

        static SceneBootstrapper()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            if (!ShouldLoadBootstrapScene)
            {
                return;
            }

            switch (playModeStateChange)
            {
                case PlayModeStateChange.EnteredEditMode:

                    if (!string.IsNullOrEmpty(PreviousScene))
                    {
                        EditorSceneManager.OpenScene(PreviousScene);
                    }
                    break;

                case PlayModeStateChange.ExitingEditMode:
                    PreviousScene = EditorSceneManager.GetActiveScene().path;

                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() && IsSceneInBuildSettings(BOOTSTRAP_SCENE))
                    {
                        EditorSceneManager.OpenScene(BOOTSTRAP_SCENE);
                    }
                    break;
            }
        }
        private static bool IsSceneInBuildSettings(string scenePath)
        {
            if (string.IsNullOrEmpty(scenePath))
                return false;

            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.path == scenePath)
                {
                    return true;
                }
            }

            return false;
        }

        [MenuItem(LOAD_BOOTSTRAP_MENU)]
        private static void EnableBootstrapper()
        {
            ShouldLoadBootstrapScene = true;
        }

        [MenuItem(LOAD_BOOTSTRAP_MENU, true)]
        private static bool ValidateEnableBootstrapper()
        {
            return !ShouldLoadBootstrapScene;
        }

        [MenuItem(DONT_LOAD_BOOTSTRAP_MENU)]
        private static void DisableBootstrapper()
        {
            ShouldLoadBootstrapScene = false;
        }

        [MenuItem(DONT_LOAD_BOOTSTRAP_MENU, true)]
        private static bool ValidateDisableBootstrapper()
        {
            return ShouldLoadBootstrapScene;
        }
    }

}
#endif
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Common
{
    public class LoadingManager : SingletonUtility<LoadingManager>
    {
        #region Inspector Variables
        [SerializeField] private Canvas loadingCanvas;
        [SerializeField] private Image loadingBarFill;
        #endregion Inspector Variables

        #region Public Variables
        #endregion Public Variables

        #region Private Variables
        #endregion Private Variables

        #region Monobehaviour Methods
        public override void Awake()
        {
            base.Awake();

            loadingCanvas.gameObject.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
        #endregion Monobehaviour Methods

        #region Private Methods
        private IEnumerator LoadSceneCoroutine(Scene scene)
        {
            AsyncOperation op = scene.LoadSceneAsync();

            loadingBarFill.fillAmount = 0f;
            loadingCanvas.gameObject.SetActive(true);

            while (!op.isDone)
            {
                loadingBarFill.fillAmount = op.progress / 0.9f;
                yield return null;
            }

            loadingCanvas.gameObject.SetActive(false);
        }
        #endregion Private Methods

        #region Public Methods
        internal void LoadScene(Scene scene)
        {
            StartCoroutine(LoadSceneCoroutine(scene));
        }
        #endregion Public Methods
    }
}

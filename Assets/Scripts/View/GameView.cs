using Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace.View
{
    public class GameView : EntityView
    {
        #region Inspector Variables
        [SerializeField] private Button menuButton;
        #endregion Inspector Variables

        #region Public Variables
        #endregion Public Variables

        #region Private Variables
        #endregion Private Variables

        #region Private Methods
        #endregion Private Methods

        #region Public Methods
        public override void OnGameStart()
        {
            menuButton.onClick.AddListener(menuButtonOnClikc);
        }
        private void menuButtonOnClikc()
        {
            SceneManager.LoadScene(0);
        }
        public override void OnGameOver()
        {

        }
        #endregion Public Methods
    }
}

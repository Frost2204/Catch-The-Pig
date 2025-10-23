using Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField] private Button PlayButton;
    #endregion Inspector Variables

    #region Public Variables
    #endregion Public Variables

    #region Private Variables
    #endregion Private Variables

    #region Monobehaviour Methods
    private void Start()
    {
        PlayButton.onClick.AddListener(onplayClick);
    }
    #endregion Monobehaviour Methods

    #region Private Methods
    private void onplayClick()
    {
        SceneManager.LoadScene(1);
    }
    #endregion Private Methods

    #region Public Methods
    #endregion Public Methods
}

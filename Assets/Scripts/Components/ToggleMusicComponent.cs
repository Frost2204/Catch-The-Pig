using UnityEngine;
using UnityEngine.UI;

namespace AudioManager
{
    public class ToggleMusicComponent : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private Button toggleButton;
        [SerializeField] private Image toggleIconImage;
        [SerializeField] private Sprite musicOnIcon;
        [SerializeField] private Sprite musicOffIcon;
        #endregion

        #region Monobehaviour Methods
        private void OnEnable()
        {
            toggleButton.onClick.AddListener(OnToggleClick);

            if (AudioManager.Instance.GetMusicState())
            {
                toggleIconImage.sprite = musicOnIcon;
            }
            else
            {
                toggleIconImage.sprite = musicOffIcon;
            }
        }

        private void OnDisable()
        {
            toggleButton.onClick.RemoveListener(OnToggleClick);
        }
        #endregion

        #region Private Methods
        private void OnToggleClick()
        {
            AudioManager.Instance.PlaySFX(SFX_Type.BUTTON_CLICK);

            bool canPlayMusic = !AudioManager.Instance.GetMusicState();

            AudioManager.Instance.SetMusicState(canPlayMusic);

            if (canPlayMusic)
            {
                toggleIconImage.sprite = musicOnIcon;
            }
            else
            {
                toggleIconImage.sprite = musicOffIcon;
            }
        }
        #endregion
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace AudioManager
{
    public class ToggleSFXComponent : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private Button toggleButton;
        [SerializeField] private Image toggleIconImage;
        [SerializeField] private Sprite sfxOnIcon;
        [SerializeField] private Sprite sfxOffIcon;
        #endregion

        #region Monobehaviour Methods
        private void OnEnable()
        {
            toggleButton.onClick.AddListener(OnToggleClick);

            if (AudioManager.Instance.GetSFXState())
            {
                toggleIconImage.sprite = sfxOnIcon;
            }
            else
            {
                toggleIconImage.sprite = sfxOffIcon;
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
            bool canPlaySfx = !AudioManager.Instance.GetSFXState();

            AudioManager.Instance.PlaySFX(SFX_Type.BUTTON_CLICK);

            AudioManager.Instance.SetSFXState(canPlaySfx);

            if (canPlaySfx)
            {
                toggleIconImage.sprite = sfxOnIcon;
            }
            else
            {
                toggleIconImage.sprite = sfxOffIcon;
            }
        }
        #endregion
    }
}

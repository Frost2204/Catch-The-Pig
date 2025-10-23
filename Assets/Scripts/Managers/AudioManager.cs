using System;
using UnityEngine;
using Utils;

namespace AudioManager
{
    public class AudioManager : SingletonUtility<AudioManager>
    {
        #region Inspector Variables
        [Header("Audio Source")]
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource sfxAudioSource;
        [Header("Audio Clips")]
        [SerializeField] private AudioClip musicClip;
        [SerializeField] private AudioClip buttonClickClip;
        // Add other audio clips here
        #endregion

        #region Private Variables
        private bool canPlayMusic;
        private bool canPlaySFX;
        #endregion

        #region Monobehaviour Methods
        public override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            canPlaySFX = PlayerPrefs.GetInt(AudioModel.SFX_STATE, 1) == 1;
            canPlayMusic = PlayerPrefs.GetInt(AudioModel.MUSIC_STATE, 1) == 1;

            SetMusicState(canPlayMusic);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Plays the specified SFX type clip
        /// </summary>
        /// <param name="type">The type of SFX</param>
        /// <param name="volume">The volume at which you want to play SFX</param>
        public void PlaySFX(SFX_Type type, float volume = 1f)
        {
            try
            {
                if (!canPlaySFX)
                {
                    return;
                }

                switch (type)
                {
                    case SFX_Type.BUTTON_CLICK:
                        sfxAudioSource.PlayOneShot(buttonClickClip, volume);
                        break;

                        // add other sfx types here
                }
            }
            catch (Exception e)
            {
                Debug.Log("[AudioManager/PlaySFX] Invalid Request: " + e.Message);
            }
        }

        /// <summary>
        /// Sets the state of SFX
        /// </summary>
        /// <param name="state">the state you want to set</param>
        public void SetSFXState(bool state)
        {
            sfxAudioSource.Stop();

            canPlaySFX = state;

            PlayerPrefs.SetInt(AudioModel.SFX_STATE, state ? 1 : 0);
        }

        /// <summary>
        /// Sets the background music state
        /// </summary>
        /// <param name="state">the state you want to set</param>
        public void SetMusicState(bool state)
        {
            try
            {
                canPlayMusic = state;
                PlayerPrefs.SetInt(AudioModel.MUSIC_STATE, state ? 1 : 0);

                if (canPlayMusic)
                {
                    if (musicAudioSource.clip == null)
                    {
                        musicAudioSource.clip = musicClip;
                        musicAudioSource.loop = true;
                        musicAudioSource.Play();
                    }
                    else
                    {
                        musicAudioSource.Play();
                    }
                }
                else
                {
                    if (musicAudioSource.isPlaying)
                    {
                        musicAudioSource.Pause();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[AudioManager/SetMusicState] Invalid Request: " + e.Message);
            }
        }

        /// <summary>
        /// Get the music state of the game
        /// </summary>
        /// <returns>the state of music</returns>
        public bool GetMusicState()
        {
            return canPlayMusic;
        }

        /// <summary>
        /// Get the SFX state of the game
        /// </summary>
        /// <returns>the state of SFX</returns>
        public bool GetSFXState()
        {
            return canPlaySFX;
        }
        #endregion
    }
}

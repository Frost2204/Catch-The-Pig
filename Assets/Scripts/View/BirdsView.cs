using Common;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using AudioManager;

namespace DefaultNamespace.View
{
    public class BirdsView : EntityView
    {
        [Header("References")]
        [SerializeField] private BirdComponent bird;
        [SerializeField] private PigComponent pig;
        [SerializeField] private SpriteRenderer pigSprite; // ✅ Now assigned in Inspector
        [SerializeField] private Slider distanceSlider;    // Shows progress
        [SerializeField] private GameObject winCanvas;     // Shown when completed

        [Header("Gameplay Settings")]
        [SerializeField] private int totalSteps = 6;
        [SerializeField] private float moveStep = 1f;
        [SerializeField] private float clickTimeWindow = 1f;
        [SerializeField] private int clicksRequired = 3;

        [Header("Auto Retreat")]
        [SerializeField] private float autoRetreatInterval = 5f;

        [Header("UI Buttons")]
        [SerializeField] private Button restartLevelButton;
        [SerializeField] private Button menuButton;

        private int clickCounter = 0;
        private float clickTimer = 0f;
        private float retreatTimer = 0f;

        private float initialDistance;
        private int currentStep = 0;
        private bool gameEnded = false;

        public override void OnGameStart()
        {
            // Unsubscribe old listeners first
            if (restartLevelButton != null)
                restartLevelButton.onClick.RemoveAllListeners();
            if (menuButton != null)
                menuButton.onClick.RemoveAllListeners();

            // Add button listeners
            if (restartLevelButton != null)
                restartLevelButton.onClick.AddListener(OnRestartLevelClicked);
            if (menuButton != null)
                menuButton.onClick.AddListener(OnMenuClicked);

            // Reset state
            clickCounter = 0;
            clickTimer = 0f;
            retreatTimer = 0f;
            currentStep = 0;
            gameEnded = false;
            Time.timeScale = 1f;

            if (bird != null && pig != null)
                initialDistance = Mathf.Abs(bird.transform.position.x - pig.transform.position.x);

            if (distanceSlider != null)
            {
                distanceSlider.value = 0f;
            }

            if (winCanvas != null)
                winCanvas.SetActive(false);

            bird?.ResumeAnimation();
            pig?.ResumeAnimation();

            // ✅ Reset pig color to white
            if (pigSprite != null)
                pigSprite.color = Color.white;
        }

        private void OnRestartLevelClicked()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(1);
        }

        private void OnMenuClicked()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        public override void OnGameOver()
        {
            bird.StopMovement();
            pig.StopMovement();
        }

        public void HandleInput()
        {
            if (gameEnded) return;

            // Rapid clicks
            if (Input.GetMouseButtonDown(0))
            {
                clickCounter++;
                clickTimer = 0f;
            }

            if (clickCounter > 0)
            {
                clickTimer += Time.deltaTime;
                if (clickTimer > clickTimeWindow)
                {
                    clickCounter = 0;
                    clickTimer = 0f;
                }
            }

            if (clickCounter >= clicksRequired)
            {
                bird.OnRapidClick(moveStep);
                pig.OnRapidClick(moveStep);
                clickCounter = 0;
                clickTimer = 0f;

                currentStep++;
                CheckForWin();
            }

            // Auto retreat every few seconds
            retreatTimer += Time.deltaTime;
            if (retreatTimer >= autoRetreatInterval)
            {
                bird.OnRapidClick(-moveStep);
                pig.OnRapidClick(-moveStep);
                retreatTimer = 0f;

                currentStep = Mathf.Max(0, currentStep - 1);
            }

            UpdateDistanceSlider();
        }

        private void UpdateDistanceSlider()
        {
            if (bird == null || pig == null || distanceSlider == null) return;

            float currentDistance = Mathf.Abs(bird.transform.position.x - pig.transform.position.x);
            float normalized = Mathf.Clamp01(1f - (currentDistance / initialDistance));
            distanceSlider.value = normalized;
        }

        private void CheckForWin()
        {
            if (currentStep >= totalSteps)
            {
                gameEnded = true;
                StartCoroutine(HandleWinSequence());
            }
        }

        private IEnumerator HandleWinSequence()
        {
            // Pause animations
            bird.PauseAnimation();
            pig.PauseAnimation();
            AudioManager.AudioManager.Instance.PlaySFX(SFX_Type.PIG_CRY, 0.3f);
            // ✅ Fast flick (red ↔ white)
            if (pigSprite != null)
            {
                Color original = pigSprite.color;
                for (int i = 0; i < 10; i++) // flick 4 times quickly
                {
                    pigSprite.color = (i % 2 == 0) ? Color.red : original;
                    yield return new WaitForSecondsRealtime(0.2f); // fast flick (0.1 sec per change)
                }
                pigSprite.color = original;
            }

            // ✅ Show win screen and pause
            if (winCanvas != null)
                winCanvas.SetActive(true);

            AudioManager.AudioManager.Instance.PlaySFX(SFX_Type.WON, 0.3f);

            Time.timeScale = 0f;
        }

    }
}

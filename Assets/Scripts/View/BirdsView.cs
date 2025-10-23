using Common;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.View
{
    public class BirdsView : EntityView
    {
        [Header("References")]
        [SerializeField] private BirdComponent bird;
        [SerializeField] private PigComponent pig;
        [SerializeField] private Slider distanceSlider; // Shows progress
        [SerializeField] private GameObject winCanvas;  // Shown when completed

        [Header("Gameplay Settings")]
        [SerializeField] private int totalSteps = 6; // Example: 6 steps max
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
            // Unsubscribe before re-adding listeners (prevents multiple bindings)
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

            Time.timeScale = 1f; // Ensure gameplay runs

            if (bird != null && pig != null)
                initialDistance = Mathf.Abs(bird.transform.position.x - pig.transform.position.x);

            if (distanceSlider != null)
            {
                distanceSlider.minValue = 0f;
                distanceSlider.maxValue = 1f;
                distanceSlider.value = 0f;
            }

            if (winCanvas != null)
                winCanvas.SetActive(false);
        }

        public override void OnGameOver()
        {
            bird.StopMovement();
            pig.StopMovement();
        }

        private void OnRestartLevelClicked()
        {
            Time.timeScale = 1f; // Unpause before loading
            SceneManager.LoadScene(1); // Reload main level scene
        }

        private void OnMenuClicked()
        {
            Time.timeScale = 1f; // Unpause before loading
            SceneManager.LoadScene(0); // Load menu scene
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

            // Auto retreat every interval
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
                OnWin();
            }
        }

        private void OnWin()
        {
            // Show win screen and pause the game
            if (winCanvas != null)
                winCanvas.SetActive(true);

            Time.timeScale = 0f;
        }
    }
}

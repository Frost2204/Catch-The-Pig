using Common;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.View
{
    public class BirdsView : EntityView
    {
        [Header("References")]
        [SerializeField] private BirdComponent bird;
        [SerializeField] private PigComponent pig;
        [SerializeField] private Slider distanceSlider; // assign via Inspector

        [Header("Click Settings")]
        [SerializeField] private float moveStep = 1f;
        [SerializeField] private float clickTimeWindow = 1f;
        [SerializeField] private int clicksRequired = 3;

        [Header("Auto Retreat")]
        [SerializeField] private float autoRetreatInterval = 5f;

        private int clickCounter = 0;
        private float clickTimer = 0f;
        private float retreatTimer = 0f;

        // Store initial distance for normalization
        private float initialDistance;

        public override void OnGameStart()
        {
            clickCounter = 0;
            clickTimer = 0f;
            retreatTimer = 0f;

            if (bird != null && pig != null)
                initialDistance = Mathf.Abs(bird.transform.position.x - pig.transform.position.x);

            if (distanceSlider != null)
            {
                distanceSlider.minValue = 0f;
                distanceSlider.maxValue = 1f;
                distanceSlider.value = 0f;
            }
        }

        public override void OnGameOver()
        {
            bird.StopMovement();
            pig.StopMovement();
        }

        public void HandleInput()
        {
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
            }

            // Auto retreat every interval
            retreatTimer += Time.deltaTime;
            if (retreatTimer >= autoRetreatInterval)
            {
                bird.OnRapidClick(-moveStep);
                pig.OnRapidClick(-moveStep);
                retreatTimer = 0f;
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
    }
}

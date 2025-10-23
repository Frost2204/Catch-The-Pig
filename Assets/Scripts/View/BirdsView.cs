using Common;
using UnityEngine;

namespace DefaultNamespace.View
{
    public class BirdsView : EntityView
    {
        [Header("References")]
        [SerializeField] private BirdComponent bird;
        [SerializeField] private PigComponent pig;

        [Header("Click Settings")]
        [SerializeField] private float moveStep = 1f;
        [SerializeField] private float clickTimeWindow = 1f;
        [SerializeField] private int clicksRequired = 3;

        [Header("Auto Retreat")]
        [SerializeField] private float autoRetreatInterval = 5f;

        private int clickCounter = 0;
        private float clickTimer = 0f;
        private float retreatTimer = 0f;

        public BirdComponent GetBird() => bird;
        public PigComponent GetPig() => pig;

        public override void OnGameStart()
        {
            clickCounter = 0;
            clickTimer = 0f;
            retreatTimer = 0f;
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
        }
    }
}

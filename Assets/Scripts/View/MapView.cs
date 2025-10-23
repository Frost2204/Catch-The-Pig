using Common;
using UnityEngine;

namespace DefaultNamespace.View
{
    public class MapView : EntityView
    {
        #region Inspector Variables
        [Header("Front Layer")]
        [SerializeField] private Transform frontLayer1;
        [SerializeField] private Transform frontLayer2;
        [SerializeField] private float frontSpeed = 5f;
        [SerializeField] private float frontResetOffset = 30f;

        [Header("Middle Layer")]
        [SerializeField] private Transform middleLayer1;
        [SerializeField] private Transform middleLayer2;
        [SerializeField] private float middleSpeed = 3f;
        [SerializeField] private float middleResetOffset = 40f;

        [Header("Back Layer")]
        [SerializeField] private Transform backLayer1;
        [SerializeField] private Transform backLayer2;
        [SerializeField] private float backSpeed = 1.5f;
        [SerializeField] private float backResetOffset = 50f;

        [Header("Common Settings")]
        [SerializeField] private bool moveRight = false;
        [SerializeField] private Transform spawnLocation;
        #endregion

        #region Private Variables
        private bool isPaused = true;
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (isPaused) return;

            MoveLayer(frontLayer1, frontLayer2, frontSpeed, frontResetOffset);
            MoveLayer(middleLayer1, middleLayer2, middleSpeed, middleResetOffset);
            MoveLayer(backLayer1, backLayer2, backSpeed, backResetOffset);
        }
        #endregion

        #region Private Methods
        private void MoveLayer(Transform layer1, Transform layer2, float speed, float resetOffset)
        {
            float direction = moveRight ? 1f : -1f;
            float moveStep = speed * direction * Time.deltaTime;

            layer1.Translate(Vector3.right * moveStep);
            layer2.Translate(Vector3.right * moveStep);

            if (moveRight)
            {
                if (layer1.position.x > resetOffset)
                    ResetLayer(layer1, layer2, resetOffset);
                else if (layer2.position.x > resetOffset)
                    ResetLayer(layer2, layer1, resetOffset);
            }
            else
            {
                if (layer1.position.x < -resetOffset)
                    ResetLayer(layer1, layer2, resetOffset);
                else if (layer2.position.x < -resetOffset)
                    ResetLayer(layer2, layer1, resetOffset);
            }
        }

        private void ResetLayer(Transform layerToMove, Transform referenceLayer, float resetOffset)
        {
            // float width = resetOffset * 2f; // Adjust if sprite width is different
            Vector3 newPos = spawnLocation.position;
            layerToMove.position = newPos;
        }
        #endregion

        #region Public Methods
        public override void OnGameStart()
        {
            Resume();
        }

        public override void OnGameOver()
        {
            Pause();
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }
        #endregion
    }
}

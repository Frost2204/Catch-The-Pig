using Common;
using UnityEngine;

namespace DefaultNamespace.Controller
{
    public class GameController : EntityController
    {
        #region Inspector Variables
        [SerializeField] private BirdsController birdsController;
        [SerializeField] private MapController mapController;
        #endregion Inspector Variables

        #region Public Variables
        #endregion Public Variables

        #region Private Variables
        #endregion Private Variables

        #region Monobehaviour Methods
        #endregion Monobehaviour Methods

        #region Private Methods
        private void Start()
        {
            OnGameStart();
        }
        #endregion Private Methods

        #region Public Methods
        public override void OnGameStart()
        {
            view.OnGameStart();
            view.SetController(this);

            birdsController.OnGameStart();
            birdsController.SetHubController(this);

            mapController.OnGameStart();
            mapController.SetHubController(this);
        }

        public override void OnGameOver() { }
        #endregion Public Methods
    }
}

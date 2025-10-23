using Common;
using DefaultNamespace.View;
using UnityEngine;

namespace DefaultNamespace.Controller
{
    public class BirdsController : EntityController
    {
        #region Inspector Variables
        #endregion Inspector Variables

        #region Public Variables
        #endregion Public Variables

        #region Private Variables
        #endregion Private Variables

        #region Monobehaviour Methods
        #endregion Monobehaviour Methods

        #region Private Methods
        #endregion Private Methods

        #region Public Methods
        public override void OnGameStart()
        {
            GetView<BirdsView>().OnGameStart();
        }

        public override void OnGameOver()
        {
            
        }
        #endregion Public Methods
    }
}

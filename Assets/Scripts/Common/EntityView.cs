using System;
using UnityEngine;

namespace Common
{
    public abstract class EntityView : MonoBehaviour
    {
        #region --------------------------------- Protected Fields -------------------------------------
        protected EntityController controller = null;
        #endregion -------------------------------------------------------------------------------------


        #region --------------------------------- Public Fields ----------------------------------------

        #endregion -------------------------------------------------------------------------------------


        #region --------------------------------- Public Methods ----------------------------------------
        public void SetController<T>(T controller) where T : EntityController
        {
            this.controller = (T)controller;
        }

        protected T GetController<T>() where T : EntityController
        {
            return (T)Convert.ChangeType(this.controller, typeof(T));
        }

        public abstract void OnGameStart();
        public abstract void OnGameOver();
        #endregion -------------------------------------------------------------------------------------
    }
}


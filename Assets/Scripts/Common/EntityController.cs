using System;
using UnityEngine;

namespace Common
{
    public abstract class EntityController : MonoBehaviour
    {
        #region --------------------------------- Protected Fields -------------------------------------
        [SerializeField] protected EntityView view;
        private object gameController;
        protected object data;
        #endregion --------------------------------------------------------------------------------------


        #region --------------------------------- Public Methods ----------------------------------------
        public abstract void OnGameStart();
        public abstract void OnGameOver();

        public void SetHubController<T>(T gameController)
        {
            this.gameController = gameController;
        }

        public void LoadData<T>(T data)
        {
            this.data = (T)Convert.ChangeType(data, typeof(T));
        }

        public T GetData<T>()
        {
            return (T)Convert.ChangeType(this.data, typeof(T));
        }

        public T GetView<T>()
        {
            return (T)Convert.ChangeType(this.view, typeof(T));
        }

        protected T GetGameController<T>()
        {
            return (T)Convert.ChangeType(this.gameController, typeof(T));
        }
        #endregion --------------------------------------------------------------------------------------

    }
}
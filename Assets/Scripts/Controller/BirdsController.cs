using Common;
using DefaultNamespace.View;
using UnityEngine;

namespace DefaultNamespace.Controller
{
    public class BirdsController : EntityController
    {
        private BirdsView birdsView;

        public override void OnGameStart()
        {
            birdsView = GetView<BirdsView>();
            birdsView.OnGameStart();
        }

        private void Update()
        {
            birdsView.HandleInput();
        }

        public override void OnGameOver()
        {
            birdsView.OnGameOver();
        }
    }
}

using Jenga.Main;
using UnityEngine;

namespace Jenga.UI
{
    public class TestMyStackUI : JengaMonoBehaviour
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        [SerializeField]
        private CanvasGroup group;

        [SerializeField]
        private CustomActionButton closeModeButton;

        [SerializeField]
        private CustomActionButton restartSimulationButton;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            Instances.GameBrain.GameModeStarted += OnGameModeStarted;
            Instances.GameBrain.GameModeEnded += OnGameModeEnded;
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            Instances.GameBrain.GameModeStarted -= OnGameModeStarted;
            Instances.GameBrain.GameModeEnded -= OnGameModeEnded;
        }

        private void OnGameModeStarted(object sender, EGameMode e)
        {
            group.alpha = 1f;
            group.blocksRaycasts = true;
        }

        private void OnGameModeEnded(object sender, EGameMode e)
        {
            group.alpha = 0f;
            group.blocksRaycasts = false;
        }

        protected override void Start()
        {
            base.Start();

            closeModeButton.Initialize(() => Instances.GameBrain.Notify_GameModeEnded(EGameMode.TestMyStack));
            restartSimulationButton.Initialize(Instances.GameBrain.Notify_RestartTestMyStack);
        }

        private void Awake()
        {
            group.blocksRaycasts = false;
            group.alpha = 0;
        }
    }
}
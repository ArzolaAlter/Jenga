using Jenga.Main;
using UnityEngine;

namespace Jenga.UI
{
    /// <summary>
    /// UI button that represents a toggle for a game mode.
    /// </summary>
    public class GameModeButton : AButtonWithCustomAction
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        [SerializeField]
        private EGameMode gameMode;

        // Runtime
        private bool initialState;

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

        private void OnGameModeStarted(object sender, EGameMode args)
        {
            Button.interactable = false;
        }

        private void OnGameModeEnded(object sender, EGameMode args)
        {
            Button.interactable = initialState;
        }

        protected override void ButtonClickAction()
        {
            Instances.GameBrain.Notify_GameModeStarted(gameMode);
        }

        protected override void Start()
        {
            base.Start();
            initialState = Button.interactable;
        }
    }
}
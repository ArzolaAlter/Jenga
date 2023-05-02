using UnityEngine;
using UnityEngine.UI;

namespace Jenga.UI
{
    /// <summary>
    /// Simple parent class that handles the boilerplate required by button components that have a specific on click action.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class AButtonWithCustomAction : JengaMonoBehaviour
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        // Runtime
        private Button button;

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        public Button Button => button;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            button.onClick.AddListener(ButtonClickAction);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            button.onClick.RemoveListener(ButtonClickAction);
        }

        protected abstract void ButtonClickAction();

        private void Awake()
        {
            button = gameObject.GetComponent<Button>();
        }
    }
}
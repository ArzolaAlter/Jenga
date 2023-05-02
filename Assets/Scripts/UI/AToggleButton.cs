using UnityEngine;
using UnityEngine.UI;

namespace Jenga.UI
{
    public abstract class AToggleButton : AButtonWithCustomAction
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        [SerializeField]
        private Image targetImage;

        // Runtime
        private bool state;

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected abstract bool InitialState { get; }
        protected abstract Sprite EnabledSprite { get; }
        protected abstract Sprite DisabledSprite { get; }

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        private void ToggleState()
        {
            state = !state;
            targetImage.sprite = state ? EnabledSprite : DisabledSprite;

            if (state)
            {
                OnOn();
            }
            else
            {
                OnOff();
            }
        }

        protected abstract void OnOn();
        protected abstract void OnOff();

        protected sealed override void ButtonClickAction()
        {
            ToggleState();
        }

        protected override void Start()
        {
            base.Start();
            state = InitialState;
        }
    }
}
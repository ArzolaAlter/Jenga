using System;

namespace Jenga.UI
{
    /// <summary>
    /// Button class that can be initialized to fire a specific instruction.
    /// </summary>
    public class CustomActionButton : AButtonWithCustomAction
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        // Runtime
        private Action buttonAction;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override void ButtonClickAction() => buttonAction?.Invoke();

        public void Initialize(Action ButtonAction) => buttonAction = ButtonAction;
    }
}
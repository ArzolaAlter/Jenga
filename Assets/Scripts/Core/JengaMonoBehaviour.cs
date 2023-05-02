using UnityEngine;

namespace Jenga
{
    public class JengaMonoBehaviour : MonoBehaviour
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        private bool afterStart;

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        /// <summary>
        /// Returns true when this object's <see cref="Start"/> method has been executed.
        /// </summary>
        protected bool AfterStart => afterStart;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected virtual void Start()
        {
            afterStart = true;
            SubscribeToEvents();
        }

        protected virtual void OnEnable()
        {
            if (!afterStart) { return; }
            OnEnableAfterStart();
            SubscribeToEvents();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        /// <summary>
        /// This block is first executed after <see cref="Start"/>, and subsequent times, it's executed after <see cref="OnEnable"/>.
        /// </summary>
        protected virtual void SubscribeToEvents()
        {
        }

        protected virtual void OnEnableAfterStart()
        {

        }

        /// <summary>
        /// Executed alongside <see cref="OnDisable"/>.
        /// </summary>
        protected virtual void UnsubscribeFromEvents()
        {

        }
    }
}
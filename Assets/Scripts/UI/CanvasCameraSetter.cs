using UnityEngine;

namespace Jenga.UI
{
    /// <summary>
    /// Initializes a canvas' world camera on awake to Camera.main.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class CanvasCameraSetter : MonoBehaviour
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        // Runtime
        private Canvas canvas;
        private static Camera camera;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        private void Awake()
        {
            canvas = gameObject.GetComponent<Canvas>();
            canvas.worldCamera = camera ??= Camera.main;
        }
    }
}
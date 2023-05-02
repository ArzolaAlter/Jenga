using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Jenga.UI
{
    /// <summary>
    /// Button that allows to exit the game when clicked.
    /// </summary>
    public class ExitGameButton : AButtonWithCustomAction
    {
        protected override void ButtonClickAction()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}
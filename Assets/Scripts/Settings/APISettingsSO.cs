using UnityEngine;

namespace Jenga.Settings
{
    /// <summary>
    /// Holds the different settings used by the APIManager to prepare information about the game.
    /// </summary>
    [CreateAssetMenu]
    public class APISettingsSO : ScriptableObject
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        [SerializeField, Header("General Settings")]
        [Tooltip("Time in seconds the API Manager is allowed to hang before declaring a timeout. This is handled in seconds.")]
        private int timeout = 5;

        [SerializeField, Header("Student Settings"), Space]
        [Tooltip("Hardcoded url that will be used to sim")]
        private string testStudentURL = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        /// <summary>
        /// Time in seconds the <see cref="Jenga.API.APIManager"/> is allowed to hang before declaring a timeout. This is handled in seconds.
        /// </summary>
        public int Timeout => timeout;
        
        /// <summary>
        /// <para>API url retrieved from the login sequence for this specific player.</para>
        /// <para>On this demo, we are using a hardcoded value.</para>
        /// </summary>
        public string CurrentStudentSubjectDataURL => testStudentURL;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jenga.Settings
{
    /// <summary>
    /// <para>Holds a collection of "hardcoded" and developer tweaked values that determine the different implicit behaviours of the game.</para>
    /// </summary>
    public class GameSettingsManager : Singleton<GameSettingsManager>
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        [SerializeField] private APISettingsSO api;
        [SerializeField] private JengaSettingsSO jenga;

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override GameSettingsManager InstanceTarget
        {
            get => Instances.GameSettings;
            set => Instances.GameSettings = value;
        }

        public APISettingsSO API => api;
        public JengaSettingsSO Jenga => jenga;
    }
}
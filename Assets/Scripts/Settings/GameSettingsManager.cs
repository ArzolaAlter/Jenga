using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jenga.Settings
{
    public class GameSettingsManager : Singleton<GameSettingsManager>
    {
        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override GameSettingsManager InstanceTarget
        {
            get => Instances.GameSettings;
            set => Instances.GameSettings = value;
        }
    }
}
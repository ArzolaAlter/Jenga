using System.Collections;
using System.Collections.Generic;

namespace Jenga.Persistence
{
    public class SaveManager : Singleton<SaveManager>
    {
        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override SaveManager InstanceTarget
        {
            get => Instances.SaveManager;
            set => Instances.SaveManager = value;
        }
    }
}

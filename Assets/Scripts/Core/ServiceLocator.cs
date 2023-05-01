using Jenga.API;
using Jenga.Main;
using Jenga.Persistence;
using Jenga.Settings;

namespace Jenga
{
    /// <summary>
    /// Class that holds references to all of the singletons in the game.
    /// </summary>
    public static class Instances
    {
        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        public static GameBrain GameBrain { get; set; }

        public static APIManager APIManager { get; set; }
        public static SaveManager SaveManager { get; set; }
        public static GameSettingsManager GameSettings { get; set; }
    }
}
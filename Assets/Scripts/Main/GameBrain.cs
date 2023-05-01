using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Jenga.API;
using Newtonsoft.Json;
using UnityEngine;

namespace Jenga.Main
{
    [Serializable]
    public enum EPieceType
    {
        // The values on each of the enum fields match those found in the API's "mastery" field.
        Glass = 0,
        Wood = 1,
        Stone = 2
    }

    /// <summary>
    /// Holds a beautified version of the data required for the game to function.
    /// </summary>
    public class JengaData
    {
        [JsonProperty] public Dictionary<string, List<SubjectData>> GradeToSubject { get; }

        [JsonIgnore]
        public string HighestGradeSubject
        {
            get
            {
                List<string> gradesList = GradeToSubject.Keys.ToList();
                gradesList.Sort((x, y) => String.Compare(y, x, StringComparison.InvariantCulture));

                if (gradesList.Count > 0)
                {
                    return gradesList[0];
                }

                Debug.LogError("The loaded student doesn't have any recorded subjects, and no fallback behaviour has been implemented.");
                return string.Empty;
            }
        }

        [JsonProperty] public string StudentName => "John Doe";

        public JengaData(Dictionary<string, List<SubjectData>> gradeToSubject)
        {
            GradeToSubject = gradeToSubject;
        }

        [JsonConstructor] public JengaData() { }
    }

    /// <summary>
    /// <para>Main brains of the scene containing the different jenga towers and blocks.</para>
    /// <para>Controls the different handlers and components on the scene, and is notified of any relevant actions.</para>
    /// </summary>
    public class GameBrain : Singleton<GameBrain>
    {
        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override GameBrain InstanceTarget
        {
            get => Instances.GameBrain;
            set => Instances.GameBrain = value;
        }

    }
}
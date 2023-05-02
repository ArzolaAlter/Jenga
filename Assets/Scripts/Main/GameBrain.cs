using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Jenga.API;
using Jenga.Settings;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

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

        [JsonIgnore] public string HighestGradeSubject => GradeToSubject.Keys.ToList().Last(); // This version will just use the order in the API as is, would need to talk for a list of all the possible grades to implement this properly.
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
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        [SerializeField, Header("Scene References")] private Transform tableCenter;
        [SerializeField] private Transform tableEdge;

        /// <summary>
        /// Fired the moment the game is actually ready to boot. This mainly means that the game is in a state where student data has already been gathered.
        /// </summary>
        public event EventHandler<JengaData> GameReadyToStart;

        // Runtime
        private JengaData jengaData;

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        private JengaSettingsSO Settings => Instances.GameSettings.Jenga;

        protected override GameBrain InstanceTarget
        {
            get => Instances.GameBrain;
            set => Instances.GameBrain = value;
        }

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        private void StartLoginSequence()
        {
            // Login Is Skipped on Demo

            GetStudentData();
        }

        private void GetStudentData()
        {
            Instances.APIManager.GetStudentData(response =>
            {
                if (response.ResponseResult == UnityWebRequest.Result.Success)
                {
                    Dictionary<string, List<SubjectData>> gradeToSubject = new Dictionary<string, List<SubjectData>>();
                    foreach (SubjectData subject in response.Data)
                    {
                        string key = subject.grade.Trim();

                        if (!gradeToSubject.ContainsKey(key))
                        {
                            gradeToSubject.Add(key, new List<SubjectData>());
                        }

                        gradeToSubject[key].Add(subject);
                    }

                    foreach (List<SubjectData> subjectList in gradeToSubject.Values)
                    {
                        subjectList.Sort((x, y) => String.Compare(x.domain, y.domain, StringComparison.Ordinal));
                        subjectList.Sort((x, y) => String.Compare(x.cluster, y.cluster, StringComparison.Ordinal));
                        subjectList.Sort((x, y) => String.Compare(x.standardid, y.standardid, StringComparison.Ordinal));
                    }

                    jengaData = new JengaData(gradeToSubject);
                    Instances.SaveManager.SaveStudentData(jengaData);
                }
                else
                {
                    if (!Instances.SaveManager.HasCachedStudentData)
                    {
                        // We are in an error state where there is no data to work with.
                        Debug.LogError("The game is unable to start, no prior connection has been established to load data from.");
                        return;
                    }

                    jengaData = Instances.SaveManager.CachedStudentData;
                }

                GameReadyToStart?.Invoke(this, jengaData);
                InitializeTowers();
            });
        }

        private void InitializeTowers()
        {
            float anglePerTower = 360f / (float)jengaData.GradeToSubject.Count;
            float currentAngle = 0;

            foreach (var kvp in jengaData.GradeToSubject)
            {
                Vector3 pos = tableEdge.position.RotatePointAroundPivot(tableCenter.position, Vector3.up, currentAngle);

                JengaTower tower = GameObject.Instantiate(Settings.JengaTowerPrefab, pos, Quaternion.Euler(new Vector3(0, currentAngle + 180, 0)));

                tower.Initialize(kvp.Key, kvp.Value);

                currentAngle += anglePerTower;
            }
        }

        private void Start()
        {
            StartLoginSequence();
        }
    }
}
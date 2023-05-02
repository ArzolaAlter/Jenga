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

    [Serializable]
    public enum EGameMode
    {
        TestMyStack = 0,
        StrengthenMyStack = 1,
        Earthquake = 2,
        BuildMyStack = 3,
        ChallengeClassmate = 4
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

        /// <summary>
        /// Fired the moment the game is actually ready to boot. This mainly means that the game is in a state where student data has already been gathered.
        /// </summary>
        public event EventHandler<JengaData> GameReadyToStart;

        /// <summary>
        /// Fired when the internal state of the game changes to being inside a give game mode.
        /// </summary>
        public event EventHandler<EGameMode> GameModeStarted;

        /// <summary>
        /// Fired when the internal state of the game changes to no longer being inside a game mode.
        /// </summary>
        public event EventHandler<EGameMode> GameModeEnded;

        /// <summary>
        /// Fired whenever the currently selected grade has changed.
        /// </summary>
        public event EventHandler<JengaTower> SelectedGradeChanged;

        [SerializeField, Header("Scene References")]
        private Transform tableCenter;

        [SerializeField]
        private Transform tableEdge;

        // Runtime
        private JengaData jengaData;
        private Dictionary<string, JengaTower> gradeToTower = new Dictionary<string, JengaTower>();
        private string currentGrade;
        private List<string> gradeOrder;

        private EGameMode currentGameMode;

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        private JengaTower CurrentTower => gradeToTower[currentGrade];
        private int CurrentTowerIndex => gradeOrder.IndexOf(currentGrade);
        private JengaSettingsSO Settings => Instances.GameSettings.Jenga;

        protected override GameBrain InstanceTarget
        {
            get => Instances.GameBrain;
            set => Instances.GameBrain = value;
        }

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        #region Boot Sequence

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

        #endregion Boot Sequence

        #region Towers

        private void InitializeTowers()
        {
            float anglePerTower = 360f / (float)jengaData.GradeToSubject.Count;
            float currentAngle = 360;

            foreach (var kvp in jengaData.GradeToSubject)
            {
                Vector3 pos = tableEdge.position.RotatePointAroundPivot(tableCenter.position, Vector3.up, currentAngle);

                JengaTower tower = GameObject.Instantiate(Settings.JengaTowerPrefab, pos, Quaternion.Euler(new Vector3(0, currentAngle + 180, 0)));

                tower.Initialize(kvp.Key, kvp.Value);

                gradeToTower.Add(kvp.Key, tower);

                currentAngle -= anglePerTower;
            }

            gradeOrder = gradeToTower.Keys.ToList();
            gradeOrder = gradeOrder.OrderBy(s => s).ToList();

            currentGrade = gradeOrder[0];
            UpdateSelectedTower(gradeToTower[currentGrade]);
        }

        private void UpdateSelectedTower(JengaTower tower)
        {
            SelectedGradeChanged?.Invoke(this, tower);
        }

        public void Notify_SelectLeftTower()
        {
            int currentIndex = CurrentTowerIndex;
            if (currentIndex == 0)
            {
                currentGrade = gradeOrder[gradeOrder.Count - 1];
            }
            else
            {
                currentGrade = gradeOrder[currentIndex - 1];
            }

            UpdateSelectedTower(gradeToTower[currentGrade]);
        }

        public void Notify_SelectRightTower()
        {
            int currentIndex = CurrentTowerIndex;
            if (currentIndex == gradeOrder.Count - 1)
            {
                currentGrade = gradeOrder[0];
            }
            else
            {
                currentGrade = gradeOrder[currentIndex + 1];
            }

            UpdateSelectedTower(gradeToTower[currentGrade]);
        }

        #endregion Towers

        #region Game Modes
        
        public void Notify_GameModeStarted(EGameMode gameMode)
        {
            currentGameMode = gameMode;
            GameModeStarted?.Invoke(this, currentGameMode);
        }

        public void Notify_GameModeEnded(EGameMode gameMode)
        {
            GameModeEnded?.Invoke(this, currentGameMode);
        }

        public void Notify_RestartTestMyStack()
        {
            CurrentTower.StartOrRestartSimulation();
        }

        #endregion Game Modes

        private void Start()
        {
            StartLoginSequence();
        }
    }
}
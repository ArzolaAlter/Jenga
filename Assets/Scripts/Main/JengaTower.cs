using System;
using System.Collections.Generic;
using Jenga.API;
using Jenga.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Playables;

namespace Jenga.Main
{
    /// <summary>
    /// Class that holds the information about a jenga tower, from its physical properties and camera targets to its pieces.
    /// </summary>
    public class JengaTower : JengaMonoBehaviour
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        [SerializeField]
        private Transform towerCenter;

        [SerializeField]
        private TMP_Text[] gradeLabels = new TMP_Text[0];

        // Runtime
        private List<JengaPiece> towerPieces = new List<JengaPiece>();
        private string targetGrade;

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        public string TargetGrade => targetGrade;
        private JengaSettingsSO Settings => Instances.GameSettings.Jenga;
        public Transform TowerCenter => towerCenter;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            Instances.GameBrain.GameModeEnded += OnGameModeEnded;
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            Instances.GameBrain.GameModeEnded -= OnGameModeEnded;
        }

        private void OnGameModeEnded(object sender, EGameMode e)
        {
            foreach (JengaPiece piece in towerPieces)
            {
                piece.StopPhysicsSimulation();
            }
        }

        private (Vector3 pos, Vector3 rot) GetTargetPieceSpot(int base1PieceIndex)
        {
            int level = base1PieceIndex / 3;
            float positionMultiplier;
            switch (base1PieceIndex % 3)
            {
                case 0:
                    positionMultiplier = -1;
                    break;
                case 1:
                    positionMultiplier = 0;
                    break;
                case 2:
                    positionMultiplier = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("");
            }

            bool isRotated = level % 2 == 1;

            float x = isRotated ? 0 : Settings.SinglePieceOffset.x * positionMultiplier;
            float y = level * Settings.SinglePieceOffset.y;
            float z = isRotated ? Settings.SinglePieceOffset.x * positionMultiplier : 0;

            return (new Vector3(x, y, z), isRotated ? new Vector3(0, 90 + (base1PieceIndex == 1 ? 180 : 0), 0) : Vector3.zero);
        }

        public void Initialize(string gradeString, List<SubjectData> piecesData)
        {
            targetGrade = gradeString;
            foreach (TMP_Text label in gradeLabels)
            {
                label.SetText(gradeString);
            }

            Vector3 worldPosition = towerCenter.position;

            foreach (SubjectData pieceData in piecesData)
            {
                var (localPos, localRot) = GetTargetPieceSpot(towerPieces.Count);
                JengaPiece piece = GameObject.Instantiate(Settings.JengaPiecePrefab, localPos + worldPosition, Quaternion.Euler(localRot), towerCenter);

                piece.Initialize(towerPieces.Count, pieceData);

                towerPieces.Add(piece);
            }
        }

        public void StartOrRestartSimulation()
        {
            int totalPieces = towerPieces.Count;

            void AttemptToStartSimulation()
            {
                totalPieces--;
                if (totalPieces > 0) { return; }

                foreach (JengaPiece piece in towerPieces)
                {
                    piece.StartPhysicsSimulation();
                }
            }

            foreach (JengaPiece piece in towerPieces)
            {
                piece.PollCanStartSimulation(AttemptToStartSimulation);
            }
        }
    }
}
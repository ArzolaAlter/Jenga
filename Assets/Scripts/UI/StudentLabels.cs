using System;
using Jenga.Main;
using TMPro;
using UnityEngine;

namespace Jenga.UI
{
    public class StudentLabels : JengaMonoBehaviour
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        [SerializeField]
        private TMP_Text singleLabel;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            Instances.GameBrain.GameReadyToStart += OnGameReady;
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            Instances.GameBrain.GameReadyToStart -= OnGameReady;
        }

        private void OnGameReady(object sender, JengaData data)
        {
            string topLabel = $"<u><b><smallcaps>{data.StudentName}</smallcaps></b></u>";
            string bottomLabel = $"{data.HighestGradeSubject} Student";

            singleLabel.SetText($"{topLabel}{Environment.NewLine}{bottomLabel}");
        }
    }
}
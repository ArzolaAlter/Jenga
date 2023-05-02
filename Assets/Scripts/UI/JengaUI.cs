using System.Collections;
using System.Collections.Generic;
using Jenga.Main;
using TMPro;
using UnityEngine;

namespace Jenga.UI
{
    public class JengaUI : JengaMonoBehaviour
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        [SerializeField]
        private CustomActionButton leftArrow;

        [SerializeField]
        private CustomActionButton rightArrow;

        [SerializeField]
        private TMP_Text currentlySelectedGradeLabel;

        // Runtime
        private bool gameReadyToStart;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            Instances.GameBrain.GameReadyToStart += OnGameReadyToStart;
            Instances.GameBrain.SelectedGradeChanged += OnGradeChanged;
            Instances.GameBrain.GameModeStarted += OnGameModeStarted;
            Instances.GameBrain.GameModeEnded += OnGameModeEnded;
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            Instances.GameBrain.GameReadyToStart -= OnGameReadyToStart;
            Instances.GameBrain.SelectedGradeChanged -= OnGradeChanged;
            Instances.GameBrain.GameModeStarted -= OnGameModeStarted;
            Instances.GameBrain.GameModeEnded -= OnGameModeEnded;
        }
        
        private void OnGameModeStarted(object sender, EGameMode args)
        {
            leftArrow.Button.interactable = false;
            rightArrow.Button.interactable = false;
        }

        private void OnGameModeEnded(object sender, EGameMode args)
        {
            leftArrow.Button.interactable = true;
            rightArrow.Button.interactable = true;
        }

        private void OnGradeChanged(object sender, JengaTower args)
        {
            currentlySelectedGradeLabel.SetText(args.TargetGrade);
        }

        private void OnGameReadyToStart(object sender, JengaData e)
        {
            gameReadyToStart = true;
        }

        private void Update()
        {
            if (!gameReadyToStart) { return; }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 10))
            {
                JengaPiece piece = hit.transform.gameObject.GetComponentInParent<JengaPiece>();
                if (piece == null) { return; }


            }
        }

        protected override void Start()
        {
            base.Start();

            leftArrow.Initialize(Instances.GameBrain.Notify_SelectLeftTower);
            rightArrow.Initialize(Instances.GameBrain.Notify_SelectRightTower);
        }
    }
}
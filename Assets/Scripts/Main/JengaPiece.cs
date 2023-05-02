using System;
using Jenga.API;
using Jenga.Settings;
using MEC;
using TMPro;
using UnityEngine;

namespace Jenga.Main
{
    /// <summary>
    /// Holds information about a specific jenga piece in the scene.
    /// </summary>
    public class JengaPiece : MonoBehaviour
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        [SerializeField, Header("Scene References")]
        private Renderer pieceRenderer;

        [SerializeField]
        private TMP_Text[] typeLabels = new TMP_Text[0];

        [SerializeField]
        private Rigidbody rigidbody;

        [SerializeField, Header("Animation Data")]
        private float scaleOutTimer = 1f;

        [SerializeField]
        private AnimationCurve scaleOutCurve = new AnimationCurve();

        // Runtime
        private int pieceIndex;
        private SubjectData subjectData;

        private Vector3 targetPosition;
        private Vector3 targetRotation;

        private CoroutineHandle shrinkHandle;

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        private JengaSettingsSO Settings => Instances.GameSettings.Jenga;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        private void ResetSizeAndPosition()
        {
            transform.localScale = Vector3.one;
            transform.localEulerAngles = targetRotation;
            transform.localPosition = targetPosition;
        }

        private void ShrinkOutOfExistence(Action OnEnd = null)
        {
            LeanTween.scale(gameObject, Vector3.zero, scaleOutTimer)
                .setEase(scaleOutCurve)
                .setOnComplete(() => OnEnd?.Invoke());
        }

        public void Initialize(int index, SubjectData subjectData)
        {
            pieceIndex = index;
            this.subjectData = subjectData;

            PieceTypeData drawSettings = Settings[subjectData.PieceType];
            foreach (TMP_Text label in typeLabels)
            {
                label.SetText(drawSettings.JengaPieceLabelText);
            }

            pieceRenderer.material = drawSettings.VisualMaterial;

            rigidbody.mass = drawSettings.PieceMass;

            targetPosition = transform.localPosition;
            targetRotation = transform.localEulerAngles;

            ResetSizeAndPosition();
        }

        /// <summary>
        /// Asks the piece to ready itself to start a simulation, calling the provided action when so.
        /// </summary>
        public void PollCanStartSimulation(Action OnReady)
        {
            if (subjectData.PieceType != EPieceType.Glass)
            {
                OnReady();
                return;
            }

            ShrinkOutOfExistence(OnReady);
        }

        public void StartPhysicsSimulation()
        {
            if (subjectData.PieceType == EPieceType.Glass) { return; }

            ResetSizeAndPosition();
            rigidbody.isKinematic = false;
        }

        public void StopPhysicsSimulation()
        {
            if (subjectData.PieceType == EPieceType.Glass) { return; }

            ResetSizeAndPosition();
            rigidbody.isKinematic = true;
        }

        private void Awake()
        {
            rigidbody.isKinematic = true;
        }
    }
}
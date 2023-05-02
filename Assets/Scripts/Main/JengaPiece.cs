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

        private Vector3 initialRBGOPosition;
        private Vector3 initialRBGORotation;
        
        private CoroutineHandle shrinkHandle;
        private GameObject rbGameObject;

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        public SubjectData SubjectData => subjectData;
        private JengaSettingsSO Settings => Instances.GameSettings.Jenga;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        private void ResetSizeAndPosition()
        {
            transform.localScale = Vector3.one;
            transform.localEulerAngles = targetRotation;
            transform.localPosition = targetPosition;

            rbGameObject.transform.localEulerAngles = initialRBGORotation;
            rbGameObject.transform.localPosition = initialRBGOPosition;
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
            rigidbody.velocity = Vector3.zero;

            foreach (var label in typeLabels)
            {
                Color c = label.color;
                label.color = new Color(c.r, c.g, c.b, 0);
            }
        }

        public void StopPhysicsSimulation()
        {
            ResetSizeAndPosition();
            rigidbody.isKinematic = true;

            foreach (var label in typeLabels)
            {
                Color c = label.color;
                label.color = new Color(c.r, c.g, c.b, 1);
            }
        }

        private void Awake()
        {
            rigidbody.isKinematic = true;
            rbGameObject = rigidbody.gameObject;

            initialRBGOPosition = rbGameObject.transform.localPosition;
            initialRBGORotation = rbGameObject.transform.localEulerAngles;
        }
    }
}
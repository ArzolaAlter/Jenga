using System;
using System.Collections.Generic;
using Jenga.Main;
using UnityEngine;

namespace Jenga.Settings
{
    /// <summary>
    /// Holds the specific data that determines the behavior and visuals of jenga pieces based on their type.
    /// </summary>
    [Serializable]
    public class PieceTypeData
    {
        [SerializeField]
        private EPieceType pieceType = EPieceType.Glass;

        [SerializeField]
        [Tooltip("Physical mass the piece should have on simulations.")]
        private float pieceMass = 1f;

        [SerializeField]
        [Tooltip("Material that will be assigned to the jenga piece of this type.")]
        private Material visualMaterial;

        [SerializeField]
        [Tooltip("Text that will be shown the sides of jenga pieces to illustrate the mastery level this piece type represents.")]
        private string jengaPieceLabelText = "MASTERED";

        [SerializeField]
        [Tooltip("Text that will be shown on the UI to represent this type.")]
        private string uiLabelText = "Glass";

        public EPieceType PieceType => pieceType;

        /// <summary>
        /// Physical mass the piece should have on simulations.
        /// </summary>
        public float PieceMass => pieceMass;

        /// <summary>
        /// Text that will be shown the sides of jenga pieces to illustrate the mastery level this piece type represents.
        /// </summary>
        public string JengaPieceLabelText => jengaPieceLabelText;

        /// <summary>
        /// Text that will be shown on the UI to represent this type.
        /// </summary>
        public string UILabelText => uiLabelText;

        /// <summary>
        /// Material that will be assigned to the jenga piece of this type.
        /// </summary>
        public Material VisualMaterial => visualMaterial;
    }

    /// <summary>
    /// Container class that holds tweakable settings for the Jenga aspects of the game, to make sure it feels and looks right.
    /// </summary>
    [CreateAssetMenu]
    public class JengaSettingsSO : ScriptableObject
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        [SerializeField, Header("Piece Types")]
        private PieceTypeData[] pieceTypes = new PieceTypeData[3];

        [SerializeField, Header("Jenga Sizes")]
        [Tooltip("hysical distance there should be between each of the jenga pieces. Base this on the piece prefab, and ensure this order: wide x tall x long.")]
        private Vector3 singlePieceOffset = new Vector3(2.5f, 1.5f, 7.5f);

        [SerializeField]
        [Tooltip("Determines the radius the camera will use to orbit around the bottom center section of the tower.")]
        private float jengaTowerRadius = 12f;

        [SerializeField]
        [Tooltip("Determines the distance the camera can go above the topmost part of the jenga tower while orbiting.")]
        private float jengaTowerTopHoverDistance = 3f;

        [SerializeField]
        [Tooltip("Determines the distance between each jenga tower, this distance respects the radiuses established.")]
        private float jengaTowerDistance = 6f;

        [SerializeField]
        private JengaPiece jengaPiecePrefab;

        [SerializeField]
        private JengaTower jengaTowerPrefab;

        // Runtime
        [NonSerialized] private Dictionary<EPieceType, PieceTypeData> pieceTypeToData;

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        public JengaPiece JengaPiecePrefab => jengaPiecePrefab;
        public JengaTower JengaTowerPrefab => jengaTowerPrefab;

        public PieceTypeData this[EPieceType pieceType]
        {
            get
            {
                if (pieceTypeToData == null)
                {
                    pieceTypeToData = new Dictionary<EPieceType, PieceTypeData>();

                    foreach (PieceTypeData data in pieceTypes)
                    {
                        if (pieceTypeToData.ContainsKey(data.PieceType))
                        {
                            Debug.LogError($"The object used for jenga game settings has duplicate fields for pieces of type: {data.PieceType}. Please check this.");
                            continue;
                        }

                        pieceTypeToData.Add(data.PieceType, data);
                    }

                    foreach (EPieceType type in Enum.GetValues(typeof(EPieceType)))
                    {
                        if (!pieceTypeToData.ContainsKey(type))
                        {
                            Debug.LogError($"The object used for jenga game settings doesn't have an entry for pieces of type: {type}. Please add it.");
                        }
                    }
                }

                return pieceTypeToData[pieceType];
            }
        }

        /// <summary>
        /// Physical distance there should be between each of the jenga pieces. Base this on the piece prefab, and ensure this order: wide x tall x long.
        /// </summary>
        public Vector3 SinglePieceOffset => singlePieceOffset;

        /// <summary>
        /// Determines the radius the camera will use to orbit around the bottom center section of the tower.
        /// </summary>
        public float JengaTowerRadius => jengaTowerRadius;

        /// <summary>
        /// Determines the distance the camera can go above the topmost part of the jenga tower while orbiting.
        /// </summary>
        public float JengaTowerTopHoverDistance => jengaTowerTopHoverDistance;

        /// <summary>
        /// Determines the distance between each jenga tower, this distance respects the radiuses established.
        /// </summary>
        public float JengaTowerDistance => jengaTowerDistance;
    }
}
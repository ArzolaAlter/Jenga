using System;
using Jenga.Main;
using TMPro;
using UnityEngine;

namespace Jenga.UI
{
    /// <summary>
    /// UI element that holds information about the currently hovered JengaPiece.
    /// </summary>
    public class JengaPieceTooltip : MonoBehaviour
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        [SerializeField, Header("Scene References")]
        private CanvasGroup tooltipGroup;

        [SerializeField]
        private TMP_Text mainLabel;

        [SerializeField, Header("Animation")]
        private float alphaPerSecond = 0.7f;

        // Runtime
        private bool revealing;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        public void RevealTooltip(JengaPiece targetPiece)
        {
            var data = targetPiece.SubjectData;

            string topLine = $"<color=#FFF><smallcaps><size=140%> {data.grade} : {data.domain} </size></smallcaps></color>";
            string middleLine = $"<color=#D19200>{data.cluster}</color>";
            string bottomLine = $"{data.standardid}: {data.standarddescription}";

            mainLabel.SetText($"{topLine}{Environment.NewLine}{middleLine}{Environment.NewLine}{bottomLine}");

            revealing = true;
        }

        public void HideTooltip()
        {
            revealing = false;
        }

        private void Update()
        {
            float finalAlpha = tooltipGroup.alpha + alphaPerSecond * Time.deltaTime * (revealing ? 1f : -1f);
            tooltipGroup.alpha = Mathf.Clamp01(finalAlpha);
        }

        private void Awake()
        {
            tooltipGroup.alpha = 0f;
            tooltipGroup.blocksRaycasts = false;
        }
    }
}
using System;
using Jenga.Main;
using TMPro;
using UnityEngine;

namespace Jenga.UI
{
    /// <summary>
    /// UI element that holds information about the currently hovered JengaPiece.
    /// </summary>
    public class JengaPieceTooltip : JengaMonoBehaviour
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

        [SerializeField]
        private float maxAlpha = 0.8f;

        // Runtime
        private bool revealing;
        private RectTransform rt;
        private Canvas parentCanvas;

        private bool disabled;

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        private bool ShouldShow => revealing && !disabled;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            Instances.GameBrain.GameModeStarted += OnGameModeStarted;
            Instances.GameBrain.GameModeEnded+= OnGameModeEnded;
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            Instances.GameBrain.GameModeStarted -= OnGameModeStarted;
            Instances.GameBrain.GameModeEnded -= OnGameModeEnded;
        }

        private void OnGameModeEnded(object sender, EGameMode e)
        {
            disabled = false;
        }

        private void OnGameModeStarted(object sender, EGameMode e)
        {
            disabled = true;
        }

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
            float finalAlpha = tooltipGroup.alpha + alphaPerSecond * Time.deltaTime * (ShouldShow ? 1f : -1f);
            tooltipGroup.alpha = Mathf.Clamp(finalAlpha,0, maxAlpha);

            RectTransform parentRT = parentCanvas.transform as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT,
                Input.mousePosition, parentCanvas.worldCamera,
                out Vector2 finalPos);

            transform.position = parentCanvas.transform.TransformPoint(finalPos);

            Vector3 clampedPos = rt.anchoredPosition;

            float maxX = parentRT.rect.width - rt.rect.width;
            if (clampedPos.x > maxX)
            {
                clampedPos.x = maxX;
            }

            float maxY = parentRT.rect.height - rt.rect.height;
            if (clampedPos.y > maxY)
            {
                clampedPos.y = maxY;
            }

            rt.anchoredPosition = clampedPos;
        }

        private void Awake()
        {
            tooltipGroup.alpha = 0f;
            tooltipGroup.blocksRaycasts = false;
            rt = transform as RectTransform;
            parentCanvas = gameObject.GetComponentInParent<Canvas>();
        }
    }
}
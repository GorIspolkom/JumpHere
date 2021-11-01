using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public enum HideSide
    {
        Up,
        Down,
        Right,
        Left
    }
    public class PanelMoveHide : PanelSpawnController
    {
        [SerializeField] HideSide side;
        [SerializeField] float finalXOrY;

        public override void HidePanel()
        {
            base.HidePanel();
            RectTransform rectTransform = transform as RectTransform;
            Vector2 resolution = GameObject.FindObjectOfType<Canvas>().pixelRect.size;
            Debug.Log(resolution.y);
            switch (side)
            {
                case HideSide.Up:
                    rectTransform.DOMoveY(resolution.y * rectTransform.anchorMin.y + rectTransform.rect.height * rectTransform.pivot.y, time).SetEase(ease);
                    break;
                case HideSide.Down:
                    rectTransform.DOMoveY(-rectTransform.rect.height * (1 - rectTransform.pivot.y), time).SetEase(ease);
                    break;
                case HideSide.Right:
                    rectTransform.DOMoveX(resolution.x * (1 - rectTransform.anchorMin.x) + rectTransform.rect.width * rectTransform.pivot.x, time).SetEase(ease);
                    break;
                case HideSide.Left:
                    rectTransform.DOMoveX(-rectTransform.rect.width * (1 - rectTransform.pivot.x), time).SetEase(ease);
                    break;
                default:
                    rectTransform.DOMoveX(0, 1f);
                    break;
            }
        }
        public override void OpenPanel()
        {
            base.OpenPanel();
            RectTransform panel = transform as RectTransform;
            Vector2 resolution = GameObject.FindObjectOfType<Canvas>().pixelRect.size;
            switch (side)
            {
                case HideSide.Up:
                    panel.DOMoveY(resolution.y - finalXOrY, time).SetEase(ease);
                    break;
                case HideSide.Down:
                    panel.DOMoveY(finalXOrY, time).SetEase(ease);
                    break;
                case HideSide.Right:
                    panel.DOMoveX(resolution.x - finalXOrY, time).SetEase(ease);
                    break;
                case HideSide.Left:
                    panel.DOMoveX(finalXOrY, time).SetEase(ease);
                    break;
            }
        }
    }
}
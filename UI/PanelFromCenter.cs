using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI
{
    class PanelFromCenter : PanelSpawnController
    {
        public override void HidePanel()
        {
            base.HidePanel();
            (transform as RectTransform).DOScale(Vector3.zero, time).SetEase(ease);
        }
        public override void OpenPanel()
        {
            base.OpenPanel();
            (transform as RectTransform).DOScale(Vector3.one, time).SetEase(ease);
        }
    }
}

using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    class PanelFade : PanelSpawnController
    {
        public float maxAlpha;
        public override void HidePanel()
        {
            base.HidePanel();
            transform.GetComponent<Image>().DOFade(0, time).SetEase(ease);
        }
        public override void OpenPanel()
        {
            base.OpenPanel();
            transform.GetComponent<Image>().DOFade(maxAlpha, time).SetEase(ease);
        }
    }
}

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public abstract class PanelSpawnController : MonoBehaviour
    {
        [SerializeField] protected bool isOpen = true;
        [SerializeField] protected float time;
        [SerializeField] protected Ease ease;
        private void Start()
        {
            if (isOpen)
                OpenPanel();
        }
        public void DestroyOnHide()
        {
            HidePanel();
            Destroy(gameObject, time);
        }
        public void DisableOnHide()
        {
            HidePanel();
            StartCoroutine(WaitForDisable());
        }
        private IEnumerator WaitForDisable()
        {
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
        }
        public void SwapPanel()
        {
            if (isOpen)
                HidePanel();
            else
                OpenPanel();
            isOpen = !isOpen;
        }
        public void SetTime(float newTime)
        {
            this.time = newTime;
        }
        public virtual void HidePanel()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            isOpen = false;
        }
        public virtual void OpenPanel()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            isOpen = true;
        }
    }
}

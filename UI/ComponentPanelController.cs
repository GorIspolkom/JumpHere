using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ComponentPanelController : MonoBehaviour
    {
        [SerializeField] float time;
        [SerializeField] bool isOpenOnStart;
        PanelSpawnController[] panels;
        private void Awake()
        {
            panels = transform.GetComponentsInChildren<PanelSpawnController>();
        }
        private void Start()
        {
            foreach (PanelSpawnController panel in panels)
                panel.SetTime(time);
            gameObject.SetActive(isOpenOnStart);
        }
        public void HidePanel()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            foreach (PanelSpawnController panel in panels)
                panel.HidePanel();
        }
        public void DisableOnHide()
        {
            foreach (PanelSpawnController panel in panels)
                panel.DisableOnHide();
        }

        public void OpenPanel()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            StartCoroutine(DoWithDelay());
        }
        private IEnumerator DoWithDelay()
        {
            yield return new WaitForSeconds(time);

            foreach (PanelSpawnController panel in panels)
                panel.OpenPanel();
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class ChangeLanguage : MonoBehaviour
{
    private void Start()
    {
        LocalizationManager.GetLocalizationManager().InitLanguage();
        GetComponent<Button>().onClick.AddListener(LocalizationManager.GetLocalizationManager().NextLanguage);
    }
}

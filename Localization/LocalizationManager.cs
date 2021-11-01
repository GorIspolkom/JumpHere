using UnityEngine;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField]
    private LanguagePack[] _packs;
    [SerializeField]
    private Text[] _texts;
    private int _packIndex;
    private static LocalizationManager _manager;

    public static LocalizationManager GetLocalizationManager()
    {
        if (_manager == null)
            _manager = FindObjectOfType<LocalizationManager>();
        return _manager;
    }
    public void NextLanguage()
    {
        _packs[_packIndex].currentLanguage = 0;
        if (_packIndex >= _packs.Length - 1)
            _packIndex = 0; 
        else
            _packIndex += 1;
        _packs[_packIndex].currentLanguage = 1;
        SetNewLanguage();
    }
    public void InitLanguage()
    {
        SearchPack();
        SetNewLanguage();
    }
    private void SetNewLanguage()
    {
        for (int i = 0; i < _texts.Length; i++)
            _texts[i].text = _packs[_packIndex].words[i];
    }
    private void SearchPack()
    {
        for (int i = 0; i < _packs.Length; i++)
            if (_packs[i].currentLanguage == 1)
            {
                _packIndex = i;
                break;
            }
    }
}

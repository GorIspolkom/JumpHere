using UnityEngine;

[CreateAssetMenu(fileName = "Language", menuName = "ScriptableObjects/LanguageManager", order = 1)]
public class LanguagePack : ScriptableObject
{
    [Header("Pack settings")]

    [SerializeField]
    public int currentLanguage;
    [Header("Words for pack (order - Pause, Higth score, Exit \n Play, Menu, Restart, Continue, Lose)")]
    [SerializeField]
    public string[] words;
}

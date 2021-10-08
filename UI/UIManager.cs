using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _UIManager;
    [SerializeField]
    private Text _text;
    public static UIManager GetUIManager() =>
            _UIManager == null ? FindObjectOfType<UIManager>() : _UIManager;
    public void SetText() => _text.text = "Points " + Data._points;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreShow : MonoBehaviour
{
    [SerializeField] string preScoreString = "Points: ";
    Text _text;
    private void Awake()
    {
        _text = GetComponent<Text>();   
    }
    void Update()
    {
        _text.text = preScoreString + Data.points.ToString("0");
    }
}

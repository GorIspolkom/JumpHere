using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsCounter 
{
    [SerializeField]
    private Text _text;
    public void SetText() => _text.text = "Points " + Data.points;
}

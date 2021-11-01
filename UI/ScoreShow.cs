using Assets.Scripts.Data;
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
        
        _text.text = preScoreString + SessionData.points.ToString("0");
    }
}

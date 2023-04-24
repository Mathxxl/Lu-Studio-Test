using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreDisplayManager : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        GameManager.Instance.OnScoreUpdate += UpdateScore;
        UpdateScore();
    }

    private void UpdateScore()
    {
        _text.text = "Score : " + GameManager.Instance.Score;
    }
}

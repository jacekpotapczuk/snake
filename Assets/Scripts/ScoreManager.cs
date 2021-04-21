using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI lengthText;

    public bool IsBest => Score > ScoreFileManager.Instance.Score;

    public int Score { get; private set; }

    public int BestScore => ScoreFileManager.Instance.Score;

    public void SaveScore()
    {
        if(IsBest)
            ScoreFileManager.Instance.SaveScore(Score);
    }
    
    public void Restart()
    {
        Score = 0;
        scoreText.text = Score.ToString();
    }

    public void AddToCurrentScore(int amount)
    {
        Score += amount;
        scoreText.text = Score.ToString();
    }

    public void UpdateCurrentLength(int length)
    {
        lengthText.text = length.ToString();
    }

}

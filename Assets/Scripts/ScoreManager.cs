using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI lengthText;
    
    private int currentScore;

    public bool IsBest
    {
        get
        {
            return currentScore > ScoreFileManager.Instance.Score;
        }
    }

    public int Score => currentScore;

    public int BestScore => ScoreFileManager.Instance.Score;

    public void SaveScore()
    {
        if(IsBest)
            ScoreFileManager.Instance.SaveScore(currentScore);
    }
    
    public void Restart()
    {
        currentScore = 0;
        scoreText.text = currentScore.ToString();
    }

    public void AddToCurrentScore(int amount)
    {
        currentScore += amount;
        scoreText.text = currentScore.ToString();
    }

    public void UpdateCurrentLength(int length)
    {
        lengthText.text = length.ToString();
    }

}

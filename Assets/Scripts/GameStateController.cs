using TMPro;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject newBest;
    [SerializeField] private GameObject notNewBest;

    [SerializeField] private TextMeshProUGUI[] bestScoreTexts;
    [SerializeField] private TextMeshProUGUI[] currentScoreTexts;

    private void Awake()
    {
        gameOverPanel.SetActive(false);
    }

    public void OnSnakeDeath(int currentScore, int bestScore)
    {
        Time.timeScale = 0f;
        AudioManager.Instance.MuteAllLooped();
        ShowEndGameMenu(currentScore, bestScore);
    }

    private void ShowEndGameMenu(int current, int best)
    {

        gameOverPanel.SetActive(true);

        bool isBest = current > best;
        newBest.SetActive(isBest);
        notNewBest.SetActive(!isBest);

        string bestStr = isBest ? current.ToString() : best.ToString();

        foreach (TextMeshProUGUI t in bestScoreTexts)
        {
            t.text = bestStr;
        }

        string currentStr = current.ToString();
        foreach (TextMeshProUGUI t in currentScoreTexts)
        {
            t.text = currentStr;
        }
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        GameManager.Instance.LoadScene("Game");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.LoadScene("Menu");
    }
}

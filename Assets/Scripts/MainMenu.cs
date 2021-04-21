using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        GameManager.Instance.LoadScene("Game");
    }
}

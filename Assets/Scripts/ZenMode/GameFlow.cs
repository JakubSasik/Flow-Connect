using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    [Header("Win Panel Buttons")]
    public GameObject prevButton;
    public GameObject nextButton;

    [Header("Scene Names")]
    public string zenGameSceneName = "ZenGame";
    public string menuSceneName = "ZenSelect";

    int Difficulty => PlayerPrefs.GetInt("difficulty", 0); 

    public void RefreshWinUI()
    {
        int d = Difficulty;

        if (prevButton != null)
            prevButton.SetActive(d > 0);  

        if (nextButton != null)
            nextButton.SetActive(d < 2);   
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(zenGameSceneName);
    }

    public void NextDifficulty()
    {
        int d = Difficulty;
        if (d >= 2) return;

        PlayerPrefs.SetInt("difficulty", d + 1);
        PlayerPrefs.Save();

        Replay();
    }

    public void PrevDifficulty()
    {
        int d = Difficulty;
        if (d <= 0) return;

        PlayerPrefs.SetInt("difficulty", d - 1);
        PlayerPrefs.Save();

        Replay();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
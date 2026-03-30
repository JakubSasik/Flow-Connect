using UnityEngine;
using UnityEngine.SceneManagement;

public class ZenMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject infoPanel;
    public GameObject highScorePanel;
    void Start()
    {
    PlayerPrefs.DeleteKey("ZenInfoShown");
    }

    public void Easy()
    {
        PlayerPrefs.SetInt("difficulty", 0);
        SceneManager.LoadScene("ZenGame");
    }

    public void Normal()
    {
        PlayerPrefs.SetInt("difficulty", 1);
        SceneManager.LoadScene("ZenGame");
    }

    public void Hard()
    {
        PlayerPrefs.SetInt("difficulty", 2);
        SceneManager.LoadScene("ZenGame");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("ModeSelect");
    }


    public void OpenInfo()
    {
        infoPanel.SetActive(true);
    }

    public void CloseInfo()
    {
        infoPanel.SetActive(false);
    }

    
    public void OpenHighScore()
    {
        highScorePanel.SetActive(true);
    }

    public void CloseHighScore()
    {
        highScorePanel.SetActive(false);
    }
}
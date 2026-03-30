using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject highScorePanel;
    public GameObject infoPanel;

    void Start()
    {
        PlayerPrefs.DeleteKey("FlowInfoShown");
        if (highScorePanel) highScorePanel.SetActive(false);
        if (infoPanel)      infoPanel.SetActive(true);
    }

    public void Level1() => SceneManager.LoadScene("Flow1");
    public void Level2() => SceneManager.LoadScene("Flow2");
    public void Level3() => SceneManager.LoadScene("Flow3");
    public void Level4() => SceneManager.LoadScene("Flow4");
    public void Level5() => SceneManager.LoadScene("Flow5");
    public void Level6() => SceneManager.LoadScene("Flow6");

    public void ShowHighScore()
    {
        if (highScorePanel) highScorePanel.SetActive(true);
        if (infoPanel)      infoPanel.SetActive(false);
    }

    public void ShowInfo()
    {
        if (infoPanel)      infoPanel.SetActive(true);
        if (highScorePanel) highScorePanel.SetActive(false);
    }

    public void Backtomenu() => SceneManager.LoadScene("ModeSelect");
}
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject pausePanel;

    [Header("Win Panel Texts")]
    public TextMeshProUGUI winTimeText;
    public TextMeshProUGUI winClicksText;
    public GameObject hud;

    void Start()
    {
        winPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (winPanel.activeSelf) return;
        bool isActive = !pausePanel.activeSelf;
        pausePanel.SetActive(isActive);
        Time.timeScale = isActive ? 0f : 1f;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ZenSelect");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowWin()
{
    if (hud != null) hud.SetActive(false);

    GridManager gm = FindObjectOfType<GridManager>();
    if (gm != null)
    {
        if (winTimeText != null) winTimeText.text = gm.GetTime();
        if (winClicksText != null) winClicksText.text = $"CLICKS: {gm.GetClicks()}";
    }

    winPanel.SetActive(true);

    GameFlow flow = FindObjectOfType<GameFlow>();
    if (flow != null) flow.RefreshWinUI();
}
}
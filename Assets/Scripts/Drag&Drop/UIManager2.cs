using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager2 : MonoBehaviour
{
    [Header("Panels")]
    public GameObject winPanel;
    public GameObject pausePanel;
    public GameObject hud;

    [Header("Win Panel")]
    public TextMeshProUGUI winTimeText;
    public GameObject nextLevelButton;
    public GameObject prevLevelButton;
    public GameObject riverModeButton;
    public GameObject creekModeButton;

    void Start()
    {
        if (winPanel)   winPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);

        int level = GetCurrentLevel();

        // Next: skryť na leveli 9 (Creek koniec) a 18 (River koniec)
        if (nextLevelButton) nextLevelButton.SetActive(level != 9 && level < 18);
        // Prev: skryť na leveli 1
        if (prevLevelButton) prevLevelButton.SetActive(level > 1 && level != 10);
        // River Mode button: zobraziť len na leveli 9
        if (riverModeButton) riverModeButton.SetActive(level == 9);
        // Creek Mode button: zobraziť len na leveli 10
        if (creekModeButton) creekModeButton.SetActive(level == 10);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (winPanel != null && winPanel.activeSelf) return;
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

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        int level = GetCurrentLevel();
        if (level >= 18 || level == 9) return;
        Time.timeScale = 1f;
        SceneManager.LoadScene($"Drag&Drop{level + 1}");
    }

    public void PrevLevel()
    {
        int level = GetCurrentLevel();
        if (level <= 1 || level == 10) return;
        Time.timeScale = 1f;
        SceneManager.LoadScene($"Drag&Drop{level - 1}");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Drag&DropMenu");
    }

    public void ShowWin(string time)
    {
        if (hud != null) hud.SetActive(false);
        if (winTimeText != null) winTimeText.text = time;
        if (winPanel != null) winPanel.SetActive(true);
    }

    public int GetCurrentLevel()
    {
        string name = SceneManager.GetActiveScene().name;
        string num = name.Replace("Drag&Drop", "");
        if (int.TryParse(num, out int level))
            return level;
        return 1;
    }

    public void GoToRiverMode() => SceneManager.LoadScene("Drag&Drop10");
    public void GoToCreekMode() => SceneManager.LoadScene("Drag&Drop9");
}
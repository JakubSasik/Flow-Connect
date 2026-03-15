using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject brokenPipePanel;
    public GameObject pausePanel;

    [Header("HUD (skryje sa pri paneloch)")]
    public GameObject hud;

    [Header("Win Panel")]
    public TextMeshProUGUI winTimeText;
    public TextMeshProUGUI winEnergyText;
    public GameObject nextLevelButton;
    public GameObject prevLevelButton;

    [Header("Lose Panel")]
    public TextMeshProUGUI loseReasonText;

    [Header("Broken Pipe Panel")]
    public TextMeshProUGUI brokenPipeText;

    bool gameOver = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    void Start()
    {
        if (winPanel)        winPanel.SetActive(false);
        if (losePanel)       losePanel.SetActive(false);
        if (brokenPipePanel) brokenPipePanel.SetActive(false);
        if (pausePanel)      pausePanel.SetActive(false);

        int level = GetCurrentLevel();
        if (nextLevelButton) nextLevelButton.SetActive(level < 6);
        if (prevLevelButton) prevLevelButton.SetActive(level > 1);
    }

    // ─── WIN ─────────────────────────────────────────────────────
    public void OnWin()
    {
        if (gameOver) return;
        gameOver = true;

        // zastav čas hned pri výhre (pred zaplavením)
        FindObjectOfType<TimerManager>()?.Stop();
        FindObjectOfType<EnergyManager>()?.SetLocked(true);

        float timeLeft = FindObjectOfType<TimerManager>()?.TimeLeft ?? 0f;
        float energy   = FindObjectOfType<EnergyManager>()?.currentEnergy ?? 0f;
        int   level    = GetCurrentLevel();

        FlowHighScoreStore.SaveScore(level, timeLeft, Mathf.RoundToInt(energy));

        if (winTimeText)   winTimeText.text   = $"Čas: {FormatTime(timeLeft)}";
        if (winEnergyText) winEnergyText.text = $"Energia: {Mathf.RoundToInt(energy)}";

        if (hud) hud.SetActive(false);
        if (winPanel) winPanel.SetActive(true);
    }

    // ─── LOSE – čas vypršal ──────────────────────────────────────
    public void OnTimeOut()
    {
        if (gameOver) return;
        gameOver = true;

        FindObjectOfType<EnergyManager>()?.SetLocked(true);

        if (loseReasonText) loseReasonText.text = "Čas vypršal!";
        if (hud) hud.SetActive(false);
        if (losePanel) losePanel.SetActive(true);
    }

    // ─── LOSE – energia minula ───────────────────────────────────
    public void OnEnergyOut()
    {
        if (gameOver) return;
        gameOver = true;

        FindObjectOfType<TimerManager>()?.Stop();

        if (loseReasonText) loseReasonText.text = "Energia sa minula!";
        if (hud) hud.SetActive(false);
        if (losePanel) losePanel.SetActive(true);
    }

    // ─── BROKEN PIPE ALERT ───────────────────────────────────────
    public void ShowBrokenPipeMessage()
    {
        if (brokenPipePanel) brokenPipePanel.SetActive(true);
        Invoke(nameof(HideBrokenPipeMessage), 2f);
    }

    public void HideBrokenPipeMessage()
    {
        if (brokenPipePanel) brokenPipePanel.SetActive(false);
    }

    // ─── PAUSE ───────────────────────────────────────────────────
    public void TogglePause()
    {
        if (gameOver) return;
        bool pausing = pausePanel != null && !pausePanel.activeSelf;
        if (pausePanel) pausePanel.SetActive(pausing);
        if (hud) hud.SetActive(!pausing);
        Time.timeScale = pausing ? 0f : 1f;
    }

    public void Resume()
    {
        if (pausePanel) pausePanel.SetActive(false);
        if (hud) hud.SetActive(true);
        Time.timeScale = 1f;
    }

    // ─── BUTTONS ─────────────────────────────────────────────────
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PrevLevel()
    {
        int level = GetCurrentLevel();
        if (level <= 1) return;
        Time.timeScale = 1f;
        SceneManager.LoadScene($"Flow{level - 1}");
    }

    public void NextLevel()
    {
        int level = GetCurrentLevel();
        if (level >= 6) return;
        Time.timeScale = 1f;
        SceneManager.LoadScene($"Flow{level + 1}");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("FlowSelect");
    }

    // ─── HELPER ──────────────────────────────────────────────────
    int GetCurrentLevel()
    {
        string num = SceneManager.GetActiveScene().name.Replace("Flow", "");
        return int.TryParse(num, out int l) ? l : 1;
    }

    string FormatTime(float t)
    {
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        return $"{m:00}:{s:00}";
    }
}
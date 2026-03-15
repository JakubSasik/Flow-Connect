using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [Header("Čas (sekundy)")]
    public float timeLimit = 30f;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    float timeLeft;
    bool running = false;

    public bool IsRunning => running;
    public float TimeLeft => timeLeft;

    void Start()
    {
        timeLeft = timeLimit;
        running  = true;
        UpdateUI();
    }

    void Update()
    {
        if (!running) return;

        timeLeft -= Time.deltaTime;
        timeLeft  = Mathf.Max(0f, timeLeft);
        UpdateUI();

        if (timeLeft <= 0f)
        {
            running = false;
            FindObjectOfType<GameManager>()?.OnTimeOut();
        }
    }

    public void Stop() => running = false;

    void UpdateUI()
    {
        if (timerText == null) return;
        int m = Mathf.FloorToInt(timeLeft / 60f);
        int s = Mathf.FloorToInt(timeLeft % 60f);
        timerText.text = $"TIME  {m:00}:{s:00}";
    }
}
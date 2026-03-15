using System.Text;
using TMPro;
using UnityEngine;

public class HighScorePanelUI : MonoBehaviour
{
    public TextMeshProUGUI timeListText;
    public TextMeshProUGUI clicksListText;

    int currentDifficulty = 0;

    void OnEnable()
    {
        currentDifficulty = PlayerPrefs.GetInt("difficulty", 0);
        ShowDifficulty(currentDifficulty);
    }

    public void ShowEasy()   => ShowDifficulty(0);
    public void ShowNormal() => ShowDifficulty(1);
    public void ShowHard()   => ShowDifficulty(2);

    public void Back()
    {
        gameObject.SetActive(false);
    }

    public void ResetScores()
    {
        HighScoreStore.ResetAll();
        ShowDifficulty(currentDifficulty);
    }

    void ShowDifficulty(int d)
    {
        currentDifficulty = d;

        var times = HighScoreStore.LoadTimes(d);
        var clicks = HighScoreStore.LoadClicks(d);

        if (timeListText != null)
            timeListText.text = BuildTimeColumn(times);

        if (clicksListText != null)
            clicksListText.text = BuildClicksColumn(clicks);
    }

    string BuildTimeColumn(System.Collections.Generic.List<float> times)
    {
        var sb = new StringBuilder();
        sb.AppendLine("TIME");
        for (int i = 0; i < 10; i++)
        {
            if (i < times.Count) sb.AppendLine($"{i + 1}. {FormatTime(times[i])}");
            else sb.AppendLine($"{i + 1}. --:--");
        }
        return sb.ToString();
    }

    string BuildClicksColumn(System.Collections.Generic.List<int> clicks)
    {
        var sb = new StringBuilder();
        sb.AppendLine("CLICKS");
        for (int i = 0; i < 10; i++)
        {
            if (i < clicks.Count) sb.AppendLine($"{i + 1}. {clicks[i]}");
            else sb.AppendLine($"{i + 1}. -");
        }
        return sb.ToString();
    }

    string FormatTime(float t)
    {
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        return $"{m:00}:{s:00}";
    }
}
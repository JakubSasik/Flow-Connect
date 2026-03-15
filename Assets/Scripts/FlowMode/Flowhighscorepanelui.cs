using System.Text;
using TMPro;
using UnityEngine;

public class FlowHighScorePanelUI : MonoBehaviour
{
    public TextMeshProUGUI timeListText;
    public TextMeshProUGUI energyListText;

    int currentLevel = 1;

    void OnEnable() => ShowLevel(1);

    public void ShowLevel1() => ShowLevel(1);
    public void ShowLevel2() => ShowLevel(2);
    public void ShowLevel3() => ShowLevel(3);
    public void ShowLevel4() => ShowLevel(4);
    public void ShowLevel5() => ShowLevel(5);
    public void ShowLevel6() => ShowLevel(6);

    public void Back() => gameObject.SetActive(false);

    public void ResetScores()
    {
        FlowHighScoreStore.ResetAll();
        ShowLevel(currentLevel);
    }

    void ShowLevel(int level)
    {
        currentLevel = level;
        var scores = FlowHighScoreStore.LoadScores(level);

        if (timeListText != null)   timeListText.text   = BuildTimeColumn(scores);
        if (energyListText != null) energyListText.text = BuildEnergyColumn(scores);
    }

    string BuildTimeColumn(System.Collections.Generic.List<(float timeLeft, int energy)> scores)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"── Level {currentLevel} ──\nTIME LEFT");
        for (int i = 0; i < 10; i++)
        {
            if (i < scores.Count) sb.AppendLine($"{i + 1}. {FormatTime(scores[i].timeLeft)}");
            else sb.AppendLine($"{i + 1}. --:--");
        }
        return sb.ToString();
    }

    string BuildEnergyColumn(System.Collections.Generic.List<(float timeLeft, int energy)> scores)
    {
        var sb = new StringBuilder();
        sb.AppendLine("ENERGIA");
        for (int i = 0; i < 10; i++)
        {
            if (i < scores.Count) sb.AppendLine($"{i + 1}. {scores[i].energy}");
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
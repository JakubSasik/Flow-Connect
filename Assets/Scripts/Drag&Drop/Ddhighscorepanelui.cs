using System.Text;
using TMPro;
using UnityEngine;

public class DDHighScorePanelUI : MonoBehaviour
{
    [Header("9 textových polí (každé = 1 level, top 10 časov)")]
    public TextMeshProUGUI level1Text;
    public TextMeshProUGUI level2Text;
    public TextMeshProUGUI level3Text;
    public TextMeshProUGUI level4Text;
    public TextMeshProUGUI level5Text;
    public TextMeshProUGUI level6Text;
    public TextMeshProUGUI level7Text;
    public TextMeshProUGUI level8Text;
    public TextMeshProUGUI level9Text;

    int modeOffset = 1; // 1 = Creek, 10 = River

    void OnEnable()
    {
        ShowCreek();
    }

    public void ShowCreek()
    {
        modeOffset = 1;
        Refresh();
    }

    public void ShowRiver()
    {
        modeOffset = 10;
        Refresh();
    }

    void Refresh()
    {
        SetText(level1Text, modeOffset + 0);
        SetText(level2Text, modeOffset + 1);
        SetText(level3Text, modeOffset + 2);
        SetText(level4Text, modeOffset + 3);
        SetText(level5Text, modeOffset + 4);
        SetText(level6Text, modeOffset + 5);
        SetText(level7Text, modeOffset + 6);
        SetText(level8Text, modeOffset + 7);
        SetText(level9Text, modeOffset + 8);
    }

    void SetText(TextMeshProUGUI tmp, int level)
    {
        if (tmp == null) return;

        var times = DDHighScoreStore.LoadTimes(level);

        int displayNum;
        if (level <= 9) displayNum = level;       // Creek 1-9
        else displayNum = level - 9;              // River 10-18 -> zobraz ako 1-9

        var sb = new StringBuilder();
        sb.AppendLine($"── Level {displayNum} ──");

        for (int i = 0; i < 10; i++)
        {
            if (i < times.Count)
                sb.AppendLine($"{i + 1}. {FormatTime(times[i])}");
            else
                sb.AppendLine($"{i + 1}. --:--");
        }

        tmp.text = sb.ToString();
    }

    public void ResetAll()
    {
        DDHighScoreStore.ResetAll();
        Refresh();
    }

    public void Back()
    {
        gameObject.SetActive(false);
    }

    string FormatTime(float t)
    {
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        return $"{m:00}:{s:00}";
    }
}
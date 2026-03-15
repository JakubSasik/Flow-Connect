using TMPro;
using UnityEngine;

public class InfoPanelUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject infoPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    void OnEnable() => SetSlovak();

    public void Close() => infoPanel.SetActive(false);

    public void SetSlovak()
    {
        if (titleText) titleText.text = "ZEN MODE";
        if (descriptionText) descriptionText.text =
            "🎮 Ovládanie:\n" +
            "Ľavé tlačidlo myši – otočenie potrubia\n\n" +
            "📋 Obtiažnosti:\n" +
            "Easy – malý grid\n" +
            "Normal – stredný grid\n" +
            "Hard – veľký grid\n\n" +
            "🔀 Vždy existuje viac riešení:\n" +
            "Potrubia môžeš spojiť viacerými spôsobmi.\n\n" +
            "↩️ Štart a cieľ:\n" +
            "Aj štartové a cieľové potrubie sa dajú otáčať,\n" +
            "no zostávajú vždy na okraji gridu.\n\n" +
            "😌 Bez časového tlaku:\n" +
            "Najlepší čas a počet klikov sa uložia ako high score.";
    }

    public void SetEnglish()
    {
        if (titleText) titleText.text = "ZEN MODE";
        if (descriptionText) descriptionText.text =
            "🎮 Controls:\n" +
            "Left mouse button – rotate pipe\n\n" +
            "📋 Difficulties:\n" +
            "Easy – small grid\n" +
            "Normal – medium grid\n" +
            "Hard – large grid\n\n" +
            "🔀 Multiple solutions exist:\n" +
            "There is always more than one way to connect the pipes.\n\n" +
            "↩️ Start and end:\n" +
            "Start and end pipes can also be rotated,\n" +
            "but always stay on the edge of the grid.\n\n" +
            "😌 No time pressure:\n" +
            "Best time and click count saved as high score.";
    }
}
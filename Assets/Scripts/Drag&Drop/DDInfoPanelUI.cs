using TMPro;
using UnityEngine;

public class DDInfoPanelUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject infoPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    void Start()
    {
    if (PlayerPrefs.GetInt("DDInfoShown", 0) == 0)
    {
        infoPanel.SetActive(true);
        PlayerPrefs.SetInt("DDInfoShown", 1);
    }
    else
    {
        infoPanel.SetActive(false);
    }

    SetSlovak();
    }

    void OnEnable() => SetSlovak();

    public void Close() => infoPanel.SetActive(false);

    public void SetSlovak()
    {
        if (titleText) titleText.text = "DRAG & DROP";
        if (descriptionText) descriptionText.text =
            "🎮 Ovládanie:\n" +
            "Ľavé tlačidlo myši – presun potrubia\n" +
            "Pravé tlačidlo myši – otočenie potrubia\n\n" +
            "📋 Levely:\n" +
            "9 levelov v každom móde s rastúcou obtiažnosťou\n" +
            "Levely 1–5: jednofarebné potrubia\n" +
            "Levely 6–9: viacfarebné potrubia\n\n" +
            "🏞️ Creek Mode:\n" +
            "Jedna cesta od štartu po cieľ.\n" +
            "Musíš využiť všetky potrubia – inak výhra nepríde!\n\n" +
            "🌊 River Mode:\n" +
            "Viac cieľov, zložitejšia sieť.\n" +
            "Stačí spojiť všetky ciele – nevyužité dieliky nevadia.\n\n" +
            "🏆 Najlepší čas každého levelu sa uloží ako high score.";
    }

    public void SetEnglish()
    {
        if (titleText) titleText.text = "DRAG & DROP";
        if (descriptionText) descriptionText.text =
            "🎮 Controls:\n" +
            "Left mouse button – drag pipe\n" +
            "Right mouse button – rotate pipe\n\n" +
            "📋 Levels:\n" +
            "9 levels in each mode with increasing difficulty\n" +
            "Levels 1–5: single color pipes\n" +
            "Levels 6–9: multicolor pipes\n\n" +
            "🏞️ Creek Mode:\n" +
            "Single path from start to end.\n" +
            "You must use all pipes – otherwise no win!\n\n" +
            "🌊 River Mode:\n" +
            "Multiple endpoints, more complex network.\n" +
            "Just connect all endpoints – unused pieces are fine.\n\n" +
            "🏆 Your best time for each level saved as high score.";
    }
}
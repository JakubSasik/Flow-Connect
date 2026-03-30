using TMPro;
using UnityEngine;

public class FlowInfoPanelUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject infoPanel;
    public TextMeshProUGUI descriptionText;

    void Start()
    {
    if (PlayerPrefs.GetInt("FlowInfoShown", 0) == 0)
    {
        infoPanel.SetActive(true);
        PlayerPrefs.SetInt("FlowInfoShown", 1);
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
        if (descriptionText) descriptionText.text =
            "🎮 Ovládanie:\n" +
            "Ľavé tlačidlo myši – otočenie potrubia\n\n" +
            "📋 Levely:\n" +
            "6 levelov s rastúcou zložitosťou\n\n" +
            "🎨 Farebné potrubia:\n" +
            "Od levelu 3 sa objavujú rôznofarebné potrubia.\n" +
            "Každá farba musí tvoriť vlastnú spojenú cestu.\n\n" +
            "💥 Pokazené potrubia:\n" +
            "Niektoré levely obsahujú pokazené potrubia.\n" +
            "Ak na také klikneš, poruší sa – musíš nájsť inú cestu!\n\n" +
            "⏱ Čas a energia:\n" +
            "Každý level má vlastný čas a energiu.\n" +
            "Spoj všetky cesty skôr, než vyprší čas alebo energia.";
    }

    public void SetEnglish()
    {
        if (descriptionText) descriptionText.text =
            "🎮 Controls:\n" +
            "Left mouse button – rotate pipe\n\n" +
            "📋 Levels:\n" +
            "6 levels with increasing complexity\n\n" +
            "🎨 Colored pipes:\n" +
            "From level 3, different colored pipes appear.\n" +
            "Each color must form its own connected path.\n\n" +
            "💥 Broken pipes:\n" +
            "Some levels contain broken pipes.\n" +
            "If you click one, it breaks – you must find another route!\n\n" +
            "⏱ Time and energy:\n" +
            "Each level has its own time and energy limit.\n" +
            "Connect all paths before time or energy runs out.";
    }
}
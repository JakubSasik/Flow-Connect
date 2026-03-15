using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ModeMenu : MonoBehaviour
{
    [Header("Info Panel")]
    public GameObject infoPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    private bool isSlovak = true;
    private int currentMode = 0; // 0=Zen, 1=Flow, 2=DD

    public void Zen() => SceneManager.LoadScene("ZenSelect");
    public void Flow() => SceneManager.LoadScene("FlowSelect");
    public void DragandDrop() => SceneManager.LoadScene("Drag&DropMenu");
    public void BackToTitle() => SceneManager.LoadScene("Menu");

    public void OpenInfo()
    {
        currentMode = 0;
        infoPanel.SetActive(true);
        RefreshText();
    }

    public void CloseInfo() => infoPanel.SetActive(false);

    public void SetSlovak() { isSlovak = true; RefreshText(); }
    public void SetEnglish() { isSlovak = false; RefreshText(); }

    public void ShowInfoZen() { currentMode = 0; RefreshText(); }
    public void ShowInfoFlow() { currentMode = 1; RefreshText(); }
    public void ShowInfoDragDrop() { currentMode = 2; RefreshText(); }

    void RefreshText()
    {
        if (currentMode == 0) { if (isSlovak) ShowZenSK(); else ShowZenEN(); }
        else if (currentMode == 1) { if (isSlovak) ShowFlowSK(); else ShowFlowEN(); }
        else if (currentMode == 2) { if (isSlovak) ShowDragDropSK(); else ShowDragDropEN(); }
    }

    void ShowZenSK()
    {
        if (titleText) titleText.text = "ZEN MODE";
        if (descriptionText) descriptionText.text =
            "🧘 Oddychový mód bez časového tlaku.\n\n" +
            "Otáčaj potrubia a spoj štart s cieľom.\n" +
            "Keď je cesta správna, potrubím pretečie voda.\n\n" +
            "Pre každú obtiažnosť sa vždy vygeneruje\n" +
            "nový originálny level – môžeš hrať donekonečna!";
    }

    void ShowZenEN()
    {
        if (titleText) titleText.text = "ZEN MODE";
        if (descriptionText) descriptionText.text =
            "🧘 Relaxing mode with no time pressure.\n\n" +
            "Rotate pipes and connect start to end.\n" +
            "When the path is correct, water flows through.\n\n" +
            "Every difficulty generates a brand new level –\n" +
            "play as many times as you want!";
    }

    void ShowFlowSK()
    {
        if (titleText) titleText.text = "FLOW MODE";
        if (descriptionText) descriptionText.text =
            "💧 Spoj potrubia od štartu po všetky ciele.\n\n" +
            "Otáčaj dieliky a vytvor správnu cestu pre každú farbu.\n" +
            "Dávaj pozor na pokazené potrubia – ak na ne klikneš,\n" +
            "poškodia sa a musíš nájsť inú cestu!\n\n" +
            "Každý level má obmedzený čas a energiu.\n" +
            "Ak vyprší čas alebo energia, prehrávaš.";
    }

    void ShowFlowEN()
    {
        if (titleText) titleText.text = "FLOW MODE";
        if (descriptionText) descriptionText.text =
            "💧 Connect pipes from start to all endpoints.\n\n" +
            "Rotate pieces and create the correct path for each color.\n" +
            "Watch out for broken pipes – if you click one,\n" +
            "it breaks and you must find another route!\n\n" +
            "Each level has a limited time and energy.\n" +
            "If time or energy runs out, you lose.";
    }

    void ShowDragDropSK()
    {
        if (titleText) titleText.text = "DRAG & DROP";
        if (descriptionText) descriptionText.text =
            "🔧 Presúvaj potrubia na správne miesto a spoj cestu.\n\n" +
            "Creek Mode – spoj štart s cieľom a využi všetky dieliky.\n" +
            "River Mode – spoj všetky ciele, nevyužité dieliky nevadia.\n\n" +
            "Potrubia ťaháš ľavým tlačidlom myši,\n" +
            "otáčaš ich pravým tlačidlom myši.";
    }

    void ShowDragDropEN()
    {
        if (titleText) titleText.text = "DRAG & DROP";
        if (descriptionText) descriptionText.text =
            "🔧 Drag pipes into position and connect the path.\n\n" +
            "Creek Mode – connect start to end using all pieces.\n" +
            "River Mode – connect all endpoints, unused pieces are fine.\n\n" +
            "Drag pipes with left mouse button,\n" +
            "rotate them with right mouse button.";
    }
}
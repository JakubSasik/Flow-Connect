using UnityEngine;
using UnityEngine.SceneManagement;

public class DDMode : MonoBehaviour
{
    [Header("Panels")]
    public GameObject highScorePanel;
    public GameObject infoPanel;

    void Start()
    {
        
        if (highScorePanel) highScorePanel.SetActive(false);
        if (infoPanel)      infoPanel.SetActive(true);
    }

    public void LoadCreek1() => SceneManager.LoadScene("Drag&Drop1");
    public void LoadCreek2() => SceneManager.LoadScene("Drag&Drop2");
    public void LoadCreek3() => SceneManager.LoadScene("Drag&Drop3");
    public void LoadCreek4() => SceneManager.LoadScene("Drag&Drop4");
    public void LoadCreek5() => SceneManager.LoadScene("Drag&Drop5");
    public void LoadCreek6() => SceneManager.LoadScene("Drag&Drop6");
    public void LoadCreek7() => SceneManager.LoadScene("Drag&Drop7");
    public void LoadCreek8() => SceneManager.LoadScene("Drag&Drop8");
    public void LoadCreek9() => SceneManager.LoadScene("Drag&Drop9");

    public void LoadRiver1() => SceneManager.LoadScene("Drag&Drop10");
    public void LoadRiver2() => SceneManager.LoadScene("Drag&Drop11");
    public void LoadRiver3() => SceneManager.LoadScene("Drag&Drop12");
    public void LoadRiver4() => SceneManager.LoadScene("Drag&Drop13");
    public void LoadRiver5() => SceneManager.LoadScene("Drag&Drop14");
    public void LoadRiver6() => SceneManager.LoadScene("Drag&Drop15");
    public void LoadRiver7() => SceneManager.LoadScene("Drag&Drop16");
    public void LoadRiver8() => SceneManager.LoadScene("Drag&Drop17");
    public void LoadRiver9() => SceneManager.LoadScene("Drag&Drop18");

    public void ShowHighScore()
    {
        if (highScorePanel) highScorePanel.SetActive(true);
        if (infoPanel)      infoPanel.SetActive(false);
    }

    public void HideHighScore()
    {
        if (highScorePanel) highScorePanel.SetActive(false);
    }

    public void ShowInfo()
    {
        if (infoPanel)      infoPanel.SetActive(true);
        if (highScorePanel) highScorePanel.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("ModeSelect");
    }
}
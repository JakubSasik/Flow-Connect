using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("ModeSelect");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game exited");
    }
}

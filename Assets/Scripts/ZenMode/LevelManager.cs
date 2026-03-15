using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string nextLevel; 
    public string levelKey; 

    public void CompleteLevel()
    {
        if (!string.IsNullOrEmpty(levelKey))
        {
            PlayerPrefs.SetInt(levelKey, 1); 
        }

        if (!string.IsNullOrEmpty(nextLevel))
        {
            SceneManager.LoadScene(nextLevel); 
        }
        else
        {
            SceneManager.LoadScene("Zen_LevelSelect"); 
        }
    }
}

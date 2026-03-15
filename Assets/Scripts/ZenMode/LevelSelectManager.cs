using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public Button[] levelButtons;
    public string[] levelSceneNames; 

    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("ZenLevelReached", 1); 

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
                levelButtons[i].interactable = false; 
            else
                levelButtons[i].interactable = true;  

            int index = i; 
            levelButtons[i].onClick.AddListener(() => LoadLevel(index));
        }
    }

    void LoadLevel(int index)
    {
        SceneManager.LoadScene(levelSceneNames[index]);
    }
}

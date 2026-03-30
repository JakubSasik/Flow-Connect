using UnityEngine;

public class GameStartReset : MonoBehaviour
{
    void Awake()
    {
        PlayerPrefs.DeleteKey("ZenInfoShown");
        PlayerPrefs.DeleteKey("DDInfoShown");
        PlayerPrefs.DeleteKey("FlowInfoShown");
    }
}
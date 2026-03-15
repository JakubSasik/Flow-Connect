using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    void Start()
    {
        int d = PlayerPrefs.GetInt("difficulty", 0);

        switch (d)
        {
            case 0: // Easy
                transform.localScale = new Vector3(2.4f, 1f, 1f);
                break;

            case 1: // Normal
                transform.localScale = new Vector3(2.8f, 1.1f, 1f);
                break;

            case 2: // Hard
                transform.localScale = new Vector3(3.5f, 1.4f, 1f);
                break;
        }
    }
}
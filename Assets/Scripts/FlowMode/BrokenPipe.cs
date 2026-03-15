using UnityEngine;
using UnityEngine.EventSystems;

public class BrokenPipe : MonoBehaviour
{
    public float[] correctRotation;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        SpawnWrongRotation();
    }

    private void SpawnWrongRotation()
    {
        float[] allRotations = { 0f, 90f, 180f, 270f };

        var wrongRotations = new System.Collections.Generic.List<float>();
        foreach (float rot in allRotations)
        {
            bool isCorrect = false;
            foreach (float correct in correctRotation)
            {
                if (Mathf.Abs(rot - correct) < 0.1f) { isCorrect = true; break; }
            }
            if (!isCorrect) wrongRotations.Add(rot);
        }

        float chosen = wrongRotations.Count > 0
            ? wrongRotations[Random.Range(0, wrongRotations.Count)]
            : (correctRotation[0] + 90f) % 360f;

        transform.rotation = Quaternion.Euler(0, 0, chosen);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        if (Mathf.Approximately(Time.timeScale, 0f)) return;
        if (Pipe2.InputLocked) return;

        if (sr != null) sr.color = Color.red;
        FindObjectOfType<GameManager>()?.ShowBrokenPipeMessage();
    }
}
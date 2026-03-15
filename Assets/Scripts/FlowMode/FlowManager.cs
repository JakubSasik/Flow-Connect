using UnityEngine;
using System.Collections;

public class FlowManager : MonoBehaviour
{
    [Header("Správna cesta (bez broken pipes)")]
    public Pipe2[] correctPath;

    [Header("Zaplavenie")]
    public float waterStepDelay = 0.08f;
    public Color waterColor = Color.cyan;

    [Header("Audio")]
    public AudioClip waterClip;
    private AudioSource audioSource;

    bool flooding = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void CheckWin()
    {
        if (flooding) return;

        foreach (Pipe2 pipe in correctPath)
        {
            if (pipe == null) continue;
            if (!pipe.IsPlaced()) return;
        }

        flooding = true;
        Pipe2.InputLocked = true;
        StartCoroutine(FloodAndWin());
    }

    IEnumerator FloodAndWin()
    {
        Pipe2.InputLocked = true;
        FindObjectOfType<TimerManager>()?.Stop();
        FindObjectOfType<EnergyManager>()?.SetLocked(true);

        // spusti zvuk vody
        if (audioSource != null && waterClip != null)
        {
            audioSource.clip = waterClip;
            audioSource.Play();
        }

        foreach (Pipe2 pipe in correctPath)
        {
            if (pipe == null) continue;
            SpriteRenderer sr = pipe.GetComponent<SpriteRenderer>();
            if (sr) sr.color = waterColor;
            yield return new WaitForSeconds(waterStepDelay);
        }

        // zastav zvuk
        if (audioSource != null)
            audioSource.Stop();

        FindObjectOfType<GameManager>()?.OnWin();
    }
}
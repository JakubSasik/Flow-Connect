using UnityEngine;
using System.Collections;

public class FlowManager4Color : MonoBehaviour
{
    [Header("Spoločná cesta (Start → Cross1 vrátane)")]
    public Pipe2[] sharedPath;

    [Header("Modrá vetva (Cross1 → Cross2 vrátane)")]
    public Pipe2[] blueToross2Path;

    [Header("Červená vetva (Cross1 → Cross2 vrátane)")]
    public Pipe2[] redToCross2Path;

    [Header("Žltá cesta (Cross1 → T-cross vrátane)")]
    public Pipe2[] yellowSharedPath;

    [Header("Modrá → End")]
    public Pipe2[] bluePath;

    [Header("Červená → End")]
    public Pipe2[] redPath;

    [Header("Žltá → End")]
    public Pipe2[] yellowPath;

    [Header("Zelená → End")]
    public Pipe2[] greenPath;

    [Header("Zaplavenie")]
    public float waterStepDelay = 0.08f;
    public Color blueWaterColor   = Color.cyan;
    public Color redWaterColor    = new Color(1f, 0.3f, 0.3f);
    public Color yellowWaterColor = new Color(1f, 0.9f, 0.2f);
    public Color greenWaterColor  = new Color(0.3f, 1f, 0.3f);

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
        foreach (Pipe2 pipe in sharedPath)      { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in blueToross2Path) { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in redToCross2Path) { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in yellowSharedPath){ if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in bluePath)        { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in redPath)         { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in yellowPath)      { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in greenPath)       { if (pipe != null && !pipe.IsPlaced()) return; }
        flooding = true;
        Pipe2.InputLocked = true;
        StartCoroutine(FloodAndWin());
    }

    IEnumerator FloodAndWin()
    {
        FindObjectOfType<TimerManager>()?.Stop();
        FindObjectOfType<EnergyManager>()?.SetLocked(true);

        if (audioSource != null && waterClip != null)
        {
            audioSource.clip = waterClip;
            audioSource.Play();
        }

        foreach (Pipe2 pipe in sharedPath)
        {
            if (pipe == null) continue;
            SetColor(pipe, blueWaterColor);
            yield return new WaitForSeconds(waterStepDelay);
        }

        int maxLen1 = Mathf.Max(blueToross2Path.Length, Mathf.Max(redToCross2Path.Length, yellowSharedPath.Length));
        for (int i = 0; i < maxLen1; i++)
        {
            if (i < blueToross2Path.Length && blueToross2Path[i] != null)  SetColor(blueToross2Path[i], blueWaterColor);
            if (i < redToCross2Path.Length && redToCross2Path[i] != null)  SetColor(redToCross2Path[i], redWaterColor);
            if (i < yellowSharedPath.Length && yellowSharedPath[i] != null) SetColor(yellowSharedPath[i], yellowWaterColor);
            yield return new WaitForSeconds(waterStepDelay);
        }

        int maxLen2 = Mathf.Max(bluePath.Length, Mathf.Max(redPath.Length, Mathf.Max(yellowPath.Length, greenPath.Length)));
        for (int i = 0; i < maxLen2; i++)
        {
            if (i < bluePath.Length && bluePath[i] != null)     SetColor(bluePath[i], blueWaterColor);
            if (i < redPath.Length && redPath[i] != null)       SetColor(redPath[i], redWaterColor);
            if (i < yellowPath.Length && yellowPath[i] != null) SetColor(yellowPath[i], yellowWaterColor);
            if (i < greenPath.Length && greenPath[i] != null)   SetColor(greenPath[i], greenWaterColor);
            yield return new WaitForSeconds(waterStepDelay);
        }

        if (audioSource != null) audioSource.Stop();
        FindObjectOfType<GameManager>()?.OnWin();
    }

    void SetColor(Pipe2 pipe, Color color)
    {
        SpriteRenderer sr = pipe.GetComponent<SpriteRenderer>();
        if (sr) sr.color = color;
    }
}
using UnityEngine;
using System.Collections;

public class FlowManager6 : MonoBehaviour
{
    [Header("Spoločná cesta (Start → T-cross1 vrátane)")]
    public Pipe2[] sharedPath;

    [Header("Zelená → End")]
    public Pipe2[] greenPath;

    [Header("Žltá (T-cross1 → T-cross2 vrátane)")]
    public Pipe2[] yellowToTcross2;

    [Header("Červená (T-cross2 → Cross vrátane)")]
    public Pipe2[] redToCross;

    [Header("Modrá vetva 1 → End")]
    public Pipe2[] bluePath1;

    [Header("Modrá vetva 2 → End")]
    public Pipe2[] bluePath2;

    [Header("Červená (Cross → T-cross3 vrátane)")]
    public Pipe2[] redToTcross3;

    [Header("Žltá → End (z T-cross3)")]
    public Pipe2[] yellowPath2;

    [Header("Červená → End (z T-cross3)")]
    public Pipe2[] redPath;

    [Header("Zaplavenie")]
    public float waterStepDelay = 0.08f;
    public Color yellowWaterColor = new Color(1f, 0.9f, 0.2f);
    public Color greenWaterColor  = new Color(0.3f, 1f, 0.3f);
    public Color redWaterColor    = new Color(1f, 0.3f, 0.3f);
    public Color blueWaterColor   = Color.cyan;

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
        foreach (Pipe2 pipe in greenPath)       { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in yellowToTcross2) { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in redToCross)      { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in bluePath1)       { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in bluePath2)       { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in redToTcross3)    { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in yellowPath2)     { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in redPath)         { if (pipe != null && !pipe.IsPlaced()) return; }
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
            SetColor(pipe, yellowWaterColor);
            yield return new WaitForSeconds(waterStepDelay);
        }

        int max1 = Mathf.Max(greenPath.Length, yellowToTcross2.Length);
        for (int i = 0; i < max1; i++)
        {
            if (i < greenPath.Length && greenPath[i] != null)           SetColor(greenPath[i], greenWaterColor);
            if (i < yellowToTcross2.Length && yellowToTcross2[i] != null) SetColor(yellowToTcross2[i], yellowWaterColor);
            yield return new WaitForSeconds(waterStepDelay);
        }

        foreach (Pipe2 pipe in redToCross)
        {
            if (pipe == null) continue;
            SetColor(pipe, redWaterColor);
            yield return new WaitForSeconds(waterStepDelay);
        }

        int max3 = Mathf.Max(bluePath1.Length, Mathf.Max(bluePath2.Length, redToTcross3.Length));
        for (int i = 0; i < max3; i++)
        {
            if (i < bluePath1.Length && bluePath1[i] != null)       SetColor(bluePath1[i], blueWaterColor);
            if (i < bluePath2.Length && bluePath2[i] != null)       SetColor(bluePath2[i], blueWaterColor);
            if (i < redToTcross3.Length && redToTcross3[i] != null) SetColor(redToTcross3[i], redWaterColor);
            yield return new WaitForSeconds(waterStepDelay);
        }

        int max4 = Mathf.Max(yellowPath2.Length, redPath.Length);
        for (int i = 0; i < max4; i++)
        {
            if (i < yellowPath2.Length && yellowPath2[i] != null) SetColor(yellowPath2[i], yellowWaterColor);
            if (i < redPath.Length && redPath[i] != null)         SetColor(redPath[i], redWaterColor);
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
using UnityEngine;
using System.Collections;

public class FlowManager3Color : MonoBehaviour
{
    [Header("Spoločná cesta (Start → T-cross1 vrátane)")]
    public Pipe2[] sharedPath;

    [Header("Modrá vetva (T-cross1 → End1)")]
    public Pipe2[] bluePath;

    [Header("Červená cesta (T-cross1 → T-cross2 vrátane)")]
    public Pipe2[] redSharedPath;

    [Header("Červená vetva (T-cross2 → End2)")]
    public Pipe2[] redPath;

    [Header("Žltá vetva (T-cross2 → End3)")]
    public Pipe2[] yellowPath;

    [Header("Zaplavenie")]
    public float waterStepDelay = 0.08f;
    public Color sharedWaterColor = Color.cyan;
    public Color blueWaterColor   = Color.cyan;
    public Color redWaterColor    = new Color(1f, 0.3f, 0.3f);
    public Color yellowWaterColor = new Color(1f, 0.9f, 0.2f);

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
        foreach (Pipe2 pipe in sharedPath)    { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in bluePath)      { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in redSharedPath) { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in redPath)       { if (pipe != null && !pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in yellowPath)    { if (pipe != null && !pipe.IsPlaced()) return; }
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
            SpriteRenderer sr = pipe.GetComponent<SpriteRenderer>();
            if (sr) sr.color = sharedWaterColor;
            yield return new WaitForSeconds(waterStepDelay);
        }

        int maxLen1 = Mathf.Max(bluePath.Length, redSharedPath.Length);
        for (int i = 0; i < maxLen1; i++)
        {
            if (i < bluePath.Length && bluePath[i] != null)
            {
                SpriteRenderer sr = bluePath[i].GetComponent<SpriteRenderer>();
                if (sr) sr.color = blueWaterColor;
            }
            if (i < redSharedPath.Length && redSharedPath[i] != null)
            {
                SpriteRenderer sr = redSharedPath[i].GetComponent<SpriteRenderer>();
                if (sr) sr.color = redWaterColor;
            }
            yield return new WaitForSeconds(waterStepDelay);
        }

        int maxLen2 = Mathf.Max(redPath.Length, yellowPath.Length);
        for (int i = 0; i < maxLen2; i++)
        {
            if (i < redPath.Length && redPath[i] != null)
            {
                SpriteRenderer sr = redPath[i].GetComponent<SpriteRenderer>();
                if (sr) sr.color = redWaterColor;
            }
            if (i < yellowPath.Length && yellowPath[i] != null)
            {
                SpriteRenderer sr = yellowPath[i].GetComponent<SpriteRenderer>();
                if (sr) sr.color = yellowWaterColor;
            }
            yield return new WaitForSeconds(waterStepDelay);
        }

        if (audioSource != null) audioSource.Stop();
        FindObjectOfType<GameManager>()?.OnWin();
    }
}
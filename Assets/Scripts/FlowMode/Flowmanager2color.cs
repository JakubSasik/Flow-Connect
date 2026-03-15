using UnityEngine;
using System.Collections;

public class FlowManager2Color : MonoBehaviour
{
    [Header("Spoločná cesta (od startu po T-cross vrátane)")]
    public Pipe2[] sharedPath;

    [Header("Modrá vetva (po T-cross)")]
    public Pipe2[] bluePath;

    [Header("Červená vetva (po T-cross)")]
    public Pipe2[] redPath;

    [Header("Zaplavenie")]
    public float waterStepDelay = 0.08f;
    public Color sharedWaterColor = Color.cyan;
    public Color blueWaterColor   = Color.cyan;
    public Color redWaterColor    = new Color(1f, 0.3f, 0.3f);

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
        foreach (Pipe2 pipe in sharedPath) { if (pipe == null) continue; if (!pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in bluePath)   { if (pipe == null) continue; if (!pipe.IsPlaced()) return; }
        foreach (Pipe2 pipe in redPath)    { if (pipe == null) continue; if (!pipe.IsPlaced()) return; }
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

        int maxLen = Mathf.Max(bluePath.Length, redPath.Length);
        for (int i = 0; i < maxLen; i++)
        {
            if (i < bluePath.Length && bluePath[i] != null)
            {
                SpriteRenderer sr = bluePath[i].GetComponent<SpriteRenderer>();
                if (sr) sr.color = blueWaterColor;
            }
            if (i < redPath.Length && redPath[i] != null)
            {
                SpriteRenderer sr = redPath[i].GetComponent<SpriteRenderer>();
                if (sr) sr.color = redWaterColor;
            }
            yield return new WaitForSeconds(waterStepDelay);
        }

        if (audioSource != null) audioSource.Stop();
        FindObjectOfType<GameManager>()?.OnWin();
    }
}
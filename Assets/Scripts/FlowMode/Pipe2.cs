using UnityEngine;
using UnityEngine.EventSystems;

public class Pipe2 : MonoBehaviour
{
    [Header("Rotácie")]
    private readonly float[] rotations = { 0, 90, 180, 270 };
    public float[] correctRotation;

    [Header("Flow logika")]

    private bool isPlaced = false;
    private bool isBroken = false;

    public static bool InputLocked = false;

    private SpriteRenderer sr;

    void Start()
    {
        InputLocked = false; // reset pri každom načítaní scény
        sr = GetComponent<SpriteRenderer>();
        int rand = Random.Range(0, rotations.Length);
        transform.rotation = Quaternion.Euler(0, 0, rotations[rand]);
        CheckRotation();
    }

    private void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        if (Mathf.Approximately(Time.timeScale, 0f)) return;
        if (InputLocked) return;

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm == null) return;

        EnergyManager em = FindObjectOfType<EnergyManager>();
        if (em != null)
        {
            if (em.currentEnergy <= 0)
            {
                Debug.Log("Nemáš dosť energie!");
                return;
            }
            em.UseEnergy(1f);
        }

        transform.Rotate(0, 0, 90);
        CheckRotation();

        FlowManager fm = FindObjectOfType<FlowManager>();
        if (fm != null) fm.CheckWin();

        FlowManager2Color fm2 = FindObjectOfType<FlowManager2Color>();
        if (fm2 != null) fm2.CheckWin();

        FlowManager3Color fm3 = FindObjectOfType<FlowManager3Color>();
        if (fm3 != null) fm3.CheckWin();

        FlowManager4Color fm4 = FindObjectOfType<FlowManager4Color>();
        if (fm4 != null) fm4.CheckWin();

        FlowManager6 fm6 = FindObjectOfType<FlowManager6>();
        if (fm6 != null) fm6.CheckWin();
    }

    public void CheckRotation()
    {
        float currentZ = Mathf.Round(transform.eulerAngles.z) % 360;
        isPlaced = false;

        foreach (float correct in correctRotation)
        {
            if (Mathf.Abs(currentZ - correct) < 0.1f)
            {
                isPlaced = true;
                break;
            }
        }
    }

    public void BreakPipe()
    {
        isBroken = true;
        if (sr != null) sr.color = Color.red;
    }

    public bool IsPlaced() => isPlaced;
    public bool IsBroken() => isBroken;
}
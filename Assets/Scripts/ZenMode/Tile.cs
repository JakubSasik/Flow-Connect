using UnityEngine;

public enum PipeType { Straight, Curve, TCross, Cross, StartEnd }
public enum Direction { Up = 0, Right = 1, Down = 2, Left = 3 }

public class Tile : MonoBehaviour
{
    public PipeType pipeType;
    [HideInInspector] public bool[] connections = new bool[4];

    private SpriteRenderer sr;
    private Collider2D col;

    [HideInInspector] public int currentRotation = 0;

    [Header("--- PIPES ASSETS ---")]
    public Sprite straightSprite;
    public Sprite curveSprite;
    public Sprite tCrossSprite;
    public Sprite crossSprite;
    public Sprite startEndSprite;

    private bool isFixed = false;        // true = Start/End, nedá sa otočiť vôbec
    private bool isRestricted = false;
    private int rotA, rotB;

    private bool[] straightBase = { false, true, false, true };
    private bool[] curveBase    = { true, true, false, false };
    private bool[] tCrossBase   = { false, true, true, true };
    private bool[] crossBase    = { true, true, true, true };
    private bool[] startEndBase = { true, false, false, false };

    private Color baseColor = Color.white;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (sr)
        {
            baseColor = sr.color;
            sr.sortingLayerName = "Default";
            sr.sortingOrder = 1;
        }
    }

    void Update()
    {
        // HARD BLOCK: ak sa dotkneš pravého tlačidla, tento frame nerobíme nič
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
            return;

        // Povolené je IBA ľavé tlačidlo
        if (!Input.GetMouseButtonDown(0))
            return;

        // musíš mať Collider2D na tile (BoxCollider2D napr.)
        if (col == null) return;

        // klik musí byť priamo na tento tile
        var cam = Camera.main;
        if (cam == null) return;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mouseWorld.x, mouseWorld.y);

        if (col.OverlapPoint(mousePos2D))
        {
            Rotate();
        }
    }

    public void SetPipeWithRotation(PipeType type, int rotDeg)
    {
        pipeType = type;
        currentRotation = NormalizeAngle(rotDeg);

        switch (type)
        {
            case PipeType.Straight: sr.sprite = straightSprite; break;
            case PipeType.Curve:    sr.sprite = curveSprite;    break;
            case PipeType.TCross:   sr.sprite = tCrossSprite;   break;
            case PipeType.Cross:    sr.sprite = crossSprite;    break;
            case PipeType.StartEnd: sr.sprite = startEndSprite; break;
        }

        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
        UpdateConnections();
    }

    public void SetRestrictedRotation(int angle1, int angle2)
    {
        // ak sú oba uhly rovnaké → úplne zablokuj
        if (NormalizeAngle(angle1) == NormalizeAngle(angle2))
        {
            isFixed = true;
            return;
        }
        isRestricted = true;
        rotA = NormalizeAngle(angle1);
        rotB = NormalizeAngle(angle2);
    }

    public void SetFixed(bool fixed_)
    {
        isFixed = fixed_;
    }

    void UpdateConnections()
    {
        bool[] base4 = GetBaseConnections();
        connections = new bool[4];

        int steps = Mathf.RoundToInt(currentRotation / 90f) % 4;

        for (int i = 0; i < 4; i++)
        {
            int newIndex = (i - steps) % 4;
            if (newIndex < 0) newIndex += 4;
            connections[newIndex] = base4[i];
        }
    }

    public void UpdateConnectionsPublic() => UpdateConnections();

    bool[] GetBaseConnections()
    {
        return pipeType switch
        {
            PipeType.Straight => straightBase,
            PipeType.Curve    => curveBase,
            PipeType.TCross   => tCrossBase,
            PipeType.StartEnd => startEndBase,
            _                 => crossBase
        };
    }

    public void Rotate()
    {
        GridManager gm = FindObjectOfType<GridManager>();
        if (gm != null && gm.InputLocked) return;
        if (isFixed) return;

        if (gm != null) gm.RegisterClick();

        if (isRestricted)
            currentRotation = (currentRotation == rotA) ? rotB : rotA;
        else
            currentRotation = (currentRotation + 90) % 360;

        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
        UpdateConnections();

        if (gm != null) gm.UpdateVisuals();
    }

    public void SetColor(Color color)
    {
        if (!sr) return;
        sr.color = color;
        baseColor = color;
    }

    public void SetWater(bool on)
    {
        if (!sr) return;
        sr.color = on ? Color.cyan : baseColor;
    }

    public bool HasConnection(Direction dir) => connections[(int)dir];

    int NormalizeAngle(int a) => ((a % 360) + 360) % 360;
}
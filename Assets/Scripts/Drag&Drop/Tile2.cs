using UnityEngine;

public class Tile2 : MonoBehaviour
{
    public PipeType pipeType;
    public PipeColor pipeColor;

    [HideInInspector] public bool[] connections = new bool[4];
    [HideInInspector] public int currentRotation = 0;

    [Header("--- BLUE SPRITES ---")]
    public Sprite blueStraightSprite;
    public Sprite blueCurveSprite;
    public Sprite blueTCrossSprite;
    public Sprite blueCrossSprite;
    public Sprite blueStartEndSprite;

    [Header("--- RED SPRITES ---")]
    public Sprite redStraightSprite;
    public Sprite redCurveSprite;
    public Sprite redTCrossSprite;
    public Sprite redStartEndSprite;

    [Header("--- YELLOW SPRITES ---")]
    public Sprite yellowStraightSprite;
    public Sprite yellowCurveSprite;
    public Sprite yellowTCrossSprite;
    public Sprite yellowStartEndSprite;

    [Header("--- GREEN SPRITES ---")]
    public Sprite greenStraightSprite;
    public Sprite greenCurveSprite;
    public Sprite greenTCrossSprite;
    public Sprite greenStartEndSprite;

    private SpriteRenderer sr;

    private readonly bool[] straightBase = { false, true,  false, true  };
    private readonly bool[] curveBase    = { true,  true,  false, false };
    private readonly bool[] tCrossBase   = { false, true,  true,  true  };
    private readonly bool[] crossBase    = { true,  true,  true,  true  };
    private readonly bool[] startEndBase = { true,  false, false, false };

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr)
        {
            sr.sortingLayerName = "Default";
            sr.sortingOrder = 1;
        }
    }

    public void SetPipeWithRotation(PipeType type, PipeColor color, int rotDeg)
    {
        pipeType = type;
        pipeColor = color;
        currentRotation = NormalizeAngle(rotDeg);

        if (sr)
            sr.sprite = GetSprite(pipeType, pipeColor);

        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
        UpdateConnections();
    }

    public void SetPipeWithRotation(PipeType type, int rotDeg)
    {
        SetPipeWithRotation(type, PipeColor.Blue, rotDeg);
    }

    public void Rotate90()
    {
        currentRotation = (currentRotation + 90) % 360;
        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
        UpdateConnections();
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
            PipeType.Cross    => crossBase,
            PipeType.StartEnd => startEndBase,
            _                 => straightBase
        };
    }

    Sprite GetSprite(PipeType type, PipeColor color)
    {
        // Cross existuje len v modrej
        if (type == PipeType.Cross)
            color = PipeColor.Blue;

        return color switch
        {
            PipeColor.Blue => type switch
            {
                PipeType.Straight => blueStraightSprite,
                PipeType.Curve    => blueCurveSprite,
                PipeType.TCross   => blueTCrossSprite,
                PipeType.Cross    => blueCrossSprite,
                PipeType.StartEnd => blueStartEndSprite,
                _                 => blueStraightSprite
            },
            PipeColor.Red => type switch
            {
                PipeType.Straight => redStraightSprite,
                PipeType.Curve    => redCurveSprite,
                PipeType.TCross   => redTCrossSprite,
                PipeType.StartEnd => redStartEndSprite,
                _                 => redStraightSprite
            },
            PipeColor.Yellow => type switch
            {
                PipeType.Straight => yellowStraightSprite,
                PipeType.Curve    => yellowCurveSprite,
                PipeType.TCross   => yellowTCrossSprite,
                PipeType.StartEnd => yellowStartEndSprite,
                _                 => yellowStraightSprite
            },
            PipeColor.Green => type switch
            {
                PipeType.Straight => greenStraightSprite,
                PipeType.Curve    => greenCurveSprite,
                PipeType.TCross   => greenTCrossSprite,
                PipeType.StartEnd => greenStartEndSprite,
                _                 => greenStraightSprite
            },
            _ => blueStraightSprite
        };
    }

    public bool HasConnection(Direction dir) => connections[(int)dir];

    int NormalizeAngle(int a) => ((a % 360) + 360) % 360;
}
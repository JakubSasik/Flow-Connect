using UnityEngine;

public class GridVisual2 : MonoBehaviour
{
    [Header("Level (1–18)")]
    public int level = 1;

    [Header("Veľkosť bunky")]
    public float cellSize = 1.2f;

    [Header("Farba čiar")]
    public Color lineColor = Color.black;
    public float lineWidth = 0.05f;

    int gridWidth;
    int gridHeight;

    public float CellSize   => cellSize;
    public int GridSize     => Mathf.Max(gridWidth, gridHeight);
    public int GridWidth    => gridWidth;
    public int GridHeight   => gridHeight;

    public Vector3 Origin => transform.position + new Vector3(
        -((gridWidth  - 1) * cellSize) / 2f,
        -((gridHeight - 1) * cellSize) / 2f,
        0
    );

    void Awake()  => SetGridSize();
    void Start()  { SetGridSize(); DrawGrid(); AdjustCamera(); }

    void SetGridSize()
    {
        switch (level)
        {
            case 1:  gridWidth = 3; gridHeight = 3; break;
            case 2:  gridWidth = 4; gridHeight = 4; break;
            case 3:  gridWidth = 5; gridHeight = 5; break;
            case 4:  gridWidth = 6; gridHeight = 6; break;
            case 5:  gridWidth = 7; gridHeight = 7; break;
            case 6:  gridWidth = 7; gridHeight = 6; break;
            case 7:  gridWidth = 8; gridHeight = 6; break;
            case 8:  gridWidth = 8; gridHeight = 6; break;
            case 9:  gridWidth = 9; gridHeight = 7; break;

            case 10:  gridWidth = 5; gridHeight = 6; break;
            case 11:  gridWidth = 7; gridHeight = 5; break;
            case 12:  gridWidth = 8; gridHeight = 6; break;
            case 13:  gridWidth = 9; gridHeight = 7; break;
            case 14: gridWidth = 10; gridHeight = 8; break;
            case 15: gridWidth = 7; gridHeight = 5; break;                                                               
            case 16: gridWidth = 8; gridHeight = 6; break;
            case 17: gridWidth = 9; gridHeight = 7; break;
            case 18: gridWidth = 10; gridHeight = 8; break;
            default: gridWidth = 3; gridHeight = 3; break;
        }
    }

    void DrawGrid()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        Vector3 origin = Origin;

        for (int y = 0; y <= gridHeight; y++)
        {
            Vector3 start = origin + new Vector3(-cellSize / 2f, y * cellSize - cellSize / 2f, 0);
            Vector3 end   = origin + new Vector3(gridWidth * cellSize - cellSize / 2f, y * cellSize - cellSize / 2f, 0);
            CreateLine($"HLine_{y}", start, end);
        }

        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = origin + new Vector3(x * cellSize - cellSize / 2f, -cellSize / 2f, 0);
            Vector3 end   = origin + new Vector3(x * cellSize - cellSize / 2f, gridHeight * cellSize - cellSize / 2f, 0);
            CreateLine($"VLine_{x}", start, end);
        }
    }

    void CreateLine(string lineName, Vector3 start, Vector3 end)
    {
        GameObject go = new GameObject(lineName);
        go.transform.parent = transform;

        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = lineWidth;
        lr.endWidth   = lineWidth;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lineColor;
        lr.endColor   = lineColor;
        lr.sortingLayerName = "Default";
        lr.sortingOrder = 0;
    }

    public Vector3 GetCellWorld(int x, int y)
        => Origin + new Vector3(x * cellSize, y * cellSize, 0);

    public bool GetCellFromWorld(Vector3 worldPos, out Vector2Int cell)
    {
        Vector3 local = worldPos - Origin;
        int x = Mathf.RoundToInt(local.x / cellSize);
        int y = Mathf.RoundToInt(local.y / cellSize);

        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            cell = new Vector2Int(x, y);
            return true;
        }

        cell = Vector2Int.zero;
        return false;
    }

    void AdjustCamera()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float sizeW = (gridWidth  * cellSize) / 2f + 1f;
        float sizeH = (gridHeight * cellSize) / 2f + 1f;
        cam.orthographicSize = Mathf.Max(sizeW, sizeH);
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    void OnDrawGizmos()
    {
        SetGridSize();
        Vector3 origin = Origin;
        Gizmos.color = new Color(0.3f, 0.6f, 1f, 0.4f);

        for (int x = 0; x < gridWidth; x++)
        for (int y = 0; y < gridHeight; y++)
        {
            Vector3 pos = origin + new Vector3(x * cellSize, y * cellSize, 0);
            Gizmos.DrawWireCube(pos, Vector3.one * (cellSize * 0.95f));
        }
    }
}
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DraggablePipe : MonoBehaviour
{
    [HideInInspector] public GridVisual2 grid;

    [HideInInspector] public bool isFixed = false;
    public void SetFixed(bool val) => isFixed = val;

    public static bool IsDragging { get; private set; } = false;

    Camera cam;
    bool dragging;
    Vector3 dragOffset;
    Vector3 originPos;
    Vector2Int originCell;
    SpriteRenderer sr;

    void Awake()
    {
        cam = Camera.main;
        sr  = GetComponent<SpriteRenderer>();
        if (grid == null)
            grid = FindObjectOfType<GridVisual2>();
    }

    void Start()
    {
        SnapToGrid();
    }

    void SnapToGrid()
    {
        if (grid == null) return;
        if (grid.GetCellFromWorld(transform.position, out Vector2Int cell))
        {
            transform.position = grid.GetCellWorld(cell.x, cell.y);
            originCell = cell;
            originPos  = transform.position;
        }
    }

    void Update()
    {
        WinChecker checker = FindObjectOfType<WinChecker>();
        if (checker != null && checker.InputLocked) return;

        if (Input.GetMouseButtonDown(0) && IsMouseOver() && !isFixed && !IsDragging)
        {
            if (grid == null) return;

            dragging   = true;
            IsDragging = true;

            originPos = transform.position;
            grid.GetCellFromWorld(transform.position, out originCell);
            dragOffset = transform.position - MouseWorld();

            if (sr) sr.sortingOrder = 10;
        }

       
        if (dragging && Input.GetMouseButton(0))
        {
            transform.position = MouseWorld() + dragOffset;
        }

      
        if (dragging && Input.GetMouseButtonUp(0))
        {
            dragging   = false;
            IsDragging = false;

            if (sr) sr.sortingOrder = 1;

            if (grid == null) { transform.position = originPos; return; }

            if (!grid.GetCellFromWorld(MouseWorld(), out Vector2Int targetCell))
            {
                transform.position = originPos;
                return;
            }

            DraggablePipe targetPipe = FindPipeAtCell(targetCell);
            WinChecker wc = FindObjectOfType<WinChecker>();

            if (targetPipe != null && targetPipe != this)
            {
                
                if (targetPipe.isFixed)
                {
                    transform.position = originPos;
                    return;
                }

                
                targetPipe.transform.position = originPos;
                transform.position = grid.GetCellWorld(targetCell.x, targetCell.y);
                if (wc) wc.CheckWin();
            }
            else if (targetPipe == null)
            {
                
                transform.position = grid.GetCellWorld(targetCell.x, targetCell.y);
                if (wc) wc.CheckWin();
            }
            else
            {
                
                transform.position = originPos;
            }
        }

        
        if (Input.GetMouseButtonDown(1) && IsMouseOver() && !isFixed && !IsDragging)
        {
            Tile2 tile2 = GetComponent<Tile2>();
            if (tile2 != null)
            {
                tile2.Rotate90();  
            }
            else
            {
                transform.Rotate(0, 0, 90f);
            }

            
            WinChecker wc = FindObjectOfType<WinChecker>();
            if (wc) wc.CheckWin();
        }
    }

    bool IsMouseOver()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return false;
        return col.OverlapPoint(MouseWorld());
    }

    DraggablePipe FindPipeAtCell(Vector2Int cell)
    {
        Vector3 cellWorld = grid.GetCellWorld(cell.x, cell.y);
        float threshold   = grid.CellSize * 0.5f;

        DraggablePipe[] allPipes = FindObjectsOfType<DraggablePipe>();
        foreach (var pipe in allPipes)
        {
            if (pipe == this) continue;
            if (Vector3.Distance(pipe.transform.position, cellWorld) < threshold)
                return pipe;
        }
        return null;
    }

    Vector3 MouseWorld()
    {
        Vector3 m = Input.mousePosition;
        m.z = 10f;
        Vector3 w = cam.ScreenToWorldPoint(m);
        w.z = 0f;
        return w;
    }
}
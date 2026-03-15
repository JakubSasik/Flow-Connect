using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WinChecker : MonoBehaviour
{
    [Header("Nastavenia")]
    public float waterStepDelay = 0.08f;
    public float winDelay = 2f;

    [Header("Farby")]
    public Color waterColor = Color.cyan;

    [Header("Audio")]
    public AudioClip waterClip;
    private AudioSource audioSource;

    [Header("Nastavenia levelu")]
    public bool requireAllPipes = true;
    public bool ignoreColor = false;

    [HideInInspector] public Vector2Int startCell;
    [HideInInspector] public Vector2Int endCell;
    [HideInInspector] public List<Vector2Int> endCells;

    [Header("HUD Timer Text")]
    public TMPro.TextMeshProUGUI timerText;

    UIManager2 ui;
    GridVisual2 grid;
    bool won = false;

    public bool InputLocked { get; private set; } = false;

    float elapsed = 0f;

    void Start()
    {
        ui = FindObjectOfType<UIManager2>();
        grid = FindObjectOfType<GridVisual2>();
        if (grid == null) Debug.LogError("WinChecker: GridVisual2 nenájdený!");
        if (ui == null) Debug.LogError("WinChecker: UIManager2 nenájdený!");

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        if (InputLocked || won) return;

        elapsed += Time.deltaTime;

        if (timerText != null)
        {
            int m = Mathf.FloorToInt(elapsed / 60f);
            int s = Mathf.FloorToInt(elapsed % 60f);
            timerText.text = $"TIME: {m:00}:{s:00}";
        }
    }

    public void CheckWin()
    {
        if (won || grid == null) return;

        Tile2[] allTiles = FindObjectsOfType<Tile2>();
        Dictionary<Vector2Int, Tile2> tileMap = new Dictionary<Vector2Int, Tile2>();

        foreach (var t in allTiles)
        {
            if (grid.GetCellFromWorld(t.transform.position, out Vector2Int cell))
                tileMap[cell] = t;
        }

        if (tileMap.Count == 0) return;

        bool ok = ignoreColor
            ? CheckSingleColorBFS(tileMap)
            : CheckMultiColorBFS(tileMap);

        if (!ok) return;

        won = true;
        InputLocked = true;

        StartCoroutine(FloodAndWin(tileMap));
    }

    bool CheckSingleColorBFS(Dictionary<Vector2Int, Tile2> tileMap)
    {
        List<Vector2Int> startEnds = new List<Vector2Int>();

        foreach (var kvp in tileMap)
        {
            if (kvp.Value.pipeType == PipeType.StartEnd)
                startEnds.Add(kvp.Key);
        }

        if (startEnds.Count < 2) return false;

        startEnds.Sort(CompareCells);
        Vector2Int start = startEnds[0];

        bool[,] visited = new bool[grid.GridWidth, grid.GridHeight];
        Queue<Vector2Int> q = new Queue<Vector2Int>();

        q.Enqueue(start);
        visited[start.x, start.y] = true;

        while (q.Count > 0)
        {
            Vector2Int cur = q.Dequeue();

            foreach (Direction d in System.Enum.GetValues(typeof(Direction)))
            {
                Vector2Int next = GetNeighbor(cur, d);
                if (!InBounds(next)) continue;
                if (visited[next.x, next.y]) continue;
                if (!tileMap.ContainsKey(next)) continue;

                Direction opp = Opposite(d);

                if (tileMap[cur].HasConnection(d) && tileMap[next].HasConnection(opp))
                {
                    visited[next.x, next.y] = true;
                    q.Enqueue(next);
                }
            }
        }

        foreach (var se in startEnds)
        {
            if (!visited[se.x, se.y]) return false;
        }

        if (requireAllPipes)
        {
            foreach (var kvp in tileMap)
            {
                if (!visited[kvp.Key.x, kvp.Key.y]) return false;
            }
        }

        return true;
    }

    bool CheckMultiColorBFS(Dictionary<Vector2Int, Tile2> tileMap)
    {
        Dictionary<PipeColor, List<Vector2Int>> byColor = BuildStartEndGroups(tileMap);

        if (byColor.Count == 0) return false;

        HashSet<Vector2Int> allVisitedCells = new HashSet<Vector2Int>();

        foreach (var pair in byColor)
        {
            PipeColor color = pair.Key;
            List<Vector2Int> points = pair.Value;

            if (points.Count < 2) return false;

            Vector2Int start = points[0];

            bool[,] visited = new bool[grid.GridWidth, grid.GridHeight];
            Queue<Vector2Int> q = new Queue<Vector2Int>();

            q.Enqueue(start);
            visited[start.x, start.y] = true;
            allVisitedCells.Add(start);

            while (q.Count > 0)
            {
                Vector2Int cur = q.Dequeue();

                foreach (Direction d in System.Enum.GetValues(typeof(Direction)))
                {
                    Vector2Int next = GetNeighbor(cur, d);
                    if (!InBounds(next)) continue;
                    if (visited[next.x, next.y]) continue;
                    if (!tileMap.ContainsKey(next)) continue;

                    Tile2 curTile = tileMap[cur];
                    Tile2 nextTile = tileMap[next];

                    bool curOk = curTile.pipeColor == color;
                    bool nextOk = nextTile.pipeColor == color;
                    if (!curOk || !nextOk) continue;

                    Direction opp = Opposite(d);

                    if (curTile.HasConnection(d) && nextTile.HasConnection(opp))
                    {
                        visited[next.x, next.y] = true;
                        q.Enqueue(next);
                        allVisitedCells.Add(next);
                    }
                }
            }

            foreach (var se in points)
            {
                if (!visited[se.x, se.y]) return false;
            }
        }

        if (requireAllPipes)
        {
            foreach (var kvp in tileMap)
            {
                if (!allVisitedCells.Contains(kvp.Key))
                    return false;
            }
        }

        return true;
    }

    IEnumerator FloodAndWin(Dictionary<Vector2Int, Tile2> tileMap)
    {
        // spusti zvuk vody
        if (audioSource != null && waterClip != null)
        {
            audioSource.clip = waterClip;
            audioSource.Play();
        }

        if (ignoreColor)
        {
            List<Vector2Int> startEnds = new List<Vector2Int>();
            foreach (var kvp in tileMap)
            {
                if (kvp.Value.pipeType == PipeType.StartEnd)
                    startEnds.Add(kvp.Key);
            }

            if (startEnds.Count >= 2)
            {
                startEnds.Sort(CompareCells);
                Vector2Int start = startEnds[0];

                bool[,] visited = new bool[grid.GridWidth, grid.GridHeight];
                Vector2Int?[,] parent = new Vector2Int?[grid.GridWidth, grid.GridHeight];
                Queue<Vector2Int> q = new Queue<Vector2Int>();

                q.Enqueue(start);
                visited[start.x, start.y] = true;

                while (q.Count > 0)
                {
                    Vector2Int cur = q.Dequeue();

                    foreach (Direction d in System.Enum.GetValues(typeof(Direction)))
                    {
                        Vector2Int next = GetNeighbor(cur, d);
                        if (!InBounds(next)) continue;
                        if (visited[next.x, next.y]) continue;
                        if (!tileMap.ContainsKey(next)) continue;

                        Direction opp = Opposite(d);

                        if (tileMap[cur].HasConnection(d) && tileMap[next].HasConnection(opp))
                        {
                            visited[next.x, next.y] = true;
                            parent[next.x, next.y] = cur;
                            q.Enqueue(next);
                        }
                    }
                }

                List<List<Vector2Int>> allPaths = new List<List<Vector2Int>>();

                for (int i = 1; i < startEnds.Count; i++)
                {
                    Vector2Int target = startEnds[i];
                    if (!visited[target.x, target.y]) continue;

                    List<Vector2Int> path = new List<Vector2Int>();
                    Vector2Int c = target;

                    while (c != start)
                    {
                        path.Add(c);
                        if (!parent[c.x, c.y].HasValue) break;
                        c = parent[c.x, c.y].Value;
                    }

                    path.Add(start);
                    path.Reverse();
                    allPaths.Add(path);
                }

                yield return StartCoroutine(AnimatePaths(tileMap, allPaths));
            }
        }
        else
        {
            Dictionary<PipeColor, List<Vector2Int>> byColor = BuildStartEndGroups(tileMap);
            List<List<Vector2Int>> allPaths = new List<List<Vector2Int>>();

            foreach (var pair in byColor)
            {
                PipeColor color = pair.Key;
                List<Vector2Int> points = pair.Value;

                if (points.Count < 2) continue;

                Vector2Int start = points[0];

                bool[,] visited = new bool[grid.GridWidth, grid.GridHeight];
                Vector2Int?[,] parent = new Vector2Int?[grid.GridWidth, grid.GridHeight];
                Queue<Vector2Int> q = new Queue<Vector2Int>();

                q.Enqueue(start);
                visited[start.x, start.y] = true;

                while (q.Count > 0)
                {
                    Vector2Int cur = q.Dequeue();

                    foreach (Direction d in System.Enum.GetValues(typeof(Direction)))
                    {
                        Vector2Int next = GetNeighbor(cur, d);
                        if (!InBounds(next)) continue;
                        if (visited[next.x, next.y]) continue;
                        if (!tileMap.ContainsKey(next)) continue;

                        Tile2 curTile = tileMap[cur];
                        Tile2 nextTile = tileMap[next];

                        bool curOk = curTile.pipeColor == color;
                        bool nextOk = nextTile.pipeColor == color;
                        if (!curOk || !nextOk) continue;

                        Direction opp = Opposite(d);

                        if (curTile.HasConnection(d) && nextTile.HasConnection(opp))
                        {
                            visited[next.x, next.y] = true;
                            parent[next.x, next.y] = cur;
                            q.Enqueue(next);
                        }
                    }
                }

                for (int i = 1; i < points.Count; i++)
                {
                    Vector2Int target = points[i];
                    if (!visited[target.x, target.y]) continue;

                    List<Vector2Int> path = new List<Vector2Int>();
                    Vector2Int c = target;

                    while (c != start)
                    {
                        path.Add(c);
                        if (!parent[c.x, c.y].HasValue) break;
                        c = parent[c.x, c.y].Value;
                    }

                    path.Add(start);
                    path.Reverse();
                    allPaths.Add(path);
                }
            }

            yield return StartCoroutine(AnimatePaths(tileMap, allPaths));
        }

        // zastav zvuk
        if (audioSource != null)
            audioSource.Stop();

        yield return new WaitForSeconds(winDelay);

        int level = int.TryParse(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Replace("Drag&Drop", ""),
            out int l) ? l : 0;

        if (level > 0)
            DDHighScoreStore.SaveTime(level, elapsed);

        if (ui != null)
            ui.ShowWin(FormatTime(elapsed));
    }

    IEnumerator AnimatePaths(Dictionary<Vector2Int, Tile2> tileMap, List<List<Vector2Int>> allPaths)
    {
        int maxLen = 0;
        foreach (var path in allPaths)
        {
            if (path.Count > maxLen) maxLen = path.Count;
        }

        for (int step = 0; step < maxLen; step++)
        {
            foreach (var path in allPaths)
            {
                if (step >= path.Count) continue;

                Vector2Int cell = path[step];
                if (!tileMap.ContainsKey(cell)) continue;

                Tile2 tile = tileMap[cell];
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr) sr.color = GetWaterColor(tile.pipeColor);
            }

            yield return new WaitForSeconds(waterStepDelay);
        }
    }

    Dictionary<PipeColor, List<Vector2Int>> BuildStartEndGroups(Dictionary<Vector2Int, Tile2> tileMap)
    {
        Dictionary<PipeColor, List<Vector2Int>> byColor = new Dictionary<PipeColor, List<Vector2Int>>();

        foreach (var kvp in tileMap)
        {
            if (kvp.Value.pipeType != PipeType.StartEnd) continue;

            PipeColor c = kvp.Value.pipeColor;
            if (!byColor.ContainsKey(c))
                byColor[c] = new List<Vector2Int>();

            byColor[c].Add(kvp.Key);
        }

        foreach (var key in new List<PipeColor>(byColor.Keys))
        {
            byColor[key].Sort(CompareCells);
        }

        return byColor;
    }

    int CompareCells(Vector2Int a, Vector2Int b)
    {
        int cmpX = a.x.CompareTo(b.x);
        return cmpX != 0 ? cmpX : a.y.CompareTo(b.y);
    }

    Direction Opposite(Direction dir)
    {
        return (Direction)(((int)dir + 2) % 4);
    }

    Vector2Int GetNeighbor(Vector2Int cell, Direction dir)
    {
        return dir switch
        {
            Direction.Up    => new Vector2Int(cell.x, cell.y + 1),
            Direction.Right => new Vector2Int(cell.x + 1, cell.y),
            Direction.Down  => new Vector2Int(cell.x, cell.y - 1),
            Direction.Left  => new Vector2Int(cell.x - 1, cell.y),
            _               => cell
        };
    }

    bool InBounds(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < grid.GridWidth &&
               cell.y >= 0 && cell.y < grid.GridHeight;
    }

    Color GetWaterColor(PipeColor pipeColor)
    {
        return pipeColor switch
        {
            PipeColor.Blue   => new Color32(0, 140, 255, 255),
            PipeColor.Red    => new Color32(255, 60, 60, 255),
            PipeColor.Yellow => new Color32(255, 220, 0, 255),
            PipeColor.Green  => new Color32(60, 200, 80, 255),
            _                => waterColor
        };
    }

    string FormatTime(float t)
    {
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        return $"TIME: {m:00}:{s:00}";
    }
}
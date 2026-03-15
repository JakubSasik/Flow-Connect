using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GridManager : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public float cellSize = 1.2f;
    public GameObject tilePrefab;

    [Header("HUD")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI clicksText;
    public TextMeshProUGUI difficultyText;

    [Header("Water / Win")]
    public float waterStepDelay = 0.08f;
    public float winDelay = 2f;

    [Header("Audio")]
    public AudioClip waterClip;
    private AudioSource audioSource;

    private Tile[,] grid;
    private Vector2Int startPoint;
    private Vector2Int endPoint;
    private UIManager uiManager;

    public bool InputLocked { get; private set; } = false;
    private bool winTriggered = false;

    private float elapsedTime = 0f;
    private int clickCount = 0;

    void Start()
    {
        ApplyDifficulty();
        uiManager = FindObjectOfType<UIManager>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        SpawnGrid();

        elapsedTime = 0f;
        clickCount = 0;
        UpdateHUD();
        UpdateVisuals();
    }

    void Update()
    {
        if (!InputLocked && !winTriggered)
        {
            elapsedTime += Time.deltaTime;
            UpdateHUD();
        }
    }

    public string GetTime() => FormatTime(elapsedTime);
    public int GetClicks() => clickCount;

    public void RegisterClick()
    {
        if (InputLocked || winTriggered) return;
        clickCount++;
        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (timeText != null)   timeText.text   = FormatTime(elapsedTime);
        if (clicksText != null) clicksText.text = $"CLICKS: {clickCount}";
    }

    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        return $"TIME: {minutes:00}:{seconds:00}";
    }

    void ApplyDifficulty()
    {
        int d = PlayerPrefs.GetInt("difficulty", 0);
        switch (d)
        {
            case 0: width = 6;  height = 5; break;
            case 1: width = 8;  height = 6; break;
            case 2: width = 10; height = 8; break;
        }

        if (difficultyText != null)
        {
            difficultyText.text = d switch
            {
                0 => "EASY",
                1 => "NORMAL",
                2 => "HARD",
                _ => ""
            };
        }
    }

    void SpawnGrid()
    {
        grid = new Tile[width, height];

        bool startTop = Random.value > 0.5f;
        startPoint = new Vector2Int(0, startTop ? height - 1 : 0);
        endPoint   = new Vector2Int(width - 1, startTop ? 0 : height - 1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject go = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                go.name = $"Tile_{x}_{y}";
                go.transform.localScale = Vector3.one * cellSize;

                Tile tile = go.GetComponent<Tile>();
                Vector2Int coord = new Vector2Int(x, y);

                if (coord == startPoint)
                {
                    int rotA = 270;
                    int rotB = (startPoint.y == height - 1) ? 180 : 0;
                    tile.SetPipeWithRotation(PipeType.StartEnd, rotA);
                    tile.SetRestrictedRotation(rotA, rotB);
                }
                else if (coord == endPoint)
                {
                    int rotA = 90;
                    int rotB = (endPoint.y == height - 1) ? 180 : 0;
                    tile.SetPipeWithRotation(PipeType.StartEnd, rotA);
                    tile.SetRestrictedRotation(rotA, rotB);
                }
                else
                {
                    tile.SetPipeWithRotation(RandomPipeType(), Random.Range(0, 4) * 90);
                }

                grid[x, y] = tile;
            }
        }

        transform.position = new Vector3(
            -((width - 1) * cellSize) / 2f,
            -((height - 1) * cellSize) / 2f,
            0
        );

        GeneratePath();
        AdjustCamera();
    }

    PipeType RandomPipeType()
    {
        int r = Random.Range(0, 100);
        if (r < 25) return PipeType.Straight;
        if (r < 65) return PipeType.Curve;
        if (r < 90) return PipeType.TCross;
        return PipeType.Cross;
    }

    void GeneratePath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = startPoint;
        path.Add(current);

        while (current != endPoint)
        {
            bool moveHorizontal = Random.value > 0.5f;
            if (current.x == endPoint.x) moveHorizontal = false;
            else if (current.y == endPoint.y) moveHorizontal = true;

            if (moveHorizontal) current.x++;
            else current.y += (current.y < endPoint.y ? 1 : -1);

            if (!path.Contains(current)) path.Add(current);
        }

        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int cur = path[i];
            if (cur == startPoint || cur == endPoint) continue;

            Vector2Int? prev = i > 0 ? path[i - 1] : (Vector2Int?)null;
            Vector2Int? next = i < path.Count - 1 ? path[i + 1] : (Vector2Int?)null;
            SetCorrectPipe(grid[cur.x, cur.y], prev, cur, next);
        }

        foreach (Vector2Int p in path)
        {
            if (p == startPoint || p == endPoint) continue;

            int rotations = Random.Range(1, 4);
            for (int i = 0; i < rotations; i++)
            {
                grid[p.x, p.y].currentRotation = (grid[p.x, p.y].currentRotation + 90) % 360;
                grid[p.x, p.y].transform.localRotation = Quaternion.Euler(0, 0, grid[p.x, p.y].currentRotation);
                grid[p.x, p.y].UpdateConnectionsPublic();
            }
        }
    }

    void SetCorrectPipe(Tile tile, Vector2Int? prev, Vector2Int cur, Vector2Int? next)
    {
        bool left = false, right = false, up = false, down = false;

        if (prev.HasValue)
        {
            if (prev.Value.x < cur.x) left = true;
            if (prev.Value.x > cur.x) right = true;
            if (prev.Value.y < cur.y) down = true;
            if (prev.Value.y > cur.y) up = true;
        }
        if (next.HasValue)
        {
            if (next.Value.x < cur.x) left = true;
            if (next.Value.x > cur.x) right = true;
            if (next.Value.y < cur.y) down = true;
            if (next.Value.y > cur.y) up = true;
        }

        if ((left && right) || (up && down))
            tile.SetPipeWithRotation(PipeType.Straight, (up && down) ? 90 : 0);
        else if (up && right)   tile.SetPipeWithRotation(PipeType.Curve, 0);
        else if (up && left)    tile.SetPipeWithRotation(PipeType.Curve, 90);
        else if (down && left)  tile.SetPipeWithRotation(PipeType.Curve, 180);
        else if (down && right) tile.SetPipeWithRotation(PipeType.Curve, 270);
        else tile.SetPipeWithRotation(PipeType.Straight, 0);
    }

    void AdjustCamera()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float gridWidth  = width  * cellSize;
        float gridHeight = height * cellSize;
        float screenRatio = (float)Screen.width / Screen.height;

        cam.orthographicSize =
            (screenRatio >= gridWidth / gridHeight)
            ? (gridHeight / 2f + 1f)
            : ((gridWidth / screenRatio) / 2f + 1f);

        cam.transform.position = new Vector3(0, 0, -10);
    }

    public void UpdateVisuals()
    {
        if (grid == null) return;
        if (InputLocked) return;
        if (winTriggered) return;

        bool[,] visited       = new bool[width, height];
        Vector2Int?[,] parent = new Vector2Int?[width, height];

        Queue<Vector2Int> q = new Queue<Vector2Int>();
        q.Enqueue(startPoint);
        visited[startPoint.x, startPoint.y] = true;

        while (q.Count > 0)
        {
            Vector2Int cur = q.Dequeue();
            TryEnqueue(q, visited, parent, cur, new Vector2Int(cur.x, cur.y + 1), Direction.Up);
            TryEnqueue(q, visited, parent, cur, new Vector2Int(cur.x + 1, cur.y), Direction.Right);
            TryEnqueue(q, visited, parent, cur, new Vector2Int(cur.x, cur.y - 1), Direction.Down);
            TryEnqueue(q, visited, parent, cur, new Vector2Int(cur.x - 1, cur.y), Direction.Left);
        }

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            grid[x, y].SetWater(false);
            Vector2Int coord = new Vector2Int(x, y);
            if (coord == startPoint)
                grid[x, y].SetColor(new Color(0.3f, 0.5f, 1f));
            else if (coord == endPoint)
                grid[x, y].SetColor(visited[x, y] ? Color.green : Color.red);
        }

        if (visited[endPoint.x, endPoint.y])
        {
            winTriggered = true;
            InputLocked  = true;

            int d = PlayerPrefs.GetInt("difficulty", 0);
            HighScoreStore.AddTime(d, elapsedTime);
            HighScoreStore.AddClicks(d, clickCount);
            UpdateHUD();

            List<Vector2Int> path = BuildPath(parent, endPoint);
            StartCoroutine(PlayWaterThenWin(path));
        }
    }

    void TryEnqueue(Queue<Vector2Int> q, bool[,] visited, Vector2Int?[,] parent,
        Vector2Int cur, Vector2Int next, Direction dir)
    {
        if (next.x < 0 || next.x >= width || next.y < 0 || next.y >= height) return;
        if (visited[next.x, next.y]) return;

        Direction opp = (Direction)(((int)dir + 2) % 4);

        if (grid[cur.x, cur.y].HasConnection(dir) && grid[next.x, next.y].HasConnection(opp))
        {
            visited[next.x, next.y] = true;
            parent[next.x, next.y]  = cur;
            q.Enqueue(next);
        }
    }

    List<Vector2Int> BuildPath(Vector2Int?[,] parent, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int? cur = end;
        while (cur.HasValue)
        {
            path.Add(cur.Value);
            cur = parent[cur.Value.x, cur.Value.y];
        }
        path.Reverse();
        return path;
    }

    IEnumerator PlayWaterThenWin(List<Vector2Int> path)
    {
        if (audioSource != null && waterClip != null)
        {
            audioSource.clip = waterClip;
            audioSource.Play();
        }

        foreach (Vector2Int p in path)
        {
            grid[p.x, p.y].SetWater(true);
            yield return new WaitForSecondsRealtime(waterStepDelay);
        }

        if (audioSource != null)
            audioSource.Stop();

        yield return new WaitForSecondsRealtime(winDelay);

        if (timeText != null) timeText.gameObject.SetActive(false);
        if (clicksText != null) clicksText.gameObject.SetActive(false);
        if (difficultyText != null) difficultyText.gameObject.SetActive(false);

        if (uiManager != null) uiManager.ShowWin();
    }
}
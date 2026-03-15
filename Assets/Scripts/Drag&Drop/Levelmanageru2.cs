using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManagerU2 : MonoBehaviour
{
    [Header("Pipe Prefab")]
    public GameObject pipePrefab;

    GridVisual2 grid;
    GameObject[,] placed;
    List<PipeData> pipes = new List<PipeData>();
    int shuffleSwaps;

    struct PipeData
    {
        public Vector2Int cell;
        public PipeType type;
        public PipeColor color;
        public int correctRot;
        public bool isFixed;
    }

    PipeData P(int x, int y, PipeType type, PipeColor color, int rot, bool isFixed = false) =>
        new PipeData { cell = new Vector2Int(x, y), type = type, color = color, correctRot = rot, isFixed = isFixed };

    void Start()
    {
        grid = FindObjectOfType<GridVisual2>();
        if (grid == null) { Debug.LogError("GridVisual2 nenájdený!"); return; }

        placed = new GameObject[grid.GridWidth, grid.GridHeight];

        int level = GetCurrentLevel();
        SetupLevel(level);
        SpawnPipes();
        Shuffle();
    }

    int GetCurrentLevel()
    {
        string name = SceneManager.GetActiveScene().name;
        string num = name.Replace("Drag&Drop", "");
        if (int.TryParse(num, out int l))
            return l - 9;
        return 1;
    }

    void SetupLevel(int level)
    {
        pipes.Clear();
        WinChecker wc = FindObjectOfType<WinChecker>();

        switch (level)
        {
            case 1:
                shuffleSwaps = 40;
                if (wc)
                {
                    wc.requireAllPipes = false;
                    wc.ignoreColor = true;
                    wc.startCell = new Vector2Int(0, 2);
                    wc.endCells = new List<Vector2Int> { new Vector2Int(4, 0), new Vector2Int(4, 5) };
                }
                pipes.Add(P(0, 2, PipeType.StartEnd, PipeColor.Blue, 270, true));
                pipes.Add(P(4, 0, PipeType.StartEnd, PipeColor.Blue, 0, true));
                pipes.Add(P(4, 5, PipeType.StartEnd, PipeColor.Blue, 180, true));
                pipes.Add(P(1, 2, PipeType.Curve, PipeColor.Blue, 270));
                pipes.Add(P(2, 2, PipeType.TCross, PipeColor.Blue, 270));
                pipes.Add(P(2, 3, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(2, 4, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(2, 5, PipeType.Curve, PipeColor.Blue, 180));
                pipes.Add(P(3, 5, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(3, 4, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(2, 1, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(2, 0, PipeType.Curve, PipeColor.Blue, 270));
                pipes.Add(P(3, 0, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(3, 1, PipeType.Curve, PipeColor.Blue, 180));
                pipes.Add(P(4, 1, PipeType.Straight, PipeColor.Blue, 90));
                break;

            case 2:
                shuffleSwaps = 45;
                if (wc)
                {
                    wc.requireAllPipes = false;
                    wc.ignoreColor = true;
                    wc.startCell = new Vector2Int(0, 0);
                    wc.endCells = new List<Vector2Int> { new Vector2Int(5, 1), new Vector2Int(6, 4) };
                }
                pipes.Add(P(0, 0, PipeType.StartEnd, PipeColor.Blue, 0, true));
                pipes.Add(P(5, 1, PipeType.StartEnd, PipeColor.Blue, 180, true));
                pipes.Add(P(6, 4, PipeType.StartEnd, PipeColor.Blue, 90, true));
                pipes.Add(P(0, 1, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(0, 2, PipeType.Curve, PipeColor.Blue, 270));
                pipes.Add(P(1, 2, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(2, 2, PipeType.TCross, PipeColor.Blue, 0));
                pipes.Add(P(2, 3, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(2, 4, PipeType.Curve, PipeColor.Blue, 270));
                pipes.Add(P(3, 4, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(4, 4, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(5, 4, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(3, 2, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(4, 2, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(5, 2, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(1, 0, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(3, 3, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(1, 3, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(6, 2, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(0, 3, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(4, 3, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(2, 0, PipeType.Straight, PipeColor.Blue, 0));
                break;

            case 3:
                shuffleSwaps = 50;
                if (wc)
                {
                    wc.requireAllPipes = false;
                    wc.ignoreColor = true;
                    wc.startCell = new Vector2Int(1, 2);
                    wc.endCells = new List<Vector2Int> { new Vector2Int(3, 5), new Vector2Int(4, 0), new Vector2Int(7, 3) };
                }
                pipes.Add(P(1, 2, PipeType.StartEnd, PipeColor.Blue, 90, true));
                pipes.Add(P(3, 5, PipeType.StartEnd, PipeColor.Blue, 180, true));
                pipes.Add(P(4, 0, PipeType.StartEnd, PipeColor.Blue, 0, true));
                pipes.Add(P(7, 3, PipeType.StartEnd, PipeColor.Blue, 90, true));
                pipes.Add(P(4, 3, PipeType.Cross, PipeColor.Blue, 0));
                pipes.Add(P(2, 2, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(2, 3, PipeType.Curve, PipeColor.Blue, 270));
                pipes.Add(P(3, 3, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(5, 3, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(6, 3, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(4, 4, PipeType.Curve, PipeColor.Blue, 180));
                pipes.Add(P(3, 4, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(4, 2, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(4, 1, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(1, 4, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(6, 1, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(0, 3, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(0, 5, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(6, 5, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(1, 0, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(7, 1, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(3, 1, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(5, 4, PipeType.Curve, PipeColor.Blue, 0));
                break;

            case 4:
                shuffleSwaps = 55;
                if (wc)
                {
                    wc.requireAllPipes = false;
                    wc.ignoreColor = true;
                    wc.startCell = new Vector2Int(0, 1);
                    wc.endCells = new List<Vector2Int> { new Vector2Int(2, 6), new Vector2Int(6, 0), new Vector2Int(8, 4) };
                }
                pipes.Add(P(0, 1, PipeType.StartEnd, PipeColor.Blue, 270, true));
                pipes.Add(P(2, 6, PipeType.StartEnd, PipeColor.Blue, 180, true));
                pipes.Add(P(6, 0, PipeType.StartEnd, PipeColor.Blue, 0, true));
                pipes.Add(P(8, 4, PipeType.StartEnd, PipeColor.Blue, 90, true));
                pipes.Add(P(4, 3, PipeType.Cross, PipeColor.Blue, 0));
                pipes.Add(P(1, 1, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(2, 1, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(2, 2, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(2, 3, PipeType.Curve, PipeColor.Blue, 270));
                pipes.Add(P(3, 3, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(4, 4, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(4, 5, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(3, 5, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(2, 5, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(4, 2, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(4, 1, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(5, 1, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(6, 1, PipeType.Curve, PipeColor.Blue, 180));
                pipes.Add(P(5, 3, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(6, 3, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(7, 3, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(7, 4, PipeType.Curve, PipeColor.Blue, 270));
                pipes.Add(P(0, 4, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(1, 5, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(5, 5, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(8, 1, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(3, 0, PipeType.Straight, PipeColor.Blue, 0));
                break;

            case 5:
                shuffleSwaps = 60;
                if (wc)
                {
                    wc.requireAllPipes = false;
                    wc.ignoreColor = true;
                    wc.startCell = new Vector2Int(1, 6);
                    wc.endCells = new List<Vector2Int> { new Vector2Int(9, 7), new Vector2Int(0, 0), new Vector2Int(7, 1), new Vector2Int(3, 7) };
                }
                pipes.Add(P(1, 6, PipeType.StartEnd, PipeColor.Blue, 180, true));
                pipes.Add(P(9, 7, PipeType.StartEnd, PipeColor.Blue, 90, true));
                pipes.Add(P(0, 0, PipeType.StartEnd, PipeColor.Blue, 0, true));
                pipes.Add(P(7, 1, PipeType.StartEnd, PipeColor.Blue, 180, true));
                pipes.Add(P(3, 7, PipeType.StartEnd, PipeColor.Blue, 180, true));
                pipes.Add(P(6, 4, PipeType.Cross, PipeColor.Blue, 0));
                pipes.Add(P(4, 2, PipeType.TCross, PipeColor.Blue, 0));
                pipes.Add(P(1, 5, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(1, 4, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(2, 4, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(3, 4, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(4, 4, PipeType.Curve, PipeColor.Blue, 270));
                pipes.Add(P(4, 3, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(5, 3, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(6, 3, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(6, 5, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(6, 6, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(7, 6, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(8, 6, PipeType.Curve, PipeColor.Blue, 270));
                pipes.Add(P(8, 7, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(7, 4, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(8, 4, PipeType.Curve, PipeColor.Blue, 180));
                pipes.Add(P(8, 3, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(8, 2, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(7, 2, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(6, 2, PipeType.Curve, PipeColor.Blue, 180));
                pipes.Add(P(5, 2, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(3, 2, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(2, 2, PipeType.Curve, PipeColor.Blue, 180));
                pipes.Add(P(1, 1, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(0, 1, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(4, 5, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(4, 6, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(3, 6, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(0, 5, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(2, 6, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(9, 3, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(5, 6, PipeType.Straight, PipeColor.Blue, 90));
                break;

            case 6: 
                shuffleSwaps = 40;
                if (wc)
                {
                    wc.requireAllPipes = false;
                    wc.ignoreColor = false;
                    wc.startCell = new Vector2Int(1, 4);
                    wc.endCells = new List<Vector2Int> { new Vector2Int(5, 2), new Vector2Int(4, 2) };
                }
              
                pipes.Add(P(1, 4, PipeType.StartEnd, PipeColor.Blue, 180, true));
                pipes.Add(P(5, 2, PipeType.StartEnd, PipeColor.Blue, 180, true));
                pipes.Add(P(1, 3, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(2, 3, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(3, 3, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(4, 3, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(5, 3, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(6, 3, PipeType.Curve, PipeColor.Blue, 180));
                pipes.Add(P(6, 2, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(6, 1, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(5, 1, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(2, 1, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(0, 0, PipeType.Curve, PipeColor.Blue, 0));
                
                pipes.Add(P(0, 4, PipeType.StartEnd, PipeColor.Red, 180, true));
                pipes.Add(P(4, 2, PipeType.StartEnd, PipeColor.Red, 180, true));
                pipes.Add(P(0, 3, PipeType.Straight, PipeColor.Red, 90));
                pipes.Add(P(0, 2, PipeType.Curve, PipeColor.Red, 0));
                pipes.Add(P(1, 2, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(2, 2, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(3, 2, PipeType.Curve, PipeColor.Red, 180));
                pipes.Add(P(3, 1, PipeType.Curve, PipeColor.Red, 0));
                pipes.Add(P(4, 1, PipeType.Curve, PipeColor.Red, 90));
                pipes.Add(P(1, 1, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(2, 0, PipeType.Curve, PipeColor.Red, 0));
                pipes.Add(P(0, 1, PipeType.Straight, PipeColor.Red, 0));
                break;

            case 7: 
                shuffleSwaps = 45;
                if (wc)
                {
                    wc.requireAllPipes = false;
                    wc.ignoreColor = false;
                }
                
                pipes.Add(P(1, 0, PipeType.StartEnd, PipeColor.Blue, 270, true));
                pipes.Add(P(7, 5, PipeType.StartEnd, PipeColor.Blue, 90, true));
                pipes.Add(P(7, 1, PipeType.StartEnd, PipeColor.Blue, 90, true));
                pipes.Add(P(2, 0, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(3, 0, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(4, 0, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(4, 1, PipeType.TCross, PipeColor.Blue, 270));
                pipes.Add(P(4, 2, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(4, 3, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(4, 4, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(4, 5, PipeType.Curve, PipeColor.Blue, 270));
                pipes.Add(P(5, 5, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(6, 5, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(5, 1, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(6, 1, PipeType.Straight, PipeColor.Blue, 0));
                
                pipes.Add(P(0, 0, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(7, 3, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(3, 2, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(7, 4, PipeType.Curve, PipeColor.Blue, 0));
                
                pipes.Add(P(0, 1, PipeType.StartEnd, PipeColor.Red, 270, true));
                pipes.Add(P(1, 5, PipeType.StartEnd, PipeColor.Red, 180, true));
                pipes.Add(P(3, 4, PipeType.StartEnd, PipeColor.Red, 90, true));
                pipes.Add(P(1, 1, PipeType.Curve, PipeColor.Red, 90));
                pipes.Add(P(1, 2, PipeType.Straight, PipeColor.Red, 90));
                pipes.Add(P(1, 3, PipeType.TCross, PipeColor.Red, 270));
                pipes.Add(P(1, 4, PipeType.Straight, PipeColor.Red, 90));
                pipes.Add(P(2, 3, PipeType.Curve, PipeColor.Red, 90));
                pipes.Add(P(2, 4, PipeType.Curve, PipeColor.Red, 270));
      
                pipes.Add(P(6, 3, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(0, 4, PipeType.Curve, PipeColor.Red, 0));
                break;

            case 8: 
                shuffleSwaps = 50;
                if (wc)
                {
                    wc.requireAllPipes = false;
                    wc.ignoreColor = false;
                }
         
                pipes.Add(P(0, 3, PipeType.StartEnd, PipeColor.Blue, 270, true));
                pipes.Add(P(1, 3, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(2, 3, PipeType.TCross, PipeColor.Blue, 270));
                pipes.Add(P(2, 4, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(2, 5, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(2, 6, PipeType.StartEnd, PipeColor.Blue, 180, true));
                pipes.Add(P(2, 2, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(2, 1, PipeType.Curve, PipeColor.Blue, 0));
                pipes.Add(P(3, 1, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(4, 1, PipeType.Curve, PipeColor.Blue, 180));
                pipes.Add(P(4, 0, PipeType.StartEnd, PipeColor.Blue, 0, true));

            
                pipes.Add(P(0, 0, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(1, 0, PipeType.Curve, PipeColor.Blue, 90));

            
                pipes.Add(P(7, 0, PipeType.StartEnd, PipeColor.Red, 0, true));
                pipes.Add(P(7, 1, PipeType.Straight, PipeColor.Red, 90));
                pipes.Add(P(7, 2, PipeType.Curve, PipeColor.Red, 180));
                pipes.Add(P(6, 2, PipeType.TCross, PipeColor.Red, 180));
                pipes.Add(P(6, 3, PipeType.StartEnd, PipeColor.Red, 180, true));
                pipes.Add(P(5, 2, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(4, 2, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(3, 2, PipeType.Curve, PipeColor.Red, 0));
                pipes.Add(P(3, 3, PipeType.Straight, PipeColor.Red, 90));
                pipes.Add(P(3, 4, PipeType.Curve, PipeColor.Red, 270));
                pipes.Add(P(4, 4, PipeType.StartEnd, PipeColor.Red, 90, true));

                
                pipes.Add(P(6, 0, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(6, 1, PipeType.Curve, PipeColor.Red, 180));

             
                pipes.Add(P(8, 0, PipeType.StartEnd, PipeColor.Green, 0, true));
                pipes.Add(P(8, 1, PipeType.Straight, PipeColor.Green, 90));
                pipes.Add(P(8, 2, PipeType.Straight, PipeColor.Green, 90));
                pipes.Add(P(8, 3, PipeType.Straight, PipeColor.Green, 90));
                pipes.Add(P(8, 4, PipeType.Straight, PipeColor.Green, 90));
                pipes.Add(P(8, 5, PipeType.Curve, PipeColor.Green, 180));
                pipes.Add(P(7, 5, PipeType.Straight, PipeColor.Green, 0));
                pipes.Add(P(6, 5, PipeType.Straight, PipeColor.Green, 0));
                pipes.Add(P(5, 5, PipeType.TCross, PipeColor.Green, 0));
                pipes.Add(P(4, 5, PipeType.Straight, PipeColor.Green, 0));
                pipes.Add(P(3, 5, PipeType.Curve, PipeColor.Green, 0));
                pipes.Add(P(3, 6, PipeType.StartEnd, PipeColor.Green, 180, true));
                pipes.Add(P(5, 4, PipeType.Straight, PipeColor.Green, 90));
                pipes.Add(P(5, 3, PipeType.StartEnd, PipeColor.Green, 0, true));

                
                pipes.Add(P(7, 4, PipeType.Straight, PipeColor.Green, 90));
                pipes.Add(P(7, 6, PipeType.Curve, PipeColor.Green, 270));
                break;

            case 9: 
                shuffleSwaps = 55;
                if (wc)
                {
                    wc.requireAllPipes = false;
                    wc.ignoreColor = false;
                }

              
                pipes.Add(P(2, 2, PipeType.StartEnd, PipeColor.Blue, 270, true));
                pipes.Add(P(3, 2, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(4, 2, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(5, 2, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(6, 2, PipeType.TCross, PipeColor.Blue, 180));
                pipes.Add(P(7, 2, PipeType.StartEnd, PipeColor.Blue, 90, true));

                pipes.Add(P(6, 3, PipeType.Curve, PipeColor.Blue, 270));
                pipes.Add(P(7, 3, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(7, 4, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(7, 5, PipeType.Straight, PipeColor.Blue, 90));
                pipes.Add(P(7, 6, PipeType.StartEnd, PipeColor.Blue, 180, true));

                
                pipes.Add(P(3, 3, PipeType.Straight, PipeColor.Blue, 0));
                pipes.Add(P(4, 3, PipeType.Curve, PipeColor.Blue, 90));
                pipes.Add(P(2, 4, PipeType.Straight, PipeColor.Blue, 0));

                
                pipes.Add(P(7, 1, PipeType.StartEnd, PipeColor.Red, 90, true));
                pipes.Add(P(6, 1, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(5, 1, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(4, 1, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(3, 1, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(2, 1, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(1, 1, PipeType.Curve, PipeColor.Red, 0));

                pipes.Add(P(1, 2, PipeType.Straight, PipeColor.Red, 90));
                pipes.Add(P(1, 3, PipeType.TCross, PipeColor.Red, 90));
                pipes.Add(P(2, 3, PipeType.StartEnd, PipeColor.Red, 90, true));

                pipes.Add(P(1, 4, PipeType.Straight, PipeColor.Red, 90));
                pipes.Add(P(1, 5, PipeType.Curve, PipeColor.Red, 270));
                pipes.Add(P(2, 5, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(3, 5, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(4, 5, PipeType.Straight, PipeColor.Red, 0));

                pipes.Add(P(5, 5, PipeType.Curve, PipeColor.Red, 90));
                pipes.Add(P(5, 6, PipeType.Curve, PipeColor.Red, 270));
                pipes.Add(P(6, 6, PipeType.StartEnd, PipeColor.Red, 90, true));

                
                pipes.Add(P(3, 4, PipeType.Straight, PipeColor.Red, 0));
                pipes.Add(P(4, 4, PipeType.Curve, PipeColor.Red, 180));

               
                pipes.Add(P(1, 0, PipeType.StartEnd, PipeColor.Green, 270, true));
                pipes.Add(P(2, 0, PipeType.Straight, PipeColor.Green, 0));
                pipes.Add(P(3, 0, PipeType.Straight, PipeColor.Green, 0));
                pipes.Add(P(4, 0, PipeType.Straight, PipeColor.Green, 0));
                pipes.Add(P(5, 0, PipeType.Straight, PipeColor.Green, 0));
                pipes.Add(P(6, 0, PipeType.Straight, PipeColor.Green, 0));
                pipes.Add(P(7, 0, PipeType.Straight, PipeColor.Green, 0));

                pipes.Add(P(8, 0, PipeType.TCross, PipeColor.Green, 180));
                pipes.Add(P(9, 0, PipeType.StartEnd, PipeColor.Green, 90, true));

                pipes.Add(P(8, 1, PipeType.Curve, PipeColor.Green, 270));
                pipes.Add(P(9, 1, PipeType.Curve, PipeColor.Green, 90));

                pipes.Add(P(9, 2, PipeType.Straight, PipeColor.Green, 90));
                pipes.Add(P(9, 3, PipeType.Straight, PipeColor.Green, 90));
                pipes.Add(P(9, 4, PipeType.Straight, PipeColor.Green, 90));
                pipes.Add(P(9, 5, PipeType.Straight, PipeColor.Green, 90));
                pipes.Add(P(9, 6, PipeType.StartEnd, PipeColor.Green, 180, true));

                
                pipes.Add(P(6, 4, PipeType.Straight, PipeColor.Green, 0));
                pipes.Add(P(8, 6, PipeType.Curve, PipeColor.Green, 270));

                
                pipes.Add(P(0, 0, PipeType.StartEnd, PipeColor.Yellow, 0, true));
                pipes.Add(P(0, 1, PipeType.Straight, PipeColor.Yellow, 90));
                pipes.Add(P(0, 2, PipeType.Straight, PipeColor.Yellow, 90));
                pipes.Add(P(0, 3, PipeType.Straight, PipeColor.Yellow, 90));
                pipes.Add(P(0, 4, PipeType.Straight, PipeColor.Yellow, 90));
                pipes.Add(P(0, 5, PipeType.Straight, PipeColor.Yellow, 90));
                pipes.Add(P(0, 6, PipeType.Curve, PipeColor.Yellow, 270));

                pipes.Add(P(1, 6, PipeType.Curve, PipeColor.Yellow, 90));
                pipes.Add(P(0, 7, PipeType.StartEnd, PipeColor.Yellow, 270, true));
                pipes.Add(P(1, 7, PipeType.TCross, PipeColor.Yellow, 0));

                pipes.Add(P(2, 7, PipeType.Straight, PipeColor.Yellow, 0));
                pipes.Add(P(3, 7, PipeType.Straight, PipeColor.Yellow, 0));
                pipes.Add(P(4, 7, PipeType.Straight, PipeColor.Yellow, 0));
                pipes.Add(P(5, 7, PipeType.Straight, PipeColor.Yellow, 0));
                pipes.Add(P(6, 7, PipeType.Straight, PipeColor.Yellow, 0));
                pipes.Add(P(7, 7, PipeType.Straight, PipeColor.Yellow, 0));
                pipes.Add(P(8, 7, PipeType.Straight, PipeColor.Yellow, 0));
                pipes.Add(P(9, 7, PipeType.StartEnd, PipeColor.Yellow, 90, true));

                
                pipes.Add(P(2, 6, PipeType.Straight, PipeColor.Yellow, 0));
                pipes.Add(P(3, 6, PipeType.Curve, PipeColor.Yellow, 180));
                break;
        }
    }

    void SpawnPipes()
    {
        foreach (var p in pipes)
        {
            Vector3 pos = grid.GetCellWorld(p.cell.x, p.cell.y);
            GameObject go = Instantiate(pipePrefab, pos, Quaternion.identity);
            go.name = $"Pipe_{p.color}_{p.type}_{p.cell.x}_{p.cell.y}";

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr) sr.sortingOrder = 1;

            int spawnRot = p.isFixed ? p.correctRot : RandomRot();
            go.transform.rotation = Quaternion.Euler(0, 0, spawnRot);

            Tile2 tile2 = go.GetComponent<Tile2>();
            if (tile2 != null)
                tile2.SetPipeWithRotation(p.type, p.color, spawnRot);

            DraggablePipe dp = go.GetComponent<DraggablePipe>();
            if (dp == null) dp = go.AddComponent<DraggablePipe>();
            dp.SetFixed(p.isFixed);

            if (p.cell.x >= 0 && p.cell.x < grid.GridWidth && p.cell.y >= 0 && p.cell.y < grid.GridHeight)
                placed[p.cell.x, p.cell.y] = go;
        }
    }

    void Shuffle()
    {
        List<Vector2Int> allAvailable = new List<Vector2Int>();

        foreach (var p in pipes)
            if (!p.isFixed) allAvailable.Add(p.cell);

        for (int x = 0; x < grid.GridWidth; x++)
        for (int y = 0; y < grid.GridHeight; y++)
        {
            Vector2Int c = new Vector2Int(x, y);
            bool hasPipe = false;
            foreach (var p in pipes)
                if (p.cell == c) { hasPipe = true; break; }
            if (!hasPipe) allAvailable.Add(c);
        }

        for (int i = 0; i < shuffleSwaps; i++)
        {
            if (allAvailable.Count < 2) break;

            Vector2Int a = allAvailable[Random.Range(0, allAvailable.Count)];
            Vector2Int b = allAvailable[Random.Range(0, allAvailable.Count)];
            if (a == b) continue;

            GameObject goA = placed[a.x, a.y];
            GameObject goB = placed[b.x, b.y];

            if (goA != null) goA.transform.position = grid.GetCellWorld(b.x, b.y);
            if (goB != null) goB.transform.position = grid.GetCellWorld(a.x, a.y);

            placed[a.x, a.y] = goB;
            placed[b.x, b.y] = goA;
        }
    }

    int RandomRot() => Random.Range(0, 4) * 90;
}
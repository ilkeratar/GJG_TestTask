using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private ControlValuesSO _controlValuesSo;
    private int rows;
    private int columns;
    private int gridSize = 0;
    private int readyCubesCount = 0;
    public float spacing = 1.05f;
    private float fillTableTime=.2f;
    public GameObject cellPrefab;
    public Transform table;
    public enum CubeType
    {
        EMPTY, // An empty cube.
        DEFAULT, // Basic cube type.
    };
    
    [System.Serializable]
    public struct CubePrefab
    {
        public CubeType type;
        public GameObject prefab;
    }
    [Space(10)]
    [SerializeField] private CubePrefab[] cubePrefabs;
    
    private Dictionary<CubeType, GameObject> cubePrefabDictionary;
    
    private Cube[,] cubes;
    private bool fillingTable = false;
    
    #endregion
    
    #region Getters & Setters
    public static GameManager Instance { get; set; }
    public Cube[,] GetCubes { get { return cubes; } }
    public int GetRows { get { return rows; } }
    public int GetColumns { get { return columns; } }
    public float GetSpacing { get { return spacing; } }
    #endregion
    private void Awake()
    {
        // Singleton Pattern
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        rows = _controlValuesSo.n;
        columns = _controlValuesSo.m;
        gridSize = rows * columns;
    }

    private void Start()
    {
        ConditionsRule();
        InitCubeDictonary();
        CreateTable();
        CreateEmptyCubes();
        StartCoroutine(FillTable());
    }
    
    private void InitCubeDictonary()
    {
        cubePrefabDictionary = new Dictionary<CubeType, GameObject>();

        // Loop through all prefabs in our array.
        for (int i = 0; i < cubePrefabs.Length; i++)
        {
            // Check that the dictionary does not already contain a key; a cube type.
            if (!cubePrefabDictionary.ContainsKey(cubePrefabs[i].type))
                // Add new key/value pair to our dictionary.
                cubePrefabDictionary.Add(cubePrefabs[i].type, cubePrefabs[i].prefab);
        }
    }
    void CreateTable()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                GameObject cell = Instantiate(cellPrefab, GetWorldPosition(x, y) * spacing, Quaternion.identity);
                cell.transform.SetParent(table);
            }
        }
    }
    void CreateEmptyCubes()
    {
        cubes = new Cube[rows, columns];

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                SpawnNewCube(x, y, CubeType.EMPTY);
            }
        }
    }
    public Cube SpawnNewCube(int x, int y, CubeType type)
    {
        GameObject newCube = Instantiate(cubePrefabDictionary[type], GetWorldPosition(x, y) * spacing, Quaternion.identity);
        newCube.transform.SetParent(table);

        // Give each cube a name so it is easier to tell which cube is which
        newCube.name = "Cube (" + x + "," + y + ")";

        cubes[x, y] = newCube.GetComponent<Cube>();
        cubes[x, y].Init(x, y, this, type);

        return cubes[x, y];
    }
    
    // Convert a grid coordinate to a world position.
    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(table.position.x - rows / 2.0f + x, table.position.y + columns/ 2.0f - y);
    }
    
    // Call FillStep() until the table is filled
    // After fill the table, check the situation is a deadlock
    public IEnumerator FillTable()
    {
        fillingTable = true;

        yield return new WaitForSeconds(fillTableTime);

        while (FillStep())
        {
            yield return new WaitForSeconds(fillTableTime);
        }
        CheckSprites();
        
        fillingTable = false;
        
    }
    public bool FillStep()
    {
        bool movedCube = false;

        // Loop through all columns from bottom to top
        // We don't care about the bottom row since it cannot be moved down - hence the -2
        for (int y = columns - 2; y >= 0; y--)
        {
            for (int x = 0; x < rows; x++)
            {
                Cube cube = cubes[x, y];

                // Check that the cube is movable
                // If not movable we can not move it down to fill the empty space and we can ignore it.
                if (cube.IsMovable())
                {
                    // Get the cube below the current one - 0 is at the top
                    Cube cubeBelow = cubes[x, y + 1];

                    // Check that it is empty
                    if (cubeBelow.GetCubeType == CubeType.EMPTY)
                    {
                        // Clean up the empty cubes before moving the new ones in
                        Destroy(cubeBelow.gameObject);

                        // Move the current cube down into the space below it
                        // We are basically swapping a movable cube with an empty one below it
                        cube.GetMovableCubeComponent.Move(x, y + 1, fillTableTime);
                        cubes[x, y + 1] = cube;
                        SpawnNewCube(x, y, CubeType.EMPTY);

                        // We have moved a cube
                        movedCube = true;
                    }
                }
            }
        }

        // After checking all the rows we reach the top to see if there is any empty spaces
        // Top Row
        for (int x = 0; x < rows; x++)
        {
            // Our cube will be at the top row
            Cube cubeBelow = cubes[x, 0];

            if (cubeBelow.GetCubeType == CubeType.EMPTY)
            {
                Destroy(cubeBelow.gameObject);

                // Create a new cube with negative Y coordinate. This will be spawn at the top, outside the board.
                // For this reason, we do not call SpawnNewCube() here.
                GameObject newCube = Instantiate(cubePrefabDictionary[CubeType.DEFAULT], GetWorldPosition(x, -1) * spacing, Quaternion.identity);
                newCube.transform.SetParent(table);

                // Assign newCube coordinate
                cubes[x, 0] = newCube.GetComponent<Cube>();
                // Initialize the cube with -1 on the Y coordinate so that we can move it from -1 to 0
                cubes[x, 0].Init(x, -1, this, CubeType.DEFAULT);
                // Move the cube to the new coordinate
                cubes[x, 0].GetMovableCubeComponent.Move(x, 0, fillTableTime);
                // Set color to a random color
                cubes[x, 0].GetCubeColorComponent.SetColor((CubeColor.ColorType)Random.Range(0, _controlValuesSo.k));

                readyCubesCount++;

                movedCube = true;
            }
        }

        return movedCube;
    }
    public void DeleteConnectedCubes(int x, int y)
    {
        // Create a list of tiles to be deleted
        List<Cube> cubesToBeDeleted;

        // We check that the tiles that are ready to be popped are equal to the size of the grid given by rows * columns,
        // and that the grid is not currently being refilled with new tiles.
        // This way, popping a tile is only possible when all the tiles are ready to be interacted with.
        if (readyCubesCount == gridSize && !fillingTable)
        {
            // We check the adjacent tiles to the one we have passed the x and y coordinates; the one we have clicked on.
            cubesToBeDeleted = cubes[x, y].GetConnectedCubes();

            // There is more than one adjacent tile
            // A match is made if there are at least two or more tiles of the same type.
            if (cubesToBeDeleted.Count > 1)
            {
                foreach (Cube cube in cubesToBeDeleted)
                    DeleteCube(cube.X, cube.Y);

                StartCoroutine(FillTable());
            }
        }
    }
    public bool DeleteCube(int x, int y)
    {
        if (cubes[x, y].IsClearable() && !cubes[x, y].GetClearableCubeComponent.IsBeingCleared)
        {
            cubes[x, y].GetClearableCubeComponent.Clear();

            // Decrease the counter of tiles that are ready
            readyCubesCount--;

            // Spawn new empty tile
            SpawnNewCube(x, y, CubeType.EMPTY);

            return true;
        }

        return false;
    }

    public void CheckSprites()
    {
        int rep = 0;
        List<Cube> cubesToBeChanged;
        // Loop through all array elements and check for deadlock
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                cubesToBeChanged = cubes[i, j].GetConnectedCubes();

                if (cubesToBeChanged.Count == 1)
                {
                    rep++;
                    if (rep == rows * columns)
                    {
                        System.Random rnd = new System.Random();
                        int lengthRow = cubes.GetLength(1);

                        for (int a = cubes.Length - 1; a >= 0; a--)
                        {
                            int i0 = a / lengthRow;
                            int i1 = a % lengthRow;

                            int b = rnd.Next(a + 1);
                            int j0 = b / lengthRow;
                            int j1 = b % lengthRow;

                            Cube temp = cubes[i0, i1];
                            cubes[i0, i1] = cubes[j0, j1];
                            cubes[j0, j1] = temp;
                            cubes[i0,i1].GetMovableCubeComponent.Move(i0, i1, fillTableTime);
                        }
                        StartCoroutine(CheckSpriteCoroutine());
                    }
                        
                }
                
                // Check the cube conditions and change the sprites
                if (cubesToBeChanged.Count <=_controlValuesSo.a)
                {
                    foreach (Cube cube in cubesToBeChanged)
                        cube.GetCubeColorComponent.ChangeCubeColor(cube.GetCubeColorComponent.CubeColors);
                }
                else if ((cubesToBeChanged.Count > _controlValuesSo.a) && (cubesToBeChanged.Count <= _controlValuesSo.b))
                {
                    foreach (Cube cube in cubesToBeChanged)
                        cube.GetCubeColorComponent.ChangeCubeSpriteSecond(cube.GetCubeColorComponent.CubeColors);

                }
                else if ((cubesToBeChanged.Count > _controlValuesSo.b) && (cubesToBeChanged.Count <= _controlValuesSo.c))
                {
                    foreach (Cube cube in cubesToBeChanged)
                        cube.GetCubeColorComponent.ChangeCubeSpriteThird(cube.GetCubeColorComponent.CubeColors);

                }
                else if (cubesToBeChanged.Count >_controlValuesSo.c)
                {
                    foreach (Cube cube in cubesToBeChanged)
                        cube.GetCubeColorComponent.ChangeCubeSpriteFourth(cube.GetCubeColorComponent.CubeColors);
                }
            }
        }
        
    }
    
    //Check conditions are correct
    private void ConditionsRule()
    {
        _controlValuesSo.c = Math.Clamp(_controlValuesSo.c, 4, 50);
        _controlValuesSo.b = Math.Clamp(_controlValuesSo.b, 3, _controlValuesSo.c - 1);
        _controlValuesSo.a = Math.Clamp(_controlValuesSo.a, 2, _controlValuesSo.b - 1);
        _controlValuesSo.k = Math.Clamp(_controlValuesSo.k, 2, 6);
        _controlValuesSo.k = Math.Clamp(_controlValuesSo.k, 2, rows* columns - 1);
    }
    
    //Check Again until the table is filled
    public IEnumerator CheckSpriteCoroutine()
    {
        yield return new WaitForSeconds(fillTableTime);
        CheckSprites();
    }

}

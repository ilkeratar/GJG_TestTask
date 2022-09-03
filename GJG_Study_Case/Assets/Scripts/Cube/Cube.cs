using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int x;
    public int y;
    
    private GameManager.CubeType cubeType;
    private MovableCube movableCubeComponent;
    private CubeColor cubeColorComponent;
    private ClerableCube clearableCubeComponent;
    
    #region Getters & Setters
    public int X { get { return x; } set { if (IsMovable()) x = value; } }
    public int Y { get { return y; } set { if (IsMovable()) y = value; } }
    public GameManager.CubeType GetCubeType { get { return cubeType; } }
    public CubeColor GetCubeColorComponent { get { return cubeColorComponent; } }
    public MovableCube GetMovableCubeComponent { get { return movableCubeComponent; } }
    public ClerableCube GetClearableCubeComponent { get { return clearableCubeComponent; } }
    #endregion

    private void Awake()
    {
        movableCubeComponent = GetComponent<MovableCube>();
        cubeColorComponent = GetComponent<CubeColor>();
        clearableCubeComponent = GetComponent<ClerableCube>();
    }

    public void Init(int _x, int _y, GameManager _gameManager, GameManager.CubeType _cubeType)
    {
        x = _x;
        y = _y;
        GameManager.Instance = _gameManager;
        cubeType = _cubeType;
    }
    
    public Cube Left => x > 0 ? GameManager.Instance.GetCubes[x - 1, y] : null;  // If X = 0, we can not return anything to our left because there are no tiles to our left. Therefore we return null.
    public Cube Top => y > 0 ? GameManager.Instance.GetCubes[x, y - 1] : null;
    public Cube Right => x < GameManager.Instance.GetRows - 1 ? GameManager.Instance.GetCubes[x + 1, y] : null;
    public Cube Bottom => y < GameManager.Instance.GetColumns - 1 ? GameManager.Instance.GetCubes[x, y + 1] : null;
    //We store the adjacent cubes in an array so that we can return them all at once.
    public Cube[] Neighbours => new[]
    {
        Left,
        Top,
        Right,
        Bottom,
    };
    
    // A recursive method to get the all the adjacent tiles.
    public List<Cube> GetConnectedCubes(List<Cube> exclude = null)
    {
        // Initially return itself
        List<Cube> result = new List<Cube>() { this };

        if (exclude == null)
        {
            // exclude = a new list that contains this cube.
            exclude = new List<Cube> { this };
        }
        else
        {
            exclude.Add(this);
        }

        foreach (Cube neighbour in Neighbours)
        {
            // Skip the neighbour
            // We use the keyword continue to break one iteration, if one of the below conditions occurs, and we continue with the next iteration in the loop.
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.cubeColorComponent.CubeColors != cubeColorComponent.CubeColors) continue;

            // Recursive 
            result.AddRange(neighbour.GetConnectedCubes(exclude));
        }
        
        return result;
    }
    
    
    
    private void OnMouseDown()
    {
        GameManager.Instance.DeleteConnectedCubes(x, y);
    }
    
    
    // Returns true if it exists
    public bool IsMovable()
    {
        return movableCubeComponent != null;
    }
    // Returns true if it exists
    public bool IsClearable()
    {
        return clearableCubeComponent != null;
    }
}

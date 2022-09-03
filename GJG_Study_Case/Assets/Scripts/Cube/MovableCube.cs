using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCube : MonoBehaviour
{
    IEnumerator moveCoroutine;
    private Cube cube;

    private void Awake()
    {
        cube = GetComponent<Cube>();
    }
    
    public void Move(int newX, int newY, float time)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = MoveCoroutine(newX, newY, time);
        StartCoroutine(moveCoroutine);
    }
    // Interpolate between the tile's start and end positions,
    // moving it a tiny bit each frame.
    private IEnumerator MoveCoroutine(int newX, int newY, float time)
    {
        cube.X = newX;
        cube.Y = newY;

        Vector2 startPos = transform.position;
        Vector2 endPos = GameManager.Instance.GetWorldPosition(newX, newY) * GameManager.Instance.GetSpacing;
        
        for (float t = 0; t <= 1 * time ; t+= Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t / time);
            yield return 0;
        }

        // Set the cube to endPos in case our loop does not get all the way to endPos
        transform.position = endPos;
    }
}

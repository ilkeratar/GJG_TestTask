using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ClerableCube : MonoBehaviour
{
    private bool isBeingCleared = false;

    protected Cube cube;

    // Getters & Setters
    public bool IsBeingCleared { get { return isBeingCleared; } }

    private void Awake()
    {
        cube = GetComponent<Cube>();
    }

    public void Clear()
    {
        isBeingCleared = true;
        Destroy(gameObject);
    }
}

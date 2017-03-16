using System;
using UnityEngine;
using System.Collections;



public class InvisibleWallParent : MonoBehaviour
{
    public DataBase _database;

    // Use this for initialization
    void Start()
    {
        Collider2D[] children = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D child in children)
        {
            child.sharedMaterial = _database.FrictionlessWall;
        }
    }
}

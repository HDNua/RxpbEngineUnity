using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 
/// </summary>
public class TiledGeometryParent : MonoBehaviour
{
    public DataBase _database;




    // Use this for initialization
    void Start()
    {
        TiledGeometryScript[] children = GetComponentsInChildren<TiledGeometryScript>();
        foreach(TiledGeometryScript child in children)
        {
            child._database = _database;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

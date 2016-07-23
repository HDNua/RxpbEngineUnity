using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 물체를 회전합니다.
/// </summary>
public class Rotator : MonoBehaviour
{
    public float _rotateX = 1;
    public float _rotateY = 1;
    public float _rotateZ = 1;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotateX, _rotateY, _rotateZ);
    }
}

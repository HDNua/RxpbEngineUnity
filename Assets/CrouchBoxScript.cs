using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 
/// </summary>
public class CrouchBoxScript : MonoBehaviour
{
    BoxCollider2D _Collider;


    private void Start()
    {
        _Collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject go = collision.gameObject;
            PlayerController player = go.GetComponent<PlayerController>();
            player.MustBeCrouched = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject go = collision.gameObject;
            PlayerController player = go.GetComponent<PlayerController>();
            player.MustBeCrouched = false;
        }
    }
}

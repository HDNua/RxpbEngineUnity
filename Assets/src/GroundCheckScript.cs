using UnityEngine;
using System.Collections;

public class GroundCheckScript : MonoBehaviour
{
    ZController player;
    Animator _animator;
    LayerMask whatIsGround;

    void Start()
    {
        player = GetComponentInParent<ZController>();
        _animator = player.GetComponent<Animator>();
        whatIsGround = player.whatIsGround;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.IsTouchingLayers(whatIsGround))
        {
            _animator.SetBool("Grounded", true);
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        _animator.SetBool("Grounded", true);
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.IsTouchingLayers(whatIsGround) == false)
        {
            _animator.SetBool("Grounded", false);
        }
    }
}

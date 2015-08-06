using UnityEngine;
using System.Collections;

public class EffectScript : MonoBehaviour
{
    void Start()
    {

    }
    public void RequestEnd()
    {
        GetComponent<Animator>().SetBool("EndRequested", true);
    }
    public void EndEffect()
    {
        Destroy(gameObject);
    }
}
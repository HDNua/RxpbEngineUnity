using UnityEngine;
using System.Collections;

public class EffectScript : MonoBehaviour
{
    void Start()
    {

    }
    public void EndEffect()
    {
        Destroy(gameObject);
    }
}
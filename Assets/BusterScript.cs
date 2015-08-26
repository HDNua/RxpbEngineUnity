using UnityEngine;
using System;
using System.Collections;

[Obsolete("BusterAttackScript로 대체되었습니다.", true)]
[RequireComponent(typeof(Rigidbody2D))]
public class BusterScript : MonoBehaviour
{
    Camera mainCamera;
    public Camera MainCamera { set { mainCamera = value; } }

    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    void Start()
    {
        // mainCamera = null;
    }
    void Update()
    {
        if (mainCamera != null)
        {
            Vector3 camPos = mainCamera.transform.position;
            Vector3 bulPos = transform.position;
            if (Mathf.Abs(camPos.x - bulPos.x) > 10)
            {
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            MettoController metto
                = other.gameObject.GetComponent<MettoController>();
            metto.Hurt(5);
        }
    }

    #endregion
}
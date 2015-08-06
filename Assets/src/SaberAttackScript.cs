using UnityEngine;
using System.Collections;

public class SaberAttackScript : MonoBehaviour
{
    public ZController zero;
    public GameObject effectSlice;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    bool attackBlocked = false;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (attackBlocked)
            {

            }
            else
            {
                AttackEnemy(other.gameObject);
            }
        }
    }

    void AttackEnemy(GameObject enemy)
    {
        MettoController metto = enemy.GetComponent<MettoController>();
        float scaleX = Mathf.Abs(effectSlice.transform.localScale.x);
        effectSlice.transform.localScale = new Vector3(zero.FacingRight ? scaleX : -scaleX, effectSlice.transform.localScale.y);

        Instantiate(effectSlice, metto.transform.position, metto.transform.rotation);
        metto.Hurt(5);

        BlockAttack();
        Invoke("UnblockAttack", 0.4f);
    }
    void BlockAttack()
    {
        attackBlocked = true;
    }
    void UnblockAttack()
    {
        attackBlocked = false;
    }
}
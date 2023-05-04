using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("基本属性")]
    public int damage;
    public float attackRange;
    public float attackRate;

    private void OnTriggerStay2D(Collider2D other)
    {
        //?即先询问有没有这个组件，没有则不执行
        other.GetComponent<Character>()?.TakeDamage(this);
    }
}

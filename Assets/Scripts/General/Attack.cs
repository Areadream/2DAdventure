using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("��������")]
    public int damage;
    public float attackRange;
    public float attackRate;

    private void OnTriggerStay2D(Collider2D other)
    {
        //?����ѯ����û����������û����ִ��
        other.GetComponent<Character>()?.TakeDamage(this);
    }
}

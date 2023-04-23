using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("��������")]
    public float maxHealth;
    public float currentHealth;

    [Header("�����޵�")]
    public float invulnerableDuration;
    public float invulnerableCount;
    public bool invulnerable;//�Ƿ����޵�״̬

    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (invulnerable)
        {
            invulnerableCount -= Time.deltaTime;
            if(invulnerableCount <= 0)
            {
                invulnerable = false;
            }
        }
    }

    public void TakeDamage(Attack attacker)
    { 
        if (invulnerable)
            return;
        if(currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            //ִ������
            OnTakeDamage?.Invoke(attacker.transform);

        }
        else
        {
            currentHealth = 0;
            //��������
            OnDie.Invoke();
        }
    }

    private void TriggerInvulnerable()
    {
        invulnerable = true;
        invulnerableCount = invulnerableDuration;
    }
}

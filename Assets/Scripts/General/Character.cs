using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("受伤无敌")]
    public float invulnerableDuration;
    public float invulnerableCount;
    public bool invulnerable;//是否处于无敌状态

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie; 


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
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
            //执行受伤
            OnTakeDamage?.Invoke(attacker.transform);

        }
        else
        {
            currentHealth = 0;
            //触发死亡
            OnDie.Invoke();
        }
        OnHealthChange?.Invoke(this);
    }

    private void TriggerInvulnerable()
    {
        invulnerable = true;
        invulnerableCount = invulnerableDuration;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, ISaveable
{
    [Header("事件监听")]
    public VoidEventSO newGameEvent;
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;

    [Header("受伤无敌")]
    public float invulnerableDuration;
    public float invulnerableCount;
    public bool invulnerable;//是否处于无敌状态

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;


    // Start is called before the first frame update
    void NewGame()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);//默认血条UI不满，回满血条UI
        currentPower = maxPower;
    }
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }
    // Update is called once per frame
    void Update()
    {
        if (invulnerable)
        {
            invulnerableCount -= Time.deltaTime;
            if (invulnerableCount <= 0)
            {
                invulnerable = false;
            }
        }
        if (currentPower < maxPower)
        {
            currentPower += powerRecoverSpeed * Time.deltaTime;
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            if (currentHealth > 0)
            {
                //死亡、更新血量
                currentHealth = 0;
                OnHealthChange?.Invoke(this);
                OnDie?.Invoke();
            }
        }

    }

    public void OnSlide(int cost)
    {
        currentPower -= cost;
        OnHealthChange?.Invoke(this);
    }


    public void TakeDamage(Attack attacker)
    {
        if (invulnerable)
            return;
        if (currentHealth - attacker.damage > 0)
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

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }
    public void GetSaveData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = new SerializeVector3(transform.position);
            data.floatSaveData[GetDataID().ID + "health"] = this.currentHealth;
            data.floatSaveData[GetDataID().ID + "power"] = this.currentPower;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, new SerializeVector3(transform.position));
            data.floatSaveData.Add(GetDataID().ID + "health", this.currentHealth);
            data.floatSaveData.Add(GetDataID().ID + "power", this.currentPower);
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID].ToVector3();
            this.currentHealth = data.floatSaveData[GetDataID().ID + "health"];
            this.currentPower = data.floatSaveData[GetDataID().ID + "power"];

            //通知UI更新
            OnHealthChange?.Invoke(this);
        }
    }

}

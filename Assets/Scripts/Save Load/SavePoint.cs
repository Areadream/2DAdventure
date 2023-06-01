using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    [Header("广播")]
    public VoidEventSO saveDataEvent;

    [Header("变量参数")]
    public SpriteRenderer spriteRenderer;
    public GameObject lightObj;
    public Sprite darkSprite;
    public Sprite lightSprite;
    bool isDone;


    private void OnEnable() {
        spriteRenderer.sprite = isDone?lightSprite:darkSprite;
        lightObj.SetActive(isDone);
    }

    public void TriggerAction()
    {
        if(!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightSprite;
            lightObj.SetActive(true);

            // TODO: 保存数据
            saveDataEvent.RaiseEvent();
            Debug.Log("TriggerAction");

            this.gameObject.tag = "Untagged";
        }
    }
}

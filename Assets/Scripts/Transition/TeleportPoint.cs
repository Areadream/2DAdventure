using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour,IInteractable
{
    public Vector3 positionToGo;
    public void TriggerAction()
    {
        Debug.Log("传送！");
    }

    
}

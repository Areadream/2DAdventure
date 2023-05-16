using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo; 
    public void TriggerAction()
    {
        Debug.Log("传送！");
        loadEventSO.RaiseLoadRequestEvent(sceneToGo,positionToGo,true);
    }


}

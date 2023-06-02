using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public string sceneToSave;
    public Dictionary<string, SerializeVector3> characterPosDict = new Dictionary<string, SerializeVector3>();
    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();

    public void SaveGameScene(GameSceneSO savedScene)
    {
        sceneToSave = JsonUtility.ToJson(savedScene);
        Debug.Log(sceneToSave);
    }

    public GameSceneSO GetSavedScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);

        return newScene;
    }
}

public class SerializeVector3
{
    public float x, y, z;

    public SerializeVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x,y,z);
    }
}
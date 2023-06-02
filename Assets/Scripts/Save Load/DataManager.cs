using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using Newtonsoft.Json;

[DefaultExecutionOrder(-100)]
public class DataManager : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;
    public static DataManager instance;
    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;
    private string jsonFolder;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        saveData = new Data();
        jsonFolder = Application.persistentDataPath + "/SAVE DATA/";

        ReadSavedData();
    }
    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }
    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;

    }

    public void Save()
    {
        foreach (var saveable in saveableList)//存进saveData
        {
            saveable.GetSaveData(saveData);
            // Debug.Log("1");
        }

        var resultPath = jsonFolder + "data.sav";

        var jsonData = JsonConvert.SerializeObject(saveData);

        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }

        File.WriteAllText(resultPath,jsonData);
        // Debug.Log("Save Run");
        // foreach (var item in saveData.characterPosDict)
        // {
        //     Debug.Log(item.Key + "     " + item.Value);
        // }

    }

    public void Load()
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }

    public void ReadSavedData()
    {
        var resultPath = jsonFolder + "data.sav";

        if(File.Exists(resultPath))
        {
            var stringData = File.ReadAllText(resultPath);

            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);

            saveData = jsonData;
        }
    }

    public void RegisterSaveData(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }
    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }
}

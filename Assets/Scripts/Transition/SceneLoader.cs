using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour, ISaveable
{
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;

    [Header("广播")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO unloadedSceneEvent;
    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;



    public float fadeDuration;


    [Header("场景")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;

    [SerializeField] private GameSceneSO currentLoadScene;//获得当前场景，用以卸载

    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;

    private bool fadeScreen;
    private bool isLoading;
    private void Awake()
    {
        // Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        // currentLoadScene = firstLoadScene;
        // currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }
    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        // NewGame();
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;

        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }



    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;

        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        // OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
            return;
        isLoading = true;
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }

    }

    private void OnBackToMenuEvent()
    {
        sceneToLoad = menuScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad,menuPosition,true);
    }

    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
            //实现渐入渐出
            fadeEvent.FadeIn(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);

        unloadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);

        yield return currentLoadScene.sceneReference.UnLoadScene();

        playerTrans.gameObject.SetActive(false);
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadScene = sceneToLoad;

        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true);

        if (fadeScreen)
        {
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;

        if (currentLoadScene.sceneType == SceneType.Location)
            //场景加载完成后事件
            afterSceneLoadedEvent.RaiseEvent();
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefinition>().ID;
        if (data.characterPosDict.ContainsKey(playerID))
        {
            positionToGo = data.characterPosDict[playerID].ToVector3();
            sceneToLoad = data.GetSavedScene();
            OnLoadRequestEvent(sceneToLoad, positionToGo, true);
        }
    }
}

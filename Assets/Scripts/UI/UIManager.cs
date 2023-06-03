using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("组件")]
    public GameObject gameOverPanel;
    public GameObject restartBtn;
    public GameObject mobileTouch;
    public Button settingsBtn;
    public GameObject pausePanel;
    public Slider volumeSlider;

    [Header("广播")]
    public VoidEventSO pauseEvent;
    [Header("事件监听")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO unloadedSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public PlayerStatBar playerStatBar;
    public FloatEventSO syncVolumeEvent;



    private void Awake()
    {
#if UNITY_STANDALONE
        mobileTouch.SetActive(false);
#else
        mobileTouch.SetActive(true);
#endif

        settingsBtn.onClick.AddListener(TogglePausePanel);
    }



    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent += OnUnLoadedSceneEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent -= OnUnLoadedSceneEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
    }



    private void TogglePausePanel()
    {
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseEvent.RaiseEvent();
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnUnLoadedSceneEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }

    private void OnHealthEvent(Character character)
    {
        var percentage = character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(percentage);
        playerStatBar.OnPowerChange(character);
    }
    private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn);
    }

    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount+80)/100;
    }
}

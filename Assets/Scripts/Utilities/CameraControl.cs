using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;
    private CinemachineConfiner2D confiner2D;
    public VoidEventSO cameraShakeEvent;
    private void Awake() {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void Start() {
        GetNewCameraBounds();
    }

    private void OnEnable() {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
    }

    private void OnDisable() {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
    }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if(obj == null)
            return;
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
    }
}

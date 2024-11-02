using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_State_Manager : MonoBehaviour
{
    public enum PlayerState { ThreeDimension, TwoDimension}

    private PlayerState _state = PlayerState.ThreeDimension;

    public Camera _camera;
    public CinemachineVirtualCamera virtualCamera3D;
    public CinemachineVirtualCamera virtualCamera2D;

    [Header("3D Cam Variables")]
    public float distanceFromFollowObject;

    [Header("2D Cam Variables")]
    public float orthoSize;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerState GetState()
    {
        return _state;
    }

    public void ChangePlayerState(PlayerState state)
    {
        _state = state;

        if(_state == PlayerState.ThreeDimension)
        {
            Camera3D();
        }
        else
        {
            Camera2D();
        }

    }

    public void Camera3D()
    {
        virtualCamera3D.Priority = 10;
        virtualCamera2D.Priority = 9;

        _camera.orthographic = false;
    }

    public void Camera2D()
    {
        virtualCamera2D.Priority = 10;
        virtualCamera3D.Priority = 9;

        _camera.orthographic = true;
        _camera.orthographicSize = orthoSize;
    }
}

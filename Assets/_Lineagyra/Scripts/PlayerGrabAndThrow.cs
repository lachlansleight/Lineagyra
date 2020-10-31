using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerGrabAndThrow : MonoBehaviour
{

    public SteamVR_Action_Pose PoseAction;
    public SteamVR_Action_Single GrabAction;
    public SteamVR_Input_Sources Source;
    public float VelocityDamping = 0.1f;
    public float GrabVelocityDamping = 0.9f;
    public float ThresholdVelocity = 0.5f;

    private Player _player;
    private Vector3 _startPosition;
    private Vector3 _parentStartPosition;
    private Vector3 _velocity;
    private bool _lastGrab;
    
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    
    public void Update()
    {
        if (GrabAction.GetAxis(Source) > 0.2f) {
            var worldSpacePos = PoseAction.GetLocalPosition(Source);
            if (!_lastGrab) {
                _startPosition = worldSpacePos;
                _parentStartPosition = _player.transform.position;
            }

            var offset = _startPosition - worldSpacePos;
            _player.transform.position = _parentStartPosition + offset;
            
            _parentStartPosition += _velocity * Time.deltaTime;
            _velocity *= (1f - GrabVelocityDamping * Time.deltaTime);
            
            _lastGrab = true;
        } else if (_lastGrab) {
            var worldSpaceVel = PoseAction.GetVelocity(Source);
            if (worldSpaceVel.magnitude > ThresholdVelocity) {
                _velocity = -worldSpaceVel;
            }

            _lastGrab = false;
        } else _lastGrab = false;

        _player.transform.position += _velocity * Time.deltaTime;
        _velocity *= (1f - VelocityDamping * Time.deltaTime);
    }
}

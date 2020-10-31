using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class UiGrab : MonoBehaviour
{
    public SteamVR_Action_Single GrabAction;
    public SteamVR_Action_Pose PoseAction;
    public SteamVR_Input_Sources Source;

    public SteamVR_Action_Boolean ToggleMenu;

    [Space(10)]
    public Vector3 Offset;
    public float Smoothing = 6f;

    private Player _player;

    private Vector3 _targetPosition;
    private Camera _camera;
    private MainUiController _mainUi;
    private bool _lastState;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _mainUi = FindObjectOfType<MainUiController>();
        _camera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GrabAction.GetAxis(Source) > 0.2f) {
            _targetPosition = _player.transform.TransformPoint(PoseAction.GetLocalPosition(Source)) + Offset;
        }
        
        if (ToggleMenu.GetStateDown(SteamVR_Input_Sources.Any) && !WelcomeUiController.WelcomeShowing) {
            _mainUi.ToggleMenu();
            _targetPosition = _player.transform.TransformPoint(PoseAction.GetLocalPosition(Source)) + Offset;
            transform.position = _targetPosition;
        }
        

        if (_mainUi.MenuVisible) {
            transform.position = Vector3.Lerp(
                transform.position,
                _targetPosition,
                Time.deltaTime * Smoothing);
            var zeroPosY = transform.position;
            var camZeroPosY = _camera.transform.position;
            var offset = zeroPosY - camZeroPosY;
            offset.y = 0f;
            transform.rotation = Quaternion.LookRotation(offset, Vector3.up);
        } else {
            transform.position = new Vector3(0f, -100f, 0f);
        }
    }
}

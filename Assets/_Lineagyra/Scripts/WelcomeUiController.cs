using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class WelcomeUiController : MonoBehaviour
{

    public static bool WelcomeShowing = false;

    public Vector3 TargetOffset = new Vector3(0f, -0.5f, 1f);
    public float LerpSpeed = 4f;
    
    public SteamVR_Action_Boolean ToggleMenu;
    
    private bool _lastState;
    private Transform _camera;

    private void Awake()
    {
        WelcomeShowing = true;
        _camera = Camera.main.transform;
    }
    
    private void Update()
    {
        if (ToggleMenu.GetStateDown(SteamVR_Input_Sources.Any)) {
            Invoke(nameof(SetWelcomeShowingFalse), 0.5f);
            gameObject.SetActive(false);
        }
        
        var flatForward = _camera.forward;
        flatForward.y = 0f;

        var targetPos = _camera.position + flatForward * TargetOffset.z;
        targetPos += Vector3.up * TargetOffset.y;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * LerpSpeed);
        
        var zeroPosY = transform.position;
        var camZeroPosY = _camera.position;
        var offset = zeroPosY - camZeroPosY;
        offset.y = 0f;
        transform.rotation = Quaternion.LookRotation(offset, Vector3.up);
    }

    private void SetWelcomeShowingFalse()
    {
        WelcomeShowing = false;
    }
}

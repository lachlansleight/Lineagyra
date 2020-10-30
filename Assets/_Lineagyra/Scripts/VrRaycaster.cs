using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class VrRaycaster : MonoBehaviour
{
    public XRNode Device = XRNode.RightHand;
    private InputDevice _device;
    
    private void GetDevice()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);
        foreach (var device in devices) {
            Debug.Log(device.name + " - " + device.characteristics.ToString());
        }
        
        _device = InputDevices.GetDeviceAtXRNode(Device);
        var usages = new List<InputFeatureUsage>();
        _device.TryGetFeatureUsages(usages);
        foreach (var usage in usages) {
            Debug.Log(usage.name + "\t" + usage.type);
        }
    }

    void Update()
    {
        if (!_device.isValid) {
            GetDevice();
            return;
        }
        
        CurvedUIInputModule.CustomControllerRay = new Ray(transform.position, transform.forward);
        var success = _device.TryGetFeatureValue(CommonUsages.triggerButton, out var triggerPressed);
        CurvedUIInputModule.CustomControllerButtonState = triggerPressed;

        Debug.Log($"{success} + {triggerPressed} + {Input.GetButton("RightTrigger")}");
    }
}

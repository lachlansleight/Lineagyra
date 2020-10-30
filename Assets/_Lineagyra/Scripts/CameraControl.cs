using System;
using System.Collections;
using System.Collections.Generic;
using LineCircles;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public float PanSensitivity = 1f;
    public float ZoomSensitivity = 5f;
    public float OrbitSensitivity = 1f;
    [Space(10)]
    public float InteroplationFactor = 4f;

    private Camera _camera;
    private Transform _camTransform;
    private LineCircle _lineCircle;
    private Shuffler _shuffler;
    private SnapToBounds _snapToBounds;

    private bool _dirtyFov;
    private float _preChangeFov;
    private bool _rotationEnabled;

    private Transform _orbitReference;

    private Vector3 _cameraOffset;

    private Vector3 _targetPosition;
    private float _targetFovSize;
    private float _fovSize;
    private Quaternion _targetRotation;
    
    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();
        _camTransform = _camera.transform;

        _lineCircle = FindObjectOfType<LineCircle>();
        _shuffler = FindObjectOfType<Shuffler>();
        _snapToBounds = FindObjectOfType<SnapToBounds>();

        _lineCircle.OnPatternChanged += HandlePatternChanged;

        _targetFovSize = FovToSize(_camera.fieldOfView, _snapToBounds.PerspectiveCenterDistance);
        _fovSize = _targetFovSize;

        _orbitReference = new GameObject("Orbit Reference").transform;
        _orbitReference.position = new Vector3(0f, 0f, _snapToBounds.PerspectiveCenterDistance);
        UpdateCameraPosition();
    }

    private void HandlePatternChanged(object sender, EventArgs e)
    {
        ResetCamera(true);
        _rotationEnabled = !_shuffler.Generator.RestrictThirdDimension;
    }

    public void Update()
    {
        var scale = (Screen.width + Screen.height) * 0.5f;
        //multiplied by 1000 to make the default pan sensitivity close to 1.0
        var movementDelta = 1000f * new Vector3(
            Input.GetAxis("Mouse X") / scale, 
            Input.GetAxis("Mouse Y") / scale, 
            0f
        );
        
        if (Input.GetMouseButton(2)) {
            _cameraOffset -= movementDelta * PanSensitivity * (_camera.fieldOfView / 90f);
            UpdateCameraPosition();
        }

        if (_rotationEnabled && Input.GetMouseButton(1)) {
            _orbitReference.position = new Vector3(0f, 0f, _snapToBounds.PerspectiveCenterDistance);
            _orbitReference.Rotate(0f, movementDelta.x * OrbitSensitivity, 0f, Space.World);
            _orbitReference.Rotate(-movementDelta.y * OrbitSensitivity, 0f, 0f, Space.Self);
            UpdateCameraPosition();
        }

        var scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta > 0f) {
            if (!_dirtyFov) {
                _dirtyFov = true;
                _preChangeFov = _camera.fieldOfView;
            }
            _targetFovSize = Mathf.Clamp(
                _targetFovSize / (1f + ZoomSensitivity), 
                FovToSize(0f, _snapToBounds.PerspectiveCenterDistance), 
                FovToSize(179f, _snapToBounds.PerspectiveCenterDistance)
            );
        } else if (scrollDelta < 0f) {
            if (!_dirtyFov) {
                _dirtyFov = true;
                _preChangeFov = _camera.fieldOfView;
            }
            _targetFovSize = Mathf.Clamp(
                _targetFovSize * (1f + ZoomSensitivity), 
                FovToSize(0f, _snapToBounds.PerspectiveCenterDistance), 
                FovToSize(179f, _snapToBounds.PerspectiveCenterDistance)
            );
        }
        
        if(Input.GetKeyDown(KeyCode.R)) ResetCamera();

        _camTransform.position = Vector3.Lerp(_camTransform.position, _targetPosition, Time.deltaTime * InteroplationFactor);
        _camTransform.rotation = Quaternion.Lerp(_camTransform.rotation, _targetRotation, Time.deltaTime * InteroplationFactor);
        //_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFov, Time.deltaTime * InteroplationFactor);
        _fovSize = Mathf.Lerp(_fovSize, _targetFovSize, Time.deltaTime * InteroplationFactor);
        _camera.fieldOfView = SizeToFov(_fovSize, _snapToBounds.PerspectiveCenterDistance);
    }

    private void UpdateCameraPosition()
    {
        _targetPosition = _orbitReference.TransformPoint(0f, 0f, -_snapToBounds.PerspectiveCenterDistance);
        _targetRotation = Quaternion.LookRotation(_orbitReference.position - _targetPosition, Vector3.up);
        _targetPosition += _camTransform.TransformVector(_cameraOffset);
    }

    public void ResetCamera(bool hard = false)
    {
        _targetPosition = Vector3.zero;
        _targetRotation = Quaternion.identity;

        _orbitReference.rotation = Quaternion.identity;
        _cameraOffset = Vector3.zero;
        
        if (_dirtyFov) {
            _targetFovSize = FovToSize(_preChangeFov, _snapToBounds.PerspectiveCenterDistance);
            _dirtyFov = false;
        }

        if (hard) {
            _camTransform.position = _targetPosition;
            _camTransform.rotation = _targetRotation;
            _fovSize = _targetFovSize;
            _camera.fieldOfView = SizeToFov(_fovSize, _snapToBounds.PerspectiveCenterDistance);
        }
    }

    public void SetTargetSize(float fieldOfView)
    {
        _dirtyFov = true;
        _preChangeFov = _camera.fieldOfView;
        _targetFovSize = FovToSize(fieldOfView, _snapToBounds.PerspectiveCenterDistance);
    }

    public float GetTargetFov()
    {
        return SizeToFov(_targetFovSize, _snapToBounds.PerspectiveCenterDistance);
    }

    public static float FovToSize(float fieldOfView, float distance)
    {
        while (fieldOfView > 180f) fieldOfView -= 180f;
        
        return 2f * distance * Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad);
    }

    public static float SizeToFov(float size, float distance)
    {
        return 2f * Mathf.Atan(size / (2f * distance)) * Mathf.Rad2Deg;
    }
}

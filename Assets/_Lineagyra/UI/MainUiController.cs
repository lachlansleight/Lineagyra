using System.Collections;
using System.Collections.Generic;
using LineCircles;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainUiController : MonoBehaviour
{

    private UIDocument _ui;

    private VisualElement _main;
    private Button[] _navButtons;
    private VisualElement[] _pages;

    private Camera _camera;
    private LineCircle _lineCircle;
    private Shuffler _shuffler;
    private TimeStepper _timeStepper;
    private SnapToBounds _snapToBounds;

    private bool _menuVisible;
    
    private void OnEnable()
    {
        _ui = GetComponent<UIDocument>();
        _camera = FindObjectOfType<Camera>();
        _lineCircle = FindObjectOfType<LineCircle>();
        _shuffler = FindObjectOfType<Shuffler>();
        _timeStepper = FindObjectOfType<TimeStepper>();
        _snapToBounds = FindObjectOfType<SnapToBounds>();
        
        _main = _ui.rootVisualElement.Q("main");
        var header = _ui.rootVisualElement.Q("header");
        _navButtons = new [] {
            header.Q<Button>("camera"),
            header.Q<Button>("pattern"),
            header.Q<Button>("generator"),
            header.Q<Button>("advanced"),
        };
        var body = _ui.rootVisualElement.Q("body");
        _pages = new [] {
            body.Q("cameraContainer"),
            body.Q("patternContainer"),
            body.Q("generatorContainer"),
            body.Q("advancedContainer"),
        };

        for (var i = 0; i < _navButtons.Length; i++) {
            var index = i;
            _navButtons[i].clicked += () => NavigateToPage(index);
        }
        _main.Q<Button>("closeMenu").clicked += () => SetMenu(false);

        var fovSlider = body.Q<Slider>("fieldOfView");
        fovSlider.value = _camera.fieldOfView;
        fovSlider.RegisterCallback<ChangeEvent<float>>(e => _camera.fieldOfView = e.newValue);
        
        //TODO: Add bloom sliders after adding post processing package
        
        var patternSize = body.Q<Slider>("patternSize");
        patternSize.value = _snapToBounds.SizeMultiplier;
        patternSize.RegisterCallback<ChangeEvent<float>>(e => _snapToBounds.SizeMultiplier = e.newValue);

        NavigateToPage(0);
        SetMenu(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) SetMenu(!_menuVisible);
    }
    
    private void SetMenu(bool visible)
    {
        _main.style.display = visible
            ? new StyleEnum<DisplayStyle>(DisplayStyle.Flex) 
            : new StyleEnum<DisplayStyle>(DisplayStyle.None);

        _menuVisible = visible;
    }

    private void NavigateToPage(int page)
    {
        for (var i = 0; i < _navButtons.Length; i++) {
            if (i == page) {
                _navButtons[i].AddToClassList("selected");
            } else {
                _navButtons[i].RemoveFromClassList("selected");
            }
        }

        for (var i = 0; i < _pages.Length; i++) {
            _pages[i].style.display = i == page 
                ? new StyleEnum<DisplayStyle>(DisplayStyle.Flex) 
                : new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }
    }
}

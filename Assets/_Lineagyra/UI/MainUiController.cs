using System;
using System.Collections;
using System.Collections.Generic;
using LineCircles;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainUiController : MonoBehaviour
{
    private enum AutoShuffleMode
    {
        Off,
        Oscillate,
        Linear
    }
    

    private UIDocument _ui;

    private VisualElement _main;
    private Button[] _navButtons;
    private VisualElement[] _pages;

    private Camera _camera;
    private LineCircle _lineCircle;
    private Shuffler _shuffler;
    private TimeStepper _timeStepper;
    private SnapToBounds _snapToBounds;
    private PatternOverrides _patternOverrides;
    private PostProcessProfile _post;

    private bool _menuVisible;
    
    private void OnEnable()
    {
        _ui = GetComponent<UIDocument>();
        _camera = FindObjectOfType<Camera>();
        _lineCircle = FindObjectOfType<LineCircle>();
        _shuffler = FindObjectOfType<Shuffler>();
        _timeStepper = FindObjectOfType<TimeStepper>();
        _snapToBounds = FindObjectOfType<SnapToBounds>();
        _patternOverrides = FindObjectOfType<PatternOverrides>();
        _post = FindObjectOfType<PostProcessVolume>().profile;
        
        _main = _ui.rootVisualElement.Q("main");
        var body = _ui.rootVisualElement.Q("body");

        SetupContainerControls(body);
        SetupCameraControls(body);
        SetupPatternControls(body);
        SetupGeneratorControls(body);

        NavigateToPage(0);
        SetMenu(false);
    }

    private void SetupContainerControls(VisualElement body)
    {
        var header = _ui.rootVisualElement.Q("header");
        _navButtons = new [] {
            header.Q<Button>("camera"),
            header.Q<Button>("pattern"),
            header.Q<Button>("generator"),
            header.Q<Button>("advanced"),
        };
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
    }


    private void SetupCameraControls(VisualElement body)
    {
        //Field of View
        var fovSlider = body.Q<Slider>("fieldOfView");
        fovSlider.value = _camera.fieldOfView;
        fovSlider.RegisterCallback<ChangeEvent<float>>(e => _camera.fieldOfView = e.newValue);
        
        //Bloom Toggle
        var bloomToggle = body.Q<Toggle>("bloomToggle");
        _post.TryGetSettings(out Bloom bloomA);
        bloomToggle.value = bloomA.enabled.value;
        bloomToggle.RegisterCallback<ChangeEvent<bool>>(e =>
        {
            _post.TryGetSettings(out Bloom bloomB);
            bloomB.enabled.value = e.newValue;
        });
        
        //Bloom Strength
        var bloomStrengthSlider = body.Q<Slider>("bloomStrength");
        _post.TryGetSettings(out Bloom bloomC);
        bloomStrengthSlider.value = bloomC.intensity.value;
        bloomStrengthSlider.RegisterCallback<ChangeEvent<float>>(e =>
        {
            _post.TryGetSettings(out Bloom bloomD);
            bloomD.intensity.value = e.newValue;
        });
        
        //Bloom Threshold
        var bloomThresholdSlider = body.Q<Slider>("bloomThreshold");
        _post.TryGetSettings(out Bloom bloomE);
        bloomThresholdSlider.value = bloomE.threshold.value;
        bloomThresholdSlider.RegisterCallback<ChangeEvent<float>>(e =>
        {
            _post.TryGetSettings(out Bloom bloomF);
            bloomF.threshold.value = e.newValue;
        });
        
        //Pattern Size
        var patternSize = body.Q<Slider>("zoom");
        patternSize.value = _snapToBounds.SizeMultiplier;
        patternSize.RegisterCallback<ChangeEvent<float>>(e => _snapToBounds.SizeMultiplier = e.newValue);
    }

    private void SetupPatternControls(VisualElement body)
    {
        //Line Interval
        var lineInterval = body.Q<Slider>("lineInterval");
        lineInterval.value = _patternOverrides.LineInterval;
        lineInterval.RegisterCallback<ChangeEvent<int>>(e => _patternOverrides.LineInterval = e.newValue);
        
        //Time Step
        var timeStep = body.Q<Slider>("timeStep");
        timeStep.value = _patternOverrides.TimeStep * 1000f;
        timeStep.RegisterCallback<ChangeEvent<float>>(e => _patternOverrides.TimeStep = e.newValue / 1000f);
        
        //Draw Lines
        var drawLines = body.Q<Toggle>("drawLines");
        drawLines.value = _patternOverrides.DrawLines;
        drawLines.RegisterCallback<ChangeEvent<bool>>(e => _patternOverrides.DrawLines = e.newValue);
        
        //Line Opacity
        var lineOpacity = body.Q<Slider>("lineOpacity");
        lineOpacity.value = _patternOverrides.LineOpacity;
        lineOpacity.RegisterCallback<ChangeEvent<float>>(e => _patternOverrides.LineOpacity = e.newValue);
        
        //Draw Lines
        var drawFill = body.Q<Toggle>("drawFill");
        drawFill.value = _patternOverrides.DrawFill;
        drawFill.RegisterCallback<ChangeEvent<bool>>(e => _patternOverrides.DrawFill = e.newValue);
        
        //Line Opacity
        var fillOpacity = body.Q<Slider>("fillOpacity");
        fillOpacity.value = _patternOverrides.FillOpacity;
        fillOpacity.RegisterCallback<ChangeEvent<float>>(e => _patternOverrides.FillOpacity = e.newValue);
        
        //Auto Scale Lines
        var autoScaleLines = body.Q<Toggle>("autoScaleLines");
        autoScaleLines.value = _patternOverrides.AutoScaleLines;
        autoScaleLines.RegisterCallback<ChangeEvent<bool>>(e => _patternOverrides.AutoScaleLines = e.newValue);
    }

    private void SetupGeneratorControls(VisualElement body)
    {
        //3D Mode
        var mode3D = body.Q<Toggle>("3dModeToggle");
        mode3D.value = !_shuffler.Generator.RestrictThirdDimension;
        mode3D.RegisterCallback<ChangeEvent<bool>>(e => _shuffler.Generator.RestrictThirdDimension = !e.newValue);   
        
        //Oscillate Timespan
        var oscillateTimespan = body.Q<Toggle>("oscillateTimespan");
        oscillateTimespan.value = _timeStepper.OscillateTimespan;
        oscillateTimespan.RegisterCallback<ChangeEvent<bool>>(e => _timeStepper.OscillateTimespan = !e.newValue);   
        
        //3D Mode
        var autoShuffle= body.Q<Toggle>("autoShuffle");
        autoShuffle.value = _shuffler.DoAutoShuffle;
        autoShuffle.RegisterCallback<ChangeEvent<bool>>(e => _shuffler.DoAutoShuffle = !e.newValue);   
        
        //Auto Shuffle Period
        var autoShufflePeriod = body.Q<Slider>("autoShufflePeriod");
        autoShufflePeriod.value = _shuffler.AutoShufflePeriod;
        autoShufflePeriod.RegisterCallback<ChangeEvent<float>>(e => _shuffler.AutoShufflePeriod = e.newValue);
        
        //Max Line Count
        var maxLineCount = body.Q<Slider>("maxLineCount");
        maxLineCount.value = _shuffler.Generator.MaxLineCount;
        maxLineCount.RegisterCallback<ChangeEvent<int>>(e => _shuffler.Generator.MaxLineCount = e.newValue);

        //Previous Pattern
        var previousPattern = body.Q<Button>("previousPattern");
        previousPattern.clicked += () => _shuffler.PreviousPattern();
        
        //NextPattern
        var nextPattern = body.Q<Button>("nextPattern");
        nextPattern.clicked += () => _shuffler.NextPattern();
    }
    
    /* TODO:
    private void SetupAdvancedControls(VisualElement body)
    {
    
    }
    */

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

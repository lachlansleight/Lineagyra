using System;
using System.Collections;
using System.Collections.Generic;
using LineCircles;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class MainUiController : MonoBehaviour
{
    private Button[] _navButtons;
    private GameObject[] _pages;
    private GameObject _mainPanel;

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
        _camera = FindObjectOfType<Camera>();
        _lineCircle = FindObjectOfType<LineCircle>();
        _shuffler = FindObjectOfType<Shuffler>();
        _timeStepper = FindObjectOfType<TimeStepper>();
        _snapToBounds = FindObjectOfType<SnapToBounds>();
        _patternOverrides = FindObjectOfType<PatternOverrides>();
        _post = FindObjectOfType<PostProcessVolume>().profile;

        _mainPanel = transform.Find("Container").gameObject;
        
        var mainPanelTransform = _mainPanel.transform;
        SetupContainerControls(mainPanelTransform);
        SetupCameraControls(mainPanelTransform);
        SetupPatternControls(mainPanelTransform);
        SetupGeneratorControls(mainPanelTransform);

        NavigateToPage(0);
        SetMenu(false);
    }

    private void SetupContainerControls(Transform container)
    {
        var navbar = container.Find("Header/Navbar");
        _navButtons = new [] {
            navbar.Find("CameraButton").GetComponent<Button>(),
            navbar.Find("PatternButton").GetComponent<Button>(),
            navbar.Find("GeneratorButton").GetComponent<Button>(),
            navbar.Find("AdvancedButton").GetComponent<Button>(),
        };
        var body = container.Find("Body");
        _pages = new [] {
            body.Find("CameraPanel").gameObject,
            body.Find("PatternPanel").gameObject,
            body.Find("GeneratorPanel").gameObject,
            body.Find("AdvancedPanel").gameObject,
        };

        for (var i = 0; i < _navButtons.Length; i++) {
            var index = i;
            _navButtons[i].onClick.AddListener(() => NavigateToPage(index));
        }
        container.Find("Footer/CloseMenuButton").GetComponent<Button>().onClick.AddListener(() => SetMenu(false));
    }


    private void SetupCameraControls(Transform container)
    {
        var panel = _pages[0].transform;
        
        //Field of View
        var fovSlider = panel.Find("FieldOfView/Slider").GetComponent<Slider>();
        fovSlider.value = _camera.fieldOfView;
        fovSlider.onValueChanged.AddListener((newValue => _camera.fieldOfView = newValue));
        
        //Bloom Toggle
        var bloomToggle = panel.Find("Bloom/Toggle").GetComponent<Toggle>();
        _post.TryGetSettings(out Bloom bloomA);
        bloomToggle.isOn = bloomA.enabled.value;
        bloomToggle.onValueChanged.AddListener(newValue =>
        {
            _post.TryGetSettings(out Bloom bloomB);
            bloomB.enabled.value = newValue;
        });
        
        //Bloom Strength
        var bloomStrengthSlider = panel.Find("BloomStrength/Slider").GetComponent<Slider>();
        _post.TryGetSettings(out Bloom bloomC);
        bloomStrengthSlider.value = bloomC.intensity.value;
        bloomStrengthSlider.onValueChanged.AddListener(newValue =>
        {
            _post.TryGetSettings(out Bloom bloomD);
            bloomD.intensity.value = newValue;
        });
        
        //Bloom Threshold
        var bloomThresholdSlider = panel.Find("BloomThreshold/Slider").GetComponent<Slider>();
        _post.TryGetSettings(out Bloom bloomE);
        bloomThresholdSlider.value = bloomE.threshold.value;
        bloomThresholdSlider.onValueChanged.AddListener(newValue =>
        {
            _post.TryGetSettings(out Bloom bloomF);
            bloomF.threshold.value = newValue;
        });
        
        //Pattern Size
        var patternSize = panel.Find("Zoom/Slider").GetComponent<Slider>();
        patternSize.value = _snapToBounds.SizeMultiplier;
        patternSize.onValueChanged.AddListener(newValue => _snapToBounds.SizeMultiplier = newValue);
    }

    private void SetupPatternControls(Transform container)
    {
        var panel = _pages[1].transform;
        
        //Line Interval
        var lineInterval = panel.Find("LineInterval/Slider").GetComponent<Slider>();
        lineInterval.value = _patternOverrides.LineInterval;
        lineInterval.onValueChanged.AddListener(newValue => _patternOverrides.LineInterval = Mathf.RoundToInt(newValue));
        
        //Time Step
        var timeStep = panel.Find("TimeStep/Slider").GetComponent<Slider>();
        timeStep.value = _patternOverrides.TimeStep * 1000f;
        timeStep.onValueChanged.AddListener(newValue => _patternOverrides.TimeStep = newValue / 1000f);
        
        //Draw Lines
        var drawLines = panel.Find("DrawLines/Toggle").GetComponent<Toggle>();
        drawLines.isOn = _patternOverrides.DrawLines;
        drawLines.onValueChanged.AddListener(newValue => _patternOverrides.DrawLines = newValue);
        
        //Line Opacity
        var lineOpacity = panel.Find("LineOpacity/Slider").GetComponent<Slider>();
        lineOpacity.value = _patternOverrides.LineOpacity;
        lineOpacity.onValueChanged.AddListener(newValue => _patternOverrides.LineOpacity = newValue);
        
        //Draw Lines
        var drawFill = panel.Find("DrawFill/Toggle").GetComponent<Toggle>();
        drawFill.isOn = _patternOverrides.DrawFill;
        drawFill.onValueChanged.AddListener(newValue => _patternOverrides.DrawFill = newValue);
        
        //Line Opacity
        var fillOpacity = panel.Find("FillOpacity/Slider").GetComponent<Slider>();
        fillOpacity.value = _patternOverrides.FillOpacity;
        fillOpacity.onValueChanged.AddListener(newValue => _patternOverrides.FillOpacity = newValue);
        
        //Auto Scale Lines
        var autoScaleLines = panel.Find("AutoScaleLines/Toggle").GetComponent<Toggle>();
        autoScaleLines.isOn = _patternOverrides.AutoScaleLines;
        autoScaleLines.onValueChanged.AddListener(newValue => _patternOverrides.AutoScaleLines = newValue);
    }

    private void SetupGeneratorControls(Transform container)
    {
        var panel = _pages[2].transform;
        
        //3D Mode
        var mode3D = panel.Find("3DMode/Toggle").GetComponent<Toggle>();
        mode3D.isOn = !_shuffler.Generator.RestrictThirdDimension;
        mode3D.onValueChanged.AddListener(newValue => _shuffler.Generator.RestrictThirdDimension = !newValue);   
        
        //Oscillate Timespan
        var oscillateTimespan = panel.Find("OscillateTimespan/Toggle").GetComponent<Toggle>();
        oscillateTimespan.isOn = _timeStepper.OscillateTimespan;
        oscillateTimespan.onValueChanged.AddListener(newValue => _timeStepper.OscillateTimespan = newValue);   
        
        //3D Mode
        var autoShuffle= panel.Find("AutoShuffle/Toggle").GetComponent<Toggle>();
        autoShuffle.isOn = _shuffler.DoAutoShuffle;
        autoShuffle.onValueChanged.AddListener(newValue => _shuffler.DoAutoShuffle = newValue);   
        
        //Auto Shuffle Period
        var autoShufflePeriod = panel.Find("AutoShufflePeriod/Slider").GetComponent<Slider>();
        autoShufflePeriod.value = _shuffler.AutoShufflePeriod;
        autoShufflePeriod.onValueChanged.AddListener(newValue => _shuffler.AutoShufflePeriod = newValue);
        
        //Max Line Count
        var maxLineCount = panel.Find("MaxLineCount/Slider").GetComponent<Slider>();
        maxLineCount.value = _shuffler.Generator.MaxLineCount;
        maxLineCount.onValueChanged.AddListener(newValue => _shuffler.Generator.MaxLineCount = Mathf.RoundToInt(newValue));

        //Previous Pattern
        var previousPattern = panel.Find("PatternNavigation/PreviousPattern").GetComponent<Button>();
        previousPattern.onClick.AddListener(() => _shuffler.PreviousPattern());
        
        //NextPattern
        var nextPattern = panel.Find("PatternNavigation/NextPattern").GetComponent<Button>();
        nextPattern.onClick.AddListener(() => _shuffler.NextPattern());
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
        _mainPanel.SetActive(visible);
        _menuVisible = visible;
    }

    private void NavigateToPage(int page)
    {
        for (var i = 0; i < _navButtons.Length; i++) {
            _navButtons[i].interactable = i != page;
        }

        for (var i = 0; i < _pages.Length; i++) {
            _pages[i].SetActive(i == page);
        }
    }
}

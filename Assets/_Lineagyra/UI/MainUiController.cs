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
    
    private Slider _fovSlider;

    private Dropdown _targetParameterDropdown;
    public GameObject _shapeDropdownParent;
    private Dropdown _shapeDropdown;
    public GameObject _centerSliderParent;
    private Slider _centerSlider;
    private InputField _centerInput;
    public GameObject _amplitudeSliderParent;
    private Slider _amplitudeSlider;
    private InputField _amplitudeInput;
    public GameObject _periodSliderParent;
    private Slider _periodSlider;
    private InputField _periodInput;
    public GameObject _phaseSliderParent;
    private Slider _phaseSlider;
    private InputField _phaseInput;
    public GameObject _lineCountSliderParent;
    public Slider _lineCountSlider;
    public GameObject _sphericalToggleParent;
    public Toggle _sphericalToggle;
    private Text _warningText;

    private Camera _camera;
    private LineCircle _lineCircle;
    private Shuffler _shuffler;
    private TimeStepper _timeStepper;
    private SnapToBounds _snapToBounds;
    private PatternOverrides _patternOverrides;
    private PostProcessProfile _post;
    private CameraControl _cameraControl;
    private LineCirclePauser _pauser;

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
        _cameraControl = FindObjectOfType<CameraControl>();
        _pauser = FindObjectOfType<LineCirclePauser>();

        _mainPanel = transform.Find("Container").gameObject;
        
        var mainPanelTransform = _mainPanel.transform;
        SetupContainerControls(mainPanelTransform);
        SetupCameraControls();
        SetupPatternControls();
        SetupGeneratorControls();
        SetupAdvancedControls();

        _lineCircle.OnPatternChanged += HandlePatternChanged;

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


    private void SetupCameraControls()
    {
        var panel = _pages[0].transform;
        
        //Field of View
        _fovSlider = panel.Find("FieldOfView/Slider").GetComponent<Slider>();
        _fovSlider.value = _camera.fieldOfView;
        _fovSlider.onValueChanged.AddListener((newValue => _camera.fieldOfView = newValue));
        
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
    }

    private void SetupPatternControls()
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

    private void SetupGeneratorControls()
    {
        var panel = _pages[2].transform;
        
        //3D Mode
        var mode3D = panel.Find("3DMode/Toggle").GetComponent<Toggle>();
        mode3D.isOn = !_shuffler.Generator.RestrictThirdDimension;
        mode3D.onValueChanged.AddListener(newValue => _shuffler.Generator.RestrictThirdDimension = !newValue);   
        
        //Oscillate Timespan
        var oscillateTimespan = panel.Find("OscillateTimespan/Toggle").GetComponent<Toggle>();
        oscillateTimespan.isOn = _timeStepper.OscillateTimespan;
        oscillateTimespan.onValueChanged.AddListener(newValue =>
        {
            _timeStepper.OscillateTimespan = newValue;
            _pauser.SetDefaultOscillateTimespan(newValue);
        });   
        
        //Auto Shuffle
        var autoShuffle= panel.Find("AutoShuffle/Toggle").GetComponent<Toggle>();
        autoShuffle.isOn = _shuffler.DoAutoShuffle;
        autoShuffle.onValueChanged.AddListener(newValue =>
        {
            _shuffler.DoAutoShuffle = newValue;
            _pauser.SetDefaultAutoShuffle(newValue);
        });   
        
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

    private void SetupAdvancedControls()
    {
        var panel = _pages[3].transform;

        _targetParameterDropdown = panel.Find("TargetParameter/Dropdown").GetComponent<Dropdown>();
        _targetParameterDropdown.value = 0;
        _targetParameterDropdown.onValueChanged.AddListener(HandleNewTargetParameter);

        _shapeDropdownParent = panel.Find("OscillatorShape").gameObject;
        _shapeDropdown = panel.Find("OscillatorShape/Dropdown").GetComponent<Dropdown>();
        _shapeDropdown.onValueChanged.AddListener(newValue => _lineCircle.Pattern.Oscillators[_targetParameterDropdown.value - 1].Type = (OscillatorShape)newValue);
        
        _centerSliderParent = panel.Find("Center").gameObject;
        _centerSlider = panel.Find("Center/Slider").GetComponent<Slider>();
        _centerInput = panel.Find("Center/InputField").GetComponent<InputField>();
        _centerSlider.onValueChanged.AddListener(newValue => {
            _lineCircle.Pattern.Oscillators[_targetParameterDropdown.value - 1].Center = newValue;
            _centerInput.text = newValue.ToString("0.00");
        });
        _centerInput.onValueChanged.AddListener(newRawValue => {
            if(!float.TryParse(newRawValue, out var newValue)) return;
            _lineCircle.Pattern.Oscillators[_targetParameterDropdown.value - 1].Center = newValue;
            _centerSlider.value = newValue;
        });
        
        _amplitudeSliderParent = panel.Find("Amplitude").gameObject;
        _amplitudeSlider = panel.Find("Amplitude/Slider").GetComponent<Slider>();
        _amplitudeInput = panel.Find("Amplitude/InputField").GetComponent<InputField>();
        _amplitudeSlider.onValueChanged.AddListener(newValue => {
            _lineCircle.Pattern.Oscillators[_targetParameterDropdown.value - 1].Amplitude = newValue;
            _amplitudeInput.text = newValue.ToString("0.00");
        });
        _amplitudeInput.onValueChanged.AddListener(newRawValue => {
            if(!float.TryParse(newRawValue, out var newValue)) return;
            _lineCircle.Pattern.Oscillators[_targetParameterDropdown.value - 1].Amplitude = newValue;
            _amplitudeSlider.value = newValue;
        });
        
        _periodSliderParent = panel.Find("Period").gameObject;
        _periodSlider = panel.Find("Period/Slider").GetComponent<Slider>();
        _periodInput = panel.Find("Period/InputField").GetComponent<InputField>();
        _periodSlider.onValueChanged.AddListener(newValue => {
            _lineCircle.Pattern.Oscillators[_targetParameterDropdown.value - 1].Period = newValue;
            _periodInput.text = newValue.ToString("0.00");
        });
        _periodInput.onValueChanged.AddListener(newRawValue => {
            if(!float.TryParse(newRawValue, out var newValue)) return;
            _lineCircle.Pattern.Oscillators[_targetParameterDropdown.value - 1].Period = newValue;
            _periodSlider.value = newValue;
        });
        
        _phaseSliderParent = panel.Find("Phase").gameObject;
        _phaseSlider = panel.Find("Phase/Slider").GetComponent<Slider>();
        _phaseInput = panel.Find("Phase/InputField").GetComponent<InputField>();
        _phaseSlider.onValueChanged.AddListener(newValue => {
            _lineCircle.Pattern.Oscillators[_targetParameterDropdown.value - 1].Phase = newValue;
            _phaseInput.text = newValue.ToString("0.00");
        });
        _phaseInput.onValueChanged.AddListener(newRawValue => {
            if(!float.TryParse(newRawValue, out var newValue)) return;
            _lineCircle.Pattern.Oscillators[_targetParameterDropdown.value - 1].Phase = newValue;
            _phaseSlider.value = newValue;
        });
        
        _lineCountSliderParent = panel.Find("LineCount").gameObject;
        _lineCountSlider = panel.Find("LineCount/Slider").GetComponent<Slider>();
        _lineCountSlider.onValueChanged.AddListener(newValue => {
            _lineCircle.Pattern.LineCount = Mathf.RoundToInt(newValue);
        });

        _sphericalToggleParent = panel.Find("Spherical").gameObject;
        _sphericalToggle = panel.Find("Spherical/Toggle").GetComponent<Toggle>();
        _sphericalToggle.onValueChanged.AddListener(newValue =>
        {
            _lineCircle.Pattern.SphericalCoordinates = newValue;
        });


        _warningText = panel.Find("Warning/Text").GetComponent<Text>();

        HandleNewTargetParameter(_targetParameterDropdown.value);
    }

    private void HandleNewTargetParameter(int value)
    {
        _lineCountSliderParent.SetActive(value == 0);
        _sphericalToggleParent.SetActive(value == 0);
        
        _shapeDropdownParent.SetActive(value != 0);
        _centerSliderParent.SetActive(value != 0);
        _amplitudeSliderParent.SetActive(value != 0);
        _periodSliderParent.SetActive(value != 0);
        _phaseSliderParent.SetActive(value != 0);
        
        //special case - show the global pattern settings
        if (value == 0) {
            _lineCountSlider.value = _lineCircle.Pattern.LineCount;
            _sphericalToggle.isOn = _lineCircle.Pattern.SphericalCoordinates;
            return;
        }

        value--;
        var oscillator = _lineCircle.Pattern.Oscillators[value];
        
        _shapeDropdown.value = (int)oscillator.Type;

        _centerSlider.value = oscillator.Center;
        _centerInput.text = oscillator.Center.ToString("0.00");

        _amplitudeSlider.value = oscillator.Amplitude;
        _amplitudeInput.text = oscillator.Amplitude.ToString("0.00");
        
        _periodSlider.value = oscillator.Period;
        _periodInput.text = oscillator.Period.ToString("0.00");
        
        _phaseSlider.value = oscillator.Phase;
        _phaseInput.text = oscillator.Phase.ToString("0.00");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) SetMenu(!_menuVisible);

        if (_menuVisible && _pages[3].activeSelf) {
            if (_shuffler.DoAutoShuffle) {
                _warningText.text = $"Warning - Auto Shuffle will discard any changes made in this panel. " +
                                    $"Turn Auto Shuffle off to keep any changes made\n" +
                                    $"Next Auto Shuffle will occur in {(_shuffler.AutoShufflePeriod * (1f - _shuffler.CurrentAutoShuffleTime)):0} seconds";
            } else _warningText.text = "";
        }
    }
    
    private void SetMenu(bool visible)
    {
        _mainPanel.SetActive(visible);
        _menuVisible = visible;

        if (_menuVisible) {
            _fovSlider.value = _camera.fieldOfView;
            HandleNewTargetParameter(_targetParameterDropdown.value);
        }

        //_cameraControl.enabled = !_menuVisible;
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
    
    private void HandlePatternChanged(object sender, EventArgs e)
    {
        HandleNewTargetParameter(_targetParameterDropdown.value);
    }
}

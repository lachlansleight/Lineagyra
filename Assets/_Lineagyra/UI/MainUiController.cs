using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainUiController : MonoBehaviour
{

    private UIDocument _ui;

    private VisualElement _main;
    private Button[] _navButtons;
    private VisualElement[] _pages;

    private bool _menuVisible;
    
    private void OnEnable()
    {
        _ui = GetComponent<UIDocument>();
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

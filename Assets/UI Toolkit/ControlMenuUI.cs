using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlMenu : MonoBehaviour
{

    private Button buttonBack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        buttonBack = uiDocument.rootVisualElement.Q("Back") as Button;        
        buttonBack.RegisterCallback<ClickEvent>(Back);

    }
    private void Back(ClickEvent evnt)
    {
        App.Instance.ExitControls();
        
    }


    private void Update()
    {
        
    }
}

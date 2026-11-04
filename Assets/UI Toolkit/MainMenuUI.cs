using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{

    private VisualElement container;
    private Button buttonPlay;
    private Button buttonControls;
    private Button buttonQuit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        container = uiDocument.rootVisualElement.Q("Container") as VisualElement;
        buttonPlay = uiDocument.rootVisualElement.Q("Play") as Button;        
        buttonControls = uiDocument.rootVisualElement.Q("Controls") as Button;        
        buttonQuit = uiDocument.rootVisualElement.Q("Quit") as Button;
        buttonPlay.RegisterCallback<ClickEvent>(Play);
        buttonControls.RegisterCallback<ClickEvent>(Controls);
        buttonQuit.RegisterCallback<ClickEvent>(Quit);

    }

    private void OnDisable()
    {
//        this.container.AddToClassList("hide");        
    }
    private void Play(ClickEvent evnt)
    {
        App.Instance.StartGame();
        
    }
    private void Controls(ClickEvent evnt)
    {
        App.Instance.Controls();
    }

    private void Quit(ClickEvent evnt)
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }


}

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
        buttonPlay.clicked += Play;
        buttonControls.clicked += Controls;
        buttonQuit.clicked += Quit;

    }

    private void Start()
    {
        // Start with a focused button so joystick/D-pad navigation has a starting point
        buttonPlay.Focus();
    }

    private void OnDisable()
    {
//        this.container.AddToClassList("hide");        
    }
    private void Play()
    {
        App.Instance.StartGame();
        
    }
    private void Controls()
    {
        App.Instance.Controls();
    }

    private void Quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }


}

using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = System.Numerics.Vector3;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton, quitButton,settingButton,LoadGameButton;

    public ObjectEventSO newGameEvent;
    
    [Header("广播")]
    public ObjectEventSO ButtonHoverEvent;
    public ObjectEventSO settingButtonEvent;
    public ObjectEventSO MoveCameraEvent;
    public ObjectEventSO LoadGameEvent;


    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        newGameButton = rootElement.Q<Button>("NewGameButton");
        quitButton = rootElement.Q<Button>("QuitButton");
        settingButton = rootElement.Q<Button>("Setting");
        LoadGameButton = rootElement.Q<Button>("LoadGame");

        newGameButton.clicked += OnNewGameButtonClicked;
        quitButton.clicked += OnQuitButtonClicked;
        settingButton.clicked += OnSettingButtonClicked;
        LoadGameButton.clicked += OnLoadGameButtonClicked;
        
        newGameButton.RegisterCallback<MouseEnterEvent>(OnMouseHover);
        newGameButton.clicked += OnMoveCameraEvent;
        quitButton.RegisterCallback<MouseEnterEvent>(OnMouseHover);
        if (!SaveManager.instance.HasSaveFile)
        {
            LoadGameButton.SetEnabled(false);
        }
        
    }
    
    
    
    private void OnLoadGameButtonClicked()
    {
        LoadGameEvent.RaiseEvent(null,this);
        
    }

    private void OnMoveCameraEvent()
    {
        Invoke("DelayOnMoveCameraEvent",0.5f);
    }

    private void DelayOnMoveCameraEvent()
    {
        MoveCameraEvent.RaiseEvent(null,this);
    }

    private void OnSettingButtonClicked()
    {
        settingButtonEvent.RaiseEvent(null,this);
        
    }
    private void OnMouseHover(MouseEnterEvent evt)
    {
        ButtonHoverEvent.RaiseEvent(null,this);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    private void OnNewGameButtonClicked()
    {
        newGameEvent.RaiseEvent(null, this);
    }
}

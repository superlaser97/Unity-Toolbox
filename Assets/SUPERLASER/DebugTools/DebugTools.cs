using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DebugTools : MonoBehaviour
{
    private static DebugTools instance;
    public static DebugTools Instance
    {
        get
        {
            if (instance)
                return instance;
            else
            {
                return Instantiate(Resources.Load<GameObject>("SUPERLASER/DebugTools/DebugToolsCanvas")).GetComponent<DebugTools>();
            }
        }
    }

    private Vector2 touch0InitialPos = Vector2.zero;
    private Vector2 touch1InitialPos = Vector2.zero;
    private Vector2 touch2InitialPos = Vector2.zero;

    [Header("UI References")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private TMP_InputField commandInputField;
    [SerializeField] private Button sendCmdBtn;
    [SerializeField] private GameObject quickActionBtnPanel;
    [SerializeField] private GameObject quickActionBtn_Prefab;

    [Header("Text Peferences")]
    [SerializeField] private Color normalTextColor;
    [SerializeField] private Color warningTextColor;
    [SerializeField] private Color errorTextColor;
    [SerializeField] private Color commandTextColor;

    [Header("Log Peferences")]
    [SerializeField] private bool enableTimestamp = true;

    [Header("Key Bindings")]
    [NaughtyAttributes.ReadOnly]
    [SerializeField] private KeyCode keyModifier1 = KeyCode.LeftShift;
    [NaughtyAttributes.ReadOnly]
    [SerializeField] private KeyCode keyModifier2 = KeyCode.Z;
    [NaughtyAttributes.ReadOnly]
    [SerializeField] private KeyCode showDebugToolsConsoleKey = KeyCode.D;

    private List<CommandEventLink> cmdEventLinks;
    private DebugCommandMethods debugCmdM = new DebugCommandMethods();
    
    // Gesture
    public enum DebugLevel { NORMAL, WARNING, ERROR, COMMAND };
    private enum GestureDir { NULL, UP, DOWN, LEFT, RIGHT };
    private bool recordingGesture = false;
    private bool gestureActionExecuted = false;

    // Guesture Activation Dist
    float yActivationDist = Screen.height / 3f;
    float xActivationDist = Screen.width / 3f;

    // Msgs that are logged before debug console starts
    private static string preInstanceMsgs = string.Empty;
    private string consoleParam = string.Empty;

    private void OnEnable()
    {
        if (instance)
        {
            Debug.Log("Duplicate Debug Tools Found");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void InitalizeSelf()
    {
        var debugConsole = Instance;
        Log("Debug Tools Console Started, type (list cmd) to list all commands");
    }

#if UNITY_EDITOR
    [MenuItem("Helpers/Debug Tools/Open Logs in Explorer")]
    public static void OpenConfigFileInExplorer()
    {
        Application.OpenURL(Path.Combine(Application.persistentDataPath, "Logs"));
    }
#endif

    private void Start()
    {
        panel.gameObject.SetActive(false);

        InitializeCmdEvenLinks();

        sendCmdBtn.onClick.AddListener(delegate { OnSendCmdBtnClick(); });
        commandInputField.onEndEdit.AddListener(delegate { OnSendCmdBtnClick(); });

        if (preInstanceMsgs != string.Empty)
            Log(preInstanceMsgs, DebugLevel.WARNING);
    }

    public List<string> GetEntireCommandList()
    {
        List<string> cmdList = new List<string>();
        foreach (CommandEventLink cmdELink in cmdEventLinks)
        {
            cmdList.Add(cmdELink.BaseCommand + "\n" + cmdELink.CommandDescription);
        }
        return cmdList;
    }

    private void InitializeCmdEvenLinks()
    {
        cmdEventLinks = new List<CommandEventLink>
        {
            // Base Commands
            new CommandEventLink(
                "set fps", // Base Command
                delegate{ debugCmdM.SetFPS(consoleParam); }, // Delegate for executing command
                "set fps|<fps integer>" + "\n" + // Descriptions
                "Sets fps, clamped from 10 - 120" + "\n" +
                "e.x. set fps|60",
                false, // Show QuickActionButton in DebugTools Console
                true // Command with Parameters
                ),

            new CommandEventLink(
                "list cmd",
                delegate{ debugCmdM.ListCmd(); },
                "Lists entire command list",
                true,
                false
                ),

            new CommandEventLink(
                "set active",
                delegate{ debugCmdM.GameObjectSetActive(consoleParam); },
                "set active|<GameObject name>,<boolean>" + "\n" +
                "Set GameObject Active State, sets all gameobject with same name" + "\n" +
                "e.x set active|cube,true",
                false,
                true
                ),

            new CommandEventLink(
                "send message",
                delegate{ debugCmdM.SendMessageToAllGO(consoleParam); },
                "Send Message to all GameObjects -> " + "\n" +
                "send message|<GameObject name>,<string message>" + "\n" +
                "send message|<string message>" + "\n" +
                "Send message to specific gameobject or all gameobjects" + "\n" +
                "e.x send message|Cube,Explode" + "\n" +
                "e.x send message|Explode",
                false,
                true
                ),

            new CommandEventLink(
                "set font size",
                delegate{ debugCmdM.SetConsoleFontSize(consoleParam); },
                "set font size|<Font size integer>" + "\n" +
                "Sets font size of debug tools console" + "\n" +
                "e.x set font size|50",
                true,
                true
                ),
            new CommandEventLink(
                "quit app",
                delegate{ Application.Quit(); },
                "Exits the app",
                true,
                false
                ),
        };

        foreach (CommandEventLink link in cmdEventLinks)
        {
            if (link.QuickActionButton == false)
                continue;

            GameObject btn = Instantiate(quickActionBtn_Prefab, quickActionBtnPanel.transform);
            btn.GetComponentInChildren<TMP_Text>().text = link.BaseCommand;

            if (link.HasParameters)
                btn.GetComponent<Button>().onClick.AddListener(delegate { commandInputField.text = link.BaseCommand + "|"; });
            else
                btn.GetComponent<Button>().onClick.AddListener(delegate { commandInputField.text = link.BaseCommand; });
        }
    }

    private void Update()
    {
        GestureTrackingUpdate();
        KeybindingUpdate();
    }

    private void KeybindingUpdate()
    {
        if (!Input.anyKey)
            return;

        if (!Input.GetKey(keyModifier1) || !Input.GetKey(keyModifier2))
            return;

        if (Input.GetKeyDown(showDebugToolsConsoleKey))
        {
            ShowDebugToolsConsole();
        }
    }

    private void GestureTrackingUpdate()
    {
        if (Input.touchCount != 3)
        {
            recordingGesture = false;
            gestureActionExecuted = false;
            return;
        }

        if (!recordingGesture)
        {
            RecordInitialTouchPos();
            recordingGesture = true;
        }
        else
        {
            GestureDir currGestureDir = GetCurrentGestureDirection();
            if (currGestureDir != GestureDir.NULL && !gestureActionExecuted)
            {
                ExecuteGestureAction(currGestureDir);
            }
        }
    }

    private void ExecuteGestureAction(GestureDir currGestureDir)
    {
        switch (currGestureDir)
        {
            case GestureDir.UP:
                break;
            case GestureDir.DOWN:
                ShowDebugToolsConsole();
                break;
            case GestureDir.LEFT:
                break;
            case GestureDir.RIGHT:
                break;
        }
        gestureActionExecuted = true;
    }

    private GestureDir GetCurrentGestureDirection()
    {
        Vector2 currTouchPt0 = Input.GetTouch(0).position;
        Vector2 currTouchPt1 = Input.GetTouch(1).position;
        Vector2 currTouchPt2 = Input.GetTouch(2).position;

        if (
            Mathf.Abs(touch0InitialPos.y - currTouchPt0.y) > yActivationDist &&
            Mathf.Abs(touch1InitialPos.y - currTouchPt1.y) > yActivationDist &&
            Mathf.Abs(touch2InitialPos.y - currTouchPt2.y) > yActivationDist
            )
        {
            if (Mathf.Max(touch0InitialPos.y, currTouchPt0.y) == touch0InitialPos.y)
            {
                return GestureDir.DOWN;
            }
            else
            {
                return GestureDir.UP;
            }
        }

        if (
            Mathf.Abs(touch0InitialPos.x - currTouchPt0.x) > xActivationDist &&
            Mathf.Abs(touch1InitialPos.x - currTouchPt1.x) > xActivationDist &&
            Mathf.Abs(touch2InitialPos.x - currTouchPt2.x) > xActivationDist
            )
        {
            if (Mathf.Max(touch0InitialPos.x, currTouchPt0.x) == touch0InitialPos.x)
            {
                return GestureDir.LEFT;
            }
            else
            {
                return GestureDir.RIGHT;
            }
        }

        return GestureDir.NULL;
    }

    private void ShowDebugToolsConsole()
    {
        panel.gameObject.SetActive(true);
    }

    private void HideDebugToolsConsole()
    {
        panel.gameObject.SetActive(false);
    }

    public void SetDebugFontSize(int size)
    {
        debugText.fontSize = size;
    }

    private void ResetInitialTouchPos()
    {
        touch0InitialPos = Vector2.zero;
        touch1InitialPos = Vector2.zero;
        touch2InitialPos = Vector2.zero;
    }

    private void RecordInitialTouchPos()
    {
        touch0InitialPos = Input.GetTouch(0).position;
        touch1InitialPos = Input.GetTouch(1).position;
        touch2InitialPos = Input.GetTouch(2).position;
    }

    public static void Log(string msg, DebugLevel debugLevel = DebugLevel.NORMAL)
    {
        if (instance)
            instance.AddDebugText(msg, debugLevel);
        else
        {
            string timestamp = DateTime.Now.ToString("hh:mm:ss");
            preInstanceMsgs += "(" + timestamp + ") " + msg + "\n";
        }
    }

    private void AddDebugText(string msg, DebugLevel debugLevel = DebugLevel.NORMAL)
    {
        Color msgColor = normalTextColor;

        switch (debugLevel)
        {
            case DebugLevel.NORMAL:
                msgColor = normalTextColor;
                break;
            case DebugLevel.WARNING:
                msgColor = warningTextColor;
                break;
            case DebugLevel.ERROR:
                msgColor = errorTextColor;
                break;
            case DebugLevel.COMMAND:
                msgColor = commandTextColor;
                break;
            default:
                break;
        }
        string timestamp = "";

        if (enableTimestamp)
            timestamp = "(" + DateTime.Now.ToString("hh:mm:ss") + ") ";

        debugText.text += "<#" + ColorUtility.ToHtmlStringRGB(msgColor) + ">" + timestamp + msg + "\n";

        switch (debugLevel)
        {
            case DebugLevel.NORMAL:
                Debug.Log(msg);
                break;
            case DebugLevel.WARNING:
                Debug.LogWarning(msg);
                break;
            case DebugLevel.ERROR:
                Debug.LogError(msg);
                break;
            case DebugLevel.COMMAND:
                Debug.Log(msg);
                break;
            default:
                Debug.Log(msg);
                break;
        }
    }

    public void ProcessDebugCommand(string cmd)
    {
        Log("COMMAND: " + cmd, DebugLevel.COMMAND);

        string baseCommand = string.Empty;

        if (cmd.Contains("|"))
        {
            string[] splitCmd = cmd.Split('|');
            baseCommand = splitCmd[0];
            consoleParam = splitCmd[1];
        }
        else
        {
            baseCommand = cmd;
        }

        if (cmdEventLinks.Exists(x => x.BaseCommand == baseCommand.ToLower()))
        {
            cmdEventLinks.Find(x => x.BaseCommand == baseCommand).TargetMethod.Invoke();
            consoleParam = string.Empty;
        }
        else
        {
            Log("INVALID COMMAND", DebugLevel.ERROR);
        }
    }

    private void OnSendCmdBtnClick()
    {
        ProcessDebugCommand(commandInputField.text);
        commandInputField.text = "";
    }

    public void ClearLog()
    {
        debugText.text = string.Empty;
    }

    public int GetKeyboardSize()
    {
        using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

            using (AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
            {
                View.Call("getWindowVisibleDisplayFrame", Rct);

                return Screen.height - Rct.Call<int>("height");
            }
        }
    }
}

[System.Serializable]
public class CommandEventLink
{
    public CommandEventLink(
        string baseCommand,
        UnityAction targetMethod,
        string commandDescription,
        bool quickActionButton,
        bool hasParameters
        )
    {
        BaseCommand = baseCommand;
        TargetMethod = targetMethod;
        CommandDescription = commandDescription;
        QuickActionButton = quickActionButton;
        HasParameters = hasParameters;
    }

    public string BaseCommand { get; set; }
    public UnityAction TargetMethod { get; set; }
    public string CommandDescription { get; set; }
    public bool QuickActionButton { get; set; }
    public bool HasParameters { get; set; }
}
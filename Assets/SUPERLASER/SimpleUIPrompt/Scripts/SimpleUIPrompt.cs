using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SUPERLASER
{
    public class SimpleUIPrompt : MonoBehaviour
    {
        // Change this if change where the prefab is located
        private const string PREFAB_PATH = "SUPERLASER/SimpleUIPrompt/SimpleUIPromptCanvas";

        private static SimpleUIPrompt instance;
        public static SimpleUIPrompt Instance
        {
            get
            {
                if (instance)
                    return instance;
                else
                {
                    GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
                    if (prefab)
                        return instance = Instantiate(prefab).GetComponent<SimpleUIPrompt>();
                    else
                    {
                        Debug.LogError($"Cant find SimpleUIPrompt at {PREFAB_PATH}");
                        Debug.LogError("Read README.docx, you might have forgotten to copy something");
                        return null;
                    }
                }
            }
        }
        [Space(10)]

        [SerializeField] private GameObject dialogPanelBG;
        [SerializeField] private UIAnimator dialogPanelAnimator;
        [SerializeField] private TextMeshProUGUI dialogTitle;
        [SerializeField] private TextMeshProUGUI dialogContent;
        [SerializeField] private GameObject actionBtn_Prefab;
        [SerializeField] private Button closeDialogBtn;
        [SerializeField] private RectTransform actionBtnPanel;
        
        [Space(10)]

        [SerializeField] private Color accentColor;
        [SerializeField] private float animationSpd = 20;

        private class SimpleUIDialogContent
        {
            public string dialogTitleString;
            public string dialogContentString;
            public List<SimpleUIDialogAction> uiDialogActions;
            public bool showCloseDialogBtn = true;
            public int highlightBtn = 0;
        }

        private Queue<SimpleUIDialogContent> uiDialogContentQueue = new Queue<SimpleUIDialogContent>();

        private bool activeDialog = false;

        private void OnEnable()
        {
            if (!instance)
                instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            dialogPanelAnimator.ScaleAnimSpd = animationSpd;
            gameObject.name = "SimpleUIPromptCanvas";
        }

        private void Start()
        {
            closeDialogBtn.onClick.AddListener(delegate { CloseDialog(); });
        }

        private void Update()
        {
            if (!activeDialog && uiDialogContentQueue.Count > 0)
                ShowDialogFromQueue();
        }

        // Pop up the prompt with parameters, prompt will be instantiated if not found
        public static void ShowDialog(
            string dialogTitleString,
            string dialogContentString,
            List<SimpleUIDialogAction> uiDialogActions = null,
            int highlightBtn = 0,
            bool closeDialogBtn = true
            )
        {
            SimpleUIDialogContent dialogContent = new SimpleUIDialogContent
            {
                dialogTitleString = dialogTitleString,
                dialogContentString = dialogContentString,
                uiDialogActions = uiDialogActions,
                showCloseDialogBtn = closeDialogBtn,
                highlightBtn = highlightBtn,
            };

            Instance.uiDialogContentQueue.Enqueue(dialogContent);
        }

        private void ShowDialogFromQueue()
        {
            SimpleUIDialogContent targetUIDContent = uiDialogContentQueue.Dequeue();

            activeDialog = true;
            dialogPanelBG.SetActive(true);
            dialogPanelAnimator.Animate_Scale(UIAnimator.Location.END);
            dialogTitle.text = targetUIDContent.dialogTitleString;
            dialogContent.text = targetUIDContent.dialogContentString;


            foreach (Transform obj in actionBtnPanel.GetComponent<Transform>())
            {
                if (obj.gameObject == actionBtnPanel)
                    continue;

                Destroy(obj.gameObject);
            }

            if (targetUIDContent.uiDialogActions != null)
            {
                for (int i = 0; i < targetUIDContent.uiDialogActions.Count; i++)
                {
                    GameObject newBtn = Instantiate(actionBtn_Prefab, actionBtnPanel);
                    Button newBtn_Btn = newBtn.GetComponentInChildren<Button>();

                    newBtn_Btn.onClick.AddListener(delegate { CloseDialog(); });
                    newBtn_Btn.onClick.AddListener(targetUIDContent.uiDialogActions[i].action);

                    TextMeshProUGUI newBtn_Text = newBtn.GetComponentInChildren<TextMeshProUGUI>();
                    newBtn_Text.text = targetUIDContent.uiDialogActions[i].buttonText;

                    Image newBtn_Img = newBtn.GetComponentInChildren<Image>();
                    if (
                        targetUIDContent.highlightBtn != 0 &&
                        targetUIDContent.uiDialogActions.Count >=
                        targetUIDContent.highlightBtn && i + 1 ==
                        targetUIDContent.highlightBtn)
                    {
                        newBtn_Img.color = accentColor;
                        newBtn_Text.color = Color.white;
                    }
                }
            }

            if (targetUIDContent.showCloseDialogBtn)
                closeDialogBtn.gameObject.SetActive(true);
            else
                closeDialogBtn.gameObject.SetActive(false);
        }

        private void CloseDialog()
        {
            activeDialog = false;
            dialogPanelBG.SetActive(false);
            dialogPanelAnimator.Animate_Scale(UIAnimator.Location.INITIAL);
        }

        // Destroys the Dialog GameObject from scene to free up memory
        public static void Dispose()
        {
            if (instance != null)
                Destroy(instance.gameObject);
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }

    public class SimpleUIDialogAction
    {
        public UnityAction action;
        public string buttonText;

        public SimpleUIDialogAction(UnityAction action, string buttonText)
        {
            this.action = action;
            this.buttonText = buttonText;
        }
    }
}

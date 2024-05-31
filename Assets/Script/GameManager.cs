using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    /*
        GameManager
            
        UIを1つのスクリプトでまとめて参照させる
        
        各スクリプトの
     
     */

    [SerializeField] private GameConstants gameConstants;
    private static GameManager instance;

    [SerializeField] private StickController stickController;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private GaugeController gaugeController;
    [SerializeField] private LongPressButton longPressButton;
    [SerializeField] private PauseButton pauseButton;


    //--------------------------------------------------------------------------------
    //プレイヤー関連のUI
    //--------------------------------------------------------------------------------
   
    [SerializeField] private Image[] gaugeImages;
    [SerializeField] private Image[] roomImages;
    [SerializeField] private Button[] buttons;

    //--------------------------------------------------------------------------------
    [SerializeField] private Text counterText;
    [SerializeField] private Text timerText;
    //--------------------------------------------------------------------------------
    [SerializeField] private Image pausePanel;

    //--------------------------------------------------------------------------------
    // デリゲートの定義
    private Action<int, int> redHide;
    private Action<int, int> redShow;
    private Action<int, int> purpleHide;
    private Action<int, int> purpleShow;
    private Action<int, int> whiteHide;
    private Action<int, int> whiteShow;
    private Action<int, int> pauseHide;
    private Action<int, int> pauseShow;

    // デリゲートにアクセスするためのプロパティ
    public Action<int, int> RedHide => redHide;
    public Action<int, int> RedShow => redShow;
    public Action<int, int> PurpleHide => purpleHide;
    public Action<int, int> PurpleShow => purpleShow;
    public Action<int, int> WhiteHide => whiteHide;
    public Action<int, int> WhiteShow => whiteShow;
    public Action<int,int> PauseHide => pauseHide;
    public Action<int, int> PauseShow => pauseShow;
    public Text CounterText => counterText;
    public Text TimerText => timerText;
    public Image[] GaugeImages => gaugeImages;
    public Image[] RoomImages => roomImages;
    public Button[] Buttons => buttons;
    public Image PausePanel => pausePanel;

    public void FunctionNameConversion()
    {
        // デリゲート変数に関数を代入
        redHide = RedHideImages;
        redShow = RedShowImages;
        purpleHide = PurpleHideImages;
        purpleShow = PurpleShowImages;
        whiteHide = WhiteHideImages;
        whiteShow = WhiteShowImages;
        pauseHide = PauseHideImages;
        pauseShow = PauseShowImages;
    }
    public void InvokeAction(Action<int, int> action, int start, int end, float delay)
    {
        StartCoroutine(InvokeAfterDelay(action, start, end, delay));
    }

    private IEnumerator InvokeAfterDelay(Action<int, int> action, int start, int end, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke(start, end);
    }

    public void AllHideImages()
    {
        foreach (var image in gaugeImages)
        {
            if (image != null)
            {
                image.enabled = false;
            }
        }
        foreach (var button in buttons)
        {
            if (button != null)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    public void HideImagesInRange(int start, int end)
    {
        if (start < 0 || end >= gaugeImages.Length)
        {
            Debug.LogError("Invalid range provided.");
            return;
        }
        

        for(int i = start; i <= end; i++)
        {
            if (gaugeImages[i] != null)
            {
                gaugeImages[i].enabled = false;
            }
        }
    }

    public void HideButtonsInRange(int start, int end)
    {
        if (start < 0 || end >= buttons.Length)
        {
            Debug.LogError("Button Invalid range provided.");
            return;
        }

        for(int i = start; i <= end; ++i)
        {
            if (buttons[i] != null)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
    public void ShowImagesInRange(int start, int end)
    {
        if (start < 0 || end >= gaugeImages.Length)
        {
            Debug.LogError("Invalid range provided.");
            return;
        }

        // すべての画像を一旦非表示にする
        //HideImagesInRange (gameConstants.NeverDisplayImage, gameConstants.EndDisplayImage);

        // 指定された範囲内の画像のみを表示する
        for (int i = start; i <= end; i++)
        {
            if (GaugeImages[i] != null)
            {
                GaugeImages[i].enabled = true;  // 画像を表示する
            }
        }
        
    }

    public void ShowButtonInRange(int start, int end)
    {
        if (start < 0 || end >= buttons.Length)
        {
            Debug.LogError("Invalid range provided.");
            return;
        }
        // すべての画像を一旦非表示にする
        //HideImagesInRange(gameConstants.NeverDisplayButton, gameConstants.EndDisplayButton);

        // 指定された範囲内の画像のみを表示する
        for (int i = start; i <= end; i++)
        {
            if (buttons[i] != null)
            {
                buttons[i].gameObject.SetActive(true);
            }
        }
    }

    public void RedHideImages(int start, int end)
    {
        if (start < 0 || end >= gaugeImages.Length)
        {
            Debug.LogError("Invalid range provided.");
            return;
        }
        for(int i = start; i <= end;i++)
        {
            if (gaugeImages[i] != null)
            {
                gaugeImages[i].enabled = false;
            }
        }
        if (buttons[gameConstants.StartDisplayButtonRed] != null)
        {
            buttons[gameConstants.StartDisplayButtonRed].gameObject.SetActive(false);
        }
        if (buttons[gameConstants.EndDisplayButtonRed] != null)
        {
            buttons[gameConstants.EndDisplayButtonRed].gameObject.SetActive(false);
        }
    }

    public void RedShowImages(int start, int end)
    {
        if (start < 0 || end >= gaugeImages.Length)
        {
            Debug.LogError("Invalid range provided.");
            return;
        }
        for (int i = start; i <= end; i++)
        {
            if (gaugeImages[i] != null)
            {
                gaugeImages[i].enabled = true;
            }
        }
        if (buttons[gameConstants.RedButtonRight] != null)
        {
            buttons[gameConstants.RedButtonRight].gameObject.SetActive(true);
            buttons[gameConstants.RedButtonRight].interactable = true;
        }
        if (buttons[gameConstants.RedButtonLeft] != null)
        {
            buttons[gameConstants.RedButtonLeft].gameObject.SetActive(true);
            buttons[gameConstants.RedButtonLeft].interactable= true;
        }
        if (buttons[gameConstants.RedCardButton] != null)
        {
            buttons[gameConstants.RedCardButton].gameObject.SetActive(true);
        }
    }

    public void PurpleHideImages(int start,int end)
    {
        for (int i = start; i <= end; i++)
        {
            if (gaugeImages[i] != null)
            {
                gaugeImages[i].fillAmount = 0;
                gaugeImages[i].enabled = false;
            }
        }
    }

    public void PurpleShowImages(int start,int end)
    {
        for(int i = start; i <= end;i++)
        {
            if (gaugeImages[i] != null)
            {
                gaugeImages[i].enabled = true;
            }
        }
    }


    public void WhiteHideImages(int start, int end)
    {
        if (start < 0 || end >= gaugeImages.Length)
        {
            Debug.LogError("Invalid range provided.");
            return;
        }
        for (int i = start; i <= end; i++)
        {
            if (gaugeImages[i] != null)
            {
                gaugeImages[i].fillAmount = 0.0f;
                gaugeImages[i].enabled = false;
            }
        }
        if (buttons[gameConstants.DisplayButtonWhite] != null)
        {
            buttons[gameConstants.DisplayButtonWhite].gameObject.SetActive(false);
        }
    }

    public void WhiteShowImages(int start,int end)
    {
        if (start < 0 || end >= gaugeImages.Length)
        {
            Debug.LogError("Invalid range provided.");
            return;
        }

        for(int i = start; i <= end; i++)
        {
            if (gaugeImages[i] != null)
            {
                gaugeImages[i].enabled = true;
            }
        }
        if (buttons[gameConstants.DisplayButtonWhite] != null)
        {
            buttons[gameConstants.DisplayButtonWhite].gameObject.SetActive(true);
        }
    }

    public void PauseHideImages(int start,int end)
    {
        if (start < 0 || end >= gaugeImages.Length)
        {
            Debug.LogError("Invalid range provided.");
            return;
        }
        if (PausePanel != null)
        {
            PausePanel.gameObject.SetActive(false);
        }
        for(int i = start; i <= end;i++)
        {
            if (buttons[i] != null)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void PauseShowImages(int start,int end)
    {
        if (start < 0 || end >= gaugeImages.Length)
        {
            Debug.LogError("Invalid range provided.");
            return;
        }
        if (PausePanel != null)
        {
            PausePanel.gameObject.SetActive(true);
        }
        for (int i = start; i <= end; i++)
        {
            if (buttons[i] != null)
            {
                buttons[i].gameObject.SetActive(true);
            }
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    Debug.LogError("GameManager instance not found in the scene.");
                }
                else
                {
                    DontDestroyOnLoad(instance.gameObject);
                    Debug.Log("GameManager instance set to DontDestroyOnLoad.");
                }
            }
            return instance;
        }
    }

    public StickController GetStickController() => stickController;
    public AnimationController GetAnimationController() => animationController;
    public GaugeController GetGaugeController() => gaugeController;
    public LongPressButton GetLongPressButton() => longPressButton;
    public PauseButton GetPauseButton() => pauseButton;

    private void Awake()
    {
       FunctionNameConversion();
       HideImagesInRange(gameConstants.NeverDisplayImage, gameConstants.EndDisplayImage);
       HideButtonsInRange(gameConstants.NeverDisplayButton, gameConstants.EndDisplayButton);
    }


    // コルーチンを開始するメソッド
    //public void StartAnimation(float waitTime, string controllerName)
    //{
    //    StartCoroutine(AnimationCoroutine(waitTime, controllerName));
    //}

    //private IEnumerator AnimationCoroutine(float waitTime, string controllerName)
    //{
    //    yield return new WaitForSeconds(waitTime);

    //    if (showImageActions.TryGetValue(controllerName, out var showImages))
    //    {
    //        showImages?.Invoke();
    //    }
    //    else
    //    {
    //        Debug.LogError("Invalid controller name specified.");
    //    }
    //}

    
    private void Start()
    {
        Debug.Log("GameManager Start method called.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

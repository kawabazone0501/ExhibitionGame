using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class GaugeController : MonoBehaviour
{
    public AudioSource audioSource;

    private Animator SeitoRed;
    private Animator SeitoPurple;
    private Animator SeitoWhite;
    private Animator Phone;
    private Animator GameClearPanel;
  
    public Button redCardButton; // FillAmountを増やす対象のボタン

    [SerializeField] private Image redCardImage; // FillAmountを調整するためのイメージ
    public Image RedCardImage=> redCardImage;

    [SerializeField]
    private Text counterText;

    private Coroutine purpleCoroutine; // purpleCoroutineの参照を保持する

    [SerializeField]
    private GameConstants gameConstants;
    [SerializeField]
    private GameStateManager gameStateManager;


    

    private int Bonus = 0;

    private int score = 0;
    public int Score => score;
    
    //プレイヤーのゲージ用Image 
    [SerializeField] private Image gaugeImage;
    //赤の生徒のゲージ用Images
    [SerializeField] private Image[] redGaugeImages;
    [SerializeField] private Button redButtonLeft;
    [SerializeField] private Button redButtonRight;
  
    public Image GaugeImage => gaugeImage;
    public Image[] RedGaugeImages => redGaugeImages;
    public Button RedButtonLeft => redButtonLeft;
    public Button RedButtonRight => redButtonRight;


    //白の生徒のボタン
    [SerializeField]
    private Button WhiteButton;
    // ゲージのImage
    [SerializeField]
    private Image whiteGaugeImage;


    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null in Start.");
        }
        else
        {
            Debug.Log("GameManager instance is found in Start.");
        }
        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();
        AnimatorController animatorController = FindAnyObjectByType<AnimatorController>();
        if (animatorController != null)
        {
            SeitoRed = animatorController.SeitoRed;
            SeitoPurple = animatorController.SeitoPurple;
            SeitoWhite = animatorController.SeitoWhite;
            Phone = animatorController.Phone;
            GameClearPanel = animatorController.GameClearPanel;
        }
        // ターゲットボタンのImageコンポーネントを取得
        redCardImage = redCardButton.GetComponent<Image>();
       
        
    }
    void Start()
    {
        audioSource.loop = true;
        // オーディオを再生する
        audioSource.Play();
        redCardButton.interactable = false;
    }

    void Update()
    {
        // ボタンがクリックされていないときのみ減少させる
        if (!gameStateManager.IsButtonClicked && gameStateManager.IsStudent && !gameStateManager.IsClear)
        {
            DecreaseGauge();
        }
        else if (gaugeImage.fillAmount > 0.0f && !gameStateManager.IsClear)
        {
            DecreaseGauge();
        }
    }

    //画像を非表示にする関数(赤の生徒のみ)
    public void HideImages()
    {
        foreach (var image in redGaugeImages)
        {
            if(image != null)
            {
                image.enabled = false;
            }
        }
        if(redButtonLeft != null)
        {
            redButtonLeft.gameObject.SetActive(false);
        }
        if(redButtonRight != null)
        {
            redButtonRight.gameObject.SetActive(false);
        }
    }

    public void ShowImages()
    {
        ShowImagesInRange(1, 3);
    }

    //赤の生徒の特定の画像を表示する関数
    public void ShowImagesInRange(int start, int end)
    {
        if (start < 0 || end >= redGaugeImages.Length)
        {
            Debug.LogError("Invalid range provided.");
            return;
        }

        // すべての画像を一旦非表示にする
        HideImages();

        // 指定された範囲内の画像のみを表示する
        for (int i = start; i <= end; i++)
        {
            if (redGaugeImages[i] != null)
            {
                redGaugeImages[i].enabled = true;  // 画像を表示する
            }
        }
        if (redButtonLeft != null)
        {
            redButtonLeft.gameObject.SetActive(true);  // ボタン1を表示する
        }
        if (redButtonRight != null)
        {
            redButtonRight.gameObject.SetActive(true);  // ボタン2を表示する
        }
    }

    // 特定の範囲の画像を非表示にする関数
    public void HideImagesInRange(int start, int end)
    {
        if (start < 0 || end >= redGaugeImages.Length)
        {
            Debug.LogError("Invalid range provided.");
            return;
        }

        for (int i = start; i <= end; i++)
        {
            if (redGaugeImages[i] != null)
            {
                redGaugeImages[i].enabled = false;
            }
        }
    }
   

    public void RedCardIncreaseFillAmount()
    {
        Debug.Log("red");
        // FillAmountが1.0未満の場合のみFillAmountを増やす
        if (redCardImage.fillAmount < gameConstants.GaugeFillAmountThresholdFull)
        {
            Debug.Log("card");
            // FillAmountを増やす
            redCardImage.fillAmount += gameConstants.RedCardFillAmountIncrement;

            // FillAmountが1.0未満の場合、ボタンを無効化する
            if (redCardImage.fillAmount < gameConstants.GaugeFillAmountThresholdFull)
            {
                redCardButton.interactable = false;
            }
        }
        // FillAmountが1.0に達したらボタンを無効化する
        else
        {
            redCardButton.interactable = true;
        }
    }

    public void OnClickButton()
    {
        // ボタンが既にクリックされている場合は、DecreaseGauge()を呼び出さない
        if (!gameStateManager.IsButtonClicked && !gameStateManager.IsStudent)
        {
            Debug.Log("true");
            IncreaseGauge();
            gameStateManager.IsButtonClicked = true; // ボタンがクリックされたことを示すフラグを設定
        }
    }

    public void Clear_Score()
    {
        GameClearPanel.SetBool("isScore", true);

        if (gameConstants.TotalTime > gameConstants.BonusThresholdTime)
        {
            Bonus = gameConstants.BonusBaseScore;
        }

        score = Bonus + gameConstants.BaseScore + 
            GameManager.Instance.GetAnimationController().red_arrival * gameConstants.RedArrivalScoreMultiplier +
            GameManager.Instance.GetAnimationController().purple_arrival * gameConstants.PurpleArrivalScoreMultiplier + 
            GameManager.Instance.GetAnimationController().white_arrival * gameConstants.WhiteArrivalScoreMultiplier;

        Debug.Log("Score");
        Invoke("UpdateCounterText", 1.5f);
    } 

    void UpdateCounterText()
    {
        if (counterText != null)
        {
            counterText.text = "スコア : " + Score.ToString("N0");
        }
        else
        {
            Debug.LogError("counterText is not set!");
        }
    }

    // ボタンが押されなくなったときの処理
    public void OnButtonReleased()
    {
        gameStateManager.IsButtonClicked = false; // ボタンがクリックされたフラグをリセットしてDecreaseGauge()が再び呼び出されるようにする
        Debug.Log("false");
    }

    // ボタン1がクリックされた時の処理
    public void OnRedButton1Clicked()
    {
        if (redGaugeImages[3].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            IncreaseGauge_W();
            redGaugeImages[1].enabled = false;
        }
        else if (!gameStateManager.IsButton2Enabled) // ボタン2が押されていない場合のみボタン1を処理する
        {
            Debug.Log("1");
            RedCardIncreaseFillAmount();
            IncreaseGauge_W();
            gameStateManager.IsButton1Enabled = false;
            gameStateManager.IsButton2Enabled = true;
            //increaseButton1.gameObject.SetActive(false);
            redGaugeImages[1].enabled = false;
            redGaugeImages[0].enabled = true;
            //increaseButton2.gameObject.SetActive(true);
        }
    }

    // ボタン2がクリックされた時の処理
    public void OnRedButton2Clicked()
    {
        if (redGaugeImages[3].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            IncreaseGauge_W();
            redGaugeImages[0].enabled = false;
        }
        else if (!gameStateManager.IsButtonClicked) // ボタン1が押されていない場合のみボタン2を処理する
        {
            Debug.Log("2");
            RedCardIncreaseFillAmount();
            IncreaseGauge_W();
            gameStateManager.IsButton1Enabled = true;
            gameStateManager.IsButton2Enabled = false;
            //increaseButton1.gameObject.SetActive(true);
            redGaugeImages[0].enabled = false;
            redGaugeImages[1].enabled = true;
            //increaseButton2.gameObject.SetActive(false);
        }
    }

    // ゲージを増加させる関数
    void IncreaseGauge()
    {
        // ゲージの値を増加させる
        gaugeImage.fillAmount += gameConstants.IncreaseAmount;
        if (gaugeImage.fillAmount >= gameConstants.GaugeFillAmountThresholdFull && !gameStateManager.IsClear)
        {
            gameStateManager.IsClear = true;
            GameClearPanel.SetBool("isClear", true);
            Invoke("Clear_Score", 1.5f);
        }
        // ゲージの値を0から1の範囲にクランプする
        gaugeImage.fillAmount = Mathf.Clamp01(gaugeImage.fillAmount);
    }

    void IncreaseGauge_W()
    {
        if (redGaugeImages[3].fillAmount < 1.0f)
        {
            // ゲージの値を増加させる
            redGaugeImages[3].fillAmount += gameConstants.RedIncreaseAmount;
        }
        else if(redGaugeImages[3].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            gameStateManager.IsStudent = false;
            OnGaugeFull_red();
        }
        // ゲージの値を0から1の範囲にクランプする
        redGaugeImages[3].fillAmount = Mathf.Clamp01(redGaugeImages[3].fillAmount);
    }

    // ゲージを減少させる関数
    void DecreaseGauge()
    {
        // ゲージの値を毎秒の減少率に応じて減少させる
        //gaugeImage.fillAmount -= currentDecreaseRate * Time.deltaTime;
        gaugeImage.fillAmount -= gameConstants.DecreaseRateDecreaseAmount * Time.deltaTime;

        // ゲージの値を0から1の範囲にクランプする
        gaugeImage.fillAmount = Mathf.Clamp01(gaugeImage.fillAmount);
    }

    public void RedCard()
    {
        redCardImage.fillAmount = 0.0f;
        gameStateManager.IsStudent = false;
        SeitoRed.SetBool("isRedCard", true);
        SeitoPurple.SetBool("isRedCard", true);
        SeitoWhite.SetBool("isRedCard", true);
        // redの画像非表示
        redButtonLeft.gameObject.SetActive(false);
        redButtonRight.gameObject.SetActive(false);
        for (int i = 0; i < redGaugeImages.Length; i++)
        {
            redGaugeImages[i].gameObject.SetActive(false);
        }
        // purpleの画像非表示
        for (int i = 4; i < 11; i++)
        {
            GameManager.Instance.GetAnimationController().GaugeImages[i].gameObject.SetActive(false);
        }
        // grayの画像非表示
        //longPressButton.gray_Button.gameObject.SetActive(false);

        GameManager.Instance.HideAllImages();
        Invoke("ResetAnimation", 7.0f);
    }

    public void ResetAnimation()
    {
        gameStateManager.IsStudent = true;
        SeitoRed.SetBool("isRedCard", false);
        SeitoPurple.SetBool("isRedCard", false);
        SeitoWhite.SetBool("isRedCard", false);
        // redの画像表示
        redButtonLeft.gameObject.SetActive(true);
        redButtonRight.gameObject.SetActive(true);
        gameStateManager.IsButton1Enabled = false;
        gameStateManager.IsButton2Enabled = false;
        for (int i = 1; i < redGaugeImages.Length; i++)
        {
            redGaugeImages[i].gameObject.SetActive(true);
        }
        // purpleの画像表示
        //stickController.gaugeImage.gameObject.SetActive(true);
        for (int i = 1; i < 11; i++)
        {
            GameManager.Instance.GetAnimationController().GaugeImages[i].gameObject.SetActive(true);
        }
        // grayの画像表示
        //longPressButton.gray_Button.gameObject.SetActive(true);
        //WhiteButton.gameObject.SetActive(true);
    }

    public void red_Call()
    {
        AnimationController ainconInstance = FindObjectOfType<AnimationController>();
        if(ainconInstance != null)
        {
            GameManager.Instance.GetAnimationController().Restart_Red();
            gameStateManager.IsStudents[0] = false;
        }
    }

    public void OnGaugeFull_red()
    {
        SeitoRed.SetBool("isRed", false);
        Debug.Log("isRed");
        gameStateManager.IsButton1Enabled = false;
        gameStateManager.IsButton2Enabled = false;

        GameManager.Instance.GetGaugeController().HideImages();

        redGaugeImages[3].fillAmount = 0.0f;

        Invoke("red_Call", 5.0f);
    }

    

    public void OnGaugeFull_purple()
    {
        Debug.Log("isPurple");
        SeitoPurple.SetBool("isPurple", false);
        //stickController.gaugeImage.gameObject.SetActive(false);
        GameManager.Instance.GetStickController().HideImages();
       
        // 以前のコルーチンが実行中であればキャンセルする
        if (purpleCoroutine != null)
        {
            StopCoroutine(purpleCoroutine);
        }
        // 新しいコルーチンを開始し、参照を保持する
        purpleCoroutine = StartCoroutine(PurpleCoroutine());
    }

    private IEnumerator PurpleCoroutine()
    {
        Debug.Log("restart_purple");
        yield return new WaitForSeconds(5.0f);
        if (GameManager.Instance.GetAnimationController() != null)
        {
            GameManager.Instance.GetAnimationController().Restart_Purple();
            gameStateManager.IsStudents[1] = false;
            StopCoroutine(PurpleCoroutine());
        }
    }

    public void OnGaugeFull_gray()
    {
        Debug.Log("isWhite");
        SeitoWhite.SetBool("isWhite", false);
        Phone.SetBool("isCall", false);
        //longPressButton.gray_Button.gameObject.SetActive(false);

        GameManager.Instance.GetLongPressButton().HideImages();

        //longPressButton.gaugeImage.fillAmount = 0.0f;
        whiteGaugeImage.fillAmount = 0.0f;

        StartCoroutine(WhiteCoroutine());
    }

    private IEnumerator WhiteCoroutine()
    {
        Debug.Log("restart_White");
        yield return new WaitForSeconds(5.0f);
        if (GameManager.Instance.GetAnimationController() != null)
        {
            GameManager.Instance.GetAnimationController().Restart_White();
            gameStateManager.IsStudents[2] = false;
            StopCoroutine(WhiteCoroutine());
        }
    }
}
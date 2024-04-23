using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class GaugeController : MonoBehaviour
{
    public AudioSource audioSource;

    public AnimationController Ani_con;

    Animator red_animator;
    public GameObject redObj;
    Animator purple_animator;
    public GameObject purpleObj;
    Animator gray_animator;
    public GameObject grayObj;
    Animator Score_animator;
    public GameObject Score_obj;

    public Image gaugeImage; // ゲージのイメージ
    public Image w_Image; // ゲージのイメージ
    public Button increaseButton1; // ゲージを増加させるボタン1
    public Button increaseButton2; // ゲージを増加させるボタン2
    public Image[] images;
    
    public float increaseAmount = 0.0001f; // ゲージの増加量
    public float increaseAmount_w = 0.05f; // ゲージの増加量
    public float initialDecreaseRate = 0.001f; // 初期のゲージの毎秒の減少率
    public float decreaseRateDecreaseAmount = 0.001f; // 毎秒の減少率を減らす量

    private bool button1Enabled = true; // ボタン1が有効かどうか
    private bool button2Enabled = false; // ボタン2が有効かどうか

    private bool buttonClicked = false; // ボタンがクリックされたかどうかのフラグ

    // ボタンを長押しする時間の閾値
    public float longPressThreshold = 1.0f;

    public Button targetButton; // FillAmountを増やす対象のボタン
    public float fillAmountIncrement = 0.1f; // FillAmountを増やす量

    private Image targetImage; // FillAmountを調整するためのイメージ
    public Text counterText;

    public StickController stickController;
    public LongPressButton longPressButton;
    public TimerCountDown timerCountDown;

    private Coroutine purpleCoroutine; // purpleCoroutineの参照を保持する

    public bool isStudent = false;
    public bool isClear = false;
    //private bool isCounting = false;

    public int Score = 0; 
    private int Bonus = 0;

    public Button button;

    private void Awake()
    {
        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        audioSource.loop = true;
        // オーディオを再生する
        audioSource.Play();

        red_animator = redObj.GetComponent<Animator>();
        purple_animator = purpleObj.GetComponent<Animator>();
        gray_animator = grayObj.GetComponent<Animator>();
        Score_animator= Score_obj.GetComponent<Animator>();
        
        // ターゲットボタンのImageコンポーネントを取得
        targetImage = targetButton.GetComponent<Image>();
        targetButton.interactable = false;
        increaseButton2.gameObject.SetActive(false); 
    }

    void Update()
    {
        // ボタンがクリックされていないときのみ減少させる
        if (!buttonClicked && isStudent && !isClear)
        {
            DecreaseGauge();
        }
        else if (gaugeImage.fillAmount > 0.0f && !isClear)
        {
            DecreaseGauge();
        }
    }

    public void IncreaseFillAmount()
    {
        Debug.Log("red");
        // FillAmountが1.0未満の場合のみFillAmountを増やす
        if (targetImage.fillAmount < 1.0f)
        {
            Debug.Log("card");
            // FillAmountを増やす
            targetImage.fillAmount += fillAmountIncrement;

            // FillAmountが1.0未満の場合、ボタンを無効化する
            if (targetImage.fillAmount < 1.0f)
            {
                targetButton.interactable = false;
            }
        }
        // FillAmountが1.0に達したらボタンを無効化する
        else
        {
            targetButton.interactable = true;
        }
    }

    public void OnClickButton()
    {
        // ボタンが既にクリックされている場合は、DecreaseGauge()を呼び出さない
        if (!buttonClicked && !isStudent)
        {
            Debug.Log("true");
            IncreaseGauge();
            buttonClicked = true; // ボタンがクリックされたことを示すフラグを設定
        }
    }

    public void Clear_Score()
    {
        Score_animator.SetBool("isScore", true);

        if (timerCountDown.totalTime > 120)
        {
            Bonus = 30000;
        }

        Score = Bonus + 18000 + 
            Ani_con.red_arrival * 700 + 
            Ani_con.purple_arrival * 600 + 
            Ani_con.gray_arrival * 500;

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
        buttonClicked = false; // ボタンがクリックされたフラグをリセットしてDecreaseGauge()が再び呼び出されるようにする
        Debug.Log("false");
    }

    // ボタン1がクリックされた時の処理
    public void OnIncreaseButton1Clicked()
    {
        if (w_Image.fillAmount >= 0.96f)
        {
            IncreaseGauge_W();
            images[1].gameObject.SetActive(false);
        }
        else if (!button2Enabled) // ボタン2が押されていない場合のみボタン1を処理する
        {
            Debug.Log("1");
            IncreaseFillAmount();
            IncreaseGauge_W();
            button1Enabled = false;
            button2Enabled = true;
            //increaseButton1.gameObject.SetActive(false);
            images[1].gameObject.SetActive(false);
            images[0].gameObject.SetActive(true);
            //increaseButton2.gameObject.SetActive(true);
        }
    }

    // ボタン2がクリックされた時の処理
    public void OnIncreaseButton2Clicked()
    {
        if(w_Image.fillAmount >= 0.96f)
        {
            IncreaseGauge_W();
            images[0].gameObject.SetActive(false);
        }
        else if (!button1Enabled) // ボタン1が押されていない場合のみボタン2を処理する
        {
            Debug.Log("2");
            IncreaseFillAmount();
            IncreaseGauge_W();
            button1Enabled = true;
            button2Enabled = false;
            //increaseButton1.gameObject.SetActive(true);
            images[0].gameObject.SetActive(false);
            images[1].gameObject.SetActive(true);
            //increaseButton2.gameObject.SetActive(false);
        }
    }

    // ゲージを増加させる関数
    void IncreaseGauge()
    {
        // ゲージの値を増加させる
        gaugeImage.fillAmount += increaseAmount;
        if (gaugeImage.fillAmount >= 0.99 && !isClear)
        {
            isClear = true;
            Score_animator.SetBool("isClear", true);
            Invoke("Clear_Score", 1.5f);
        }
        // ゲージの値を0から1の範囲にクランプする
        gaugeImage.fillAmount = Mathf.Clamp01(gaugeImage.fillAmount);
    }

    void IncreaseGauge_W()
    {
        if (w_Image.fillAmount < 1.0f)
        {

            // ゲージの値を増加させる
            w_Image.fillAmount += increaseAmount_w;
        }
        else if(w_Image.fillAmount >= 0.96f)
        {
            isStudent = false;
            OnGaugeFull_red();
        }
        // ゲージの値を0から1の範囲にクランプする
        w_Image.fillAmount = Mathf.Clamp01(w_Image.fillAmount);
    }

    // ゲージを減少させる関数
    void DecreaseGauge()
    {
        // ゲージの値を毎秒の減少率に応じて減少させる
        //gaugeImage.fillAmount -= currentDecreaseRate * Time.deltaTime;
        gaugeImage.fillAmount -= decreaseRateDecreaseAmount * Time.deltaTime;

        // ゲージの値を0から1の範囲にクランプする
        gaugeImage.fillAmount = Mathf.Clamp01(gaugeImage.fillAmount);
    }

    public void RedCard()
    {
        targetImage.fillAmount = 0.0f;
        isStudent = false;
        red_animator.SetBool("isRedCard", true);
        purple_animator.SetBool("isRedCard", true);
        gray_animator.SetBool("isRedCard", true);
        // redの画像非表示
        increaseButton1.gameObject.SetActive(false);
        increaseButton2.gameObject.SetActive(false);
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(false);
        }
        // purpleの画像非表示
        stickController.gaugeImage.gameObject.SetActive(false);
        for (int i = 4; i < 11; i++)
        {
            Ani_con.gauge_Images[i].gameObject.SetActive(false);
        }
        // grayの画像非表示
        longPressButton.gray_Button.gameObject.SetActive(false);

        Invoke("ResetAnimatoin", 7.0f);
    }

    public void ResetAnimatoin()
    {
        isStudent = true;
        red_animator.SetBool("isRedCard", false);
        purple_animator.SetBool("isRedCard", false);
        gray_animator.SetBool("isRedCard", false);
        // redの画像表示
        increaseButton1.gameObject.SetActive(true);
        increaseButton2.gameObject.SetActive(true);
        button1Enabled = false;
        button2Enabled = false;
        for (int i = 1; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(true);
        }
        // purpleの画像表示
        stickController.gaugeImage.gameObject.SetActive(true);
        for (int i = 1; i < 11; i++)
        {
            Ani_con.gauge_Images[i].gameObject.SetActive(true);
        }
        // grayの画像表示
        longPressButton.gray_Button.gameObject.SetActive(true);
    }

    public void red_Call()
    {
        AnimationController ainconInstance = FindObjectOfType<AnimationController>();
        if(ainconInstance != null)
        {
            Ani_con.Restart_Red();
            Ani_con.isRed = false;
        }
    }

    public void OnGaugeFull_red()
    {
        red_animator.SetBool("isRed", false);
        Debug.Log("isRed");
        increaseButton1.gameObject.SetActive(false);
        increaseButton2.gameObject.SetActive(false);
        button1Enabled = false;
        button2Enabled = false;

        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(false);
        }
        w_Image.fillAmount = 0.0f;

        Invoke("red_Call", 5.0f);
    }

    

    public void OnGaugeFull_purple()
    {
        Debug.Log("isPurple");
        purple_animator.SetBool("isPurple", false);
        stickController.gaugeImage.gameObject.SetActive(false);

        for (int i = 4; i < 9; i++)
        {
            Ani_con.gauge_Images[i].gameObject.SetActive(false);
        }
        stickController.gaugeImage.fillAmount = 0.0f;

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
        if (Ani_con != null)
        {
            Ani_con.Restart_Purple();
            Ani_con.isPurple = false;
            StopCoroutine(PurpleCoroutine());
        }
    }

    public void OnGaugeFull_gray()
    {
        Debug.Log("isGray");
        gray_animator.SetBool("isGray", false);
        longPressButton.phone_animator.SetBool("isCall", false);
        longPressButton.gray_Button.gameObject.SetActive(false);

        Ani_con.gauge_Images[9].gameObject.SetActive(false);
        Ani_con.gauge_Images[10].gameObject.SetActive(false);

        longPressButton.gaugeImage.fillAmount = 0.0f;

        StartCoroutine(GrayCoroutine());
    }

    private IEnumerator GrayCoroutine()
    {
        Debug.Log("restart_gray");
        yield return new WaitForSeconds(5.0f);
        if (Ani_con != null)
        {
            Ani_con.Restart_Gray();
            Ani_con.isGray = false;
            StopCoroutine(GrayCoroutine());
        }
    }
}
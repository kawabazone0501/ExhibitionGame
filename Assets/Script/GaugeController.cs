using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class GaugeController : MonoBehaviour
{
    private Animator SeitoRed;
    private Animator SeitoPurple;
    private Animator SeitoWhite;
    private Animator Phone;
    private Animator GameClearPanel;
    private Animator RedGuide;
    private Animator TeacherGuide;
    private Animator Teacher;


    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private UIManager uiManager;

    private int Bonus = 0;
    private int score = 0;
    public int Score => score;

    private bool RedGuidePlayed = false;
    private bool TeacherGuidePlayed = false;

    private bool isTeacherWork = false;
    private float DelayTime = 3.0f;

    private void Awake()
    {
        
 
        AnimatorController animatorController = FindAnyObjectByType<AnimatorController>();
        if (animatorController != null)
        {
            SeitoRed = animatorController.SeitoRed;
            SeitoPurple = animatorController.SeitoPurple;
            SeitoWhite = animatorController.SeitoWhite;
            Phone = animatorController.Phone;
            GameClearPanel = animatorController.GameClearPanel;
            RedGuide = animatorController.RedGuide;
            TeacherGuide = animatorController.TeacherGuide;
            Teacher = animatorController.Teacher;
        }        
    }
    
    void Update()
    {
        // ボタンがクリックされていないときのみ減少させる
        if (!gameStateManager.IsButtonClicked  && !gameStateManager.IsClear)
        {
            DecreaseGauge();
        }
        else if (uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount > gameConstants.GaugeFillAmountThresholdReset && !gameStateManager.IsClear)
        {
            DecreaseGauge();
        }
        if(!gameStateManager.TeacherGuidePlayed)
        {
            gameStateManager.TeacherGuidePlayed = true;
            TeacherGuide.SetBool("isTeacherGuide", true);
        }
    }

    public void RedCardIncreaseFillAmount()
    {
        Debug.Log("red");
        // FillAmountが1.0未満の場合のみFillAmountを増やす
        if (uiManager.GaugeImages[gameConstants.RedCard].fillAmount < gameConstants.GaugeFillAmountThresholdFull)
        {
            Debug.Log("card");
            // FillAmountを増やす
            uiManager.GaugeImages[gameConstants.RedCard].fillAmount += gameConstants.RedCardFillAmountIncrement;

            // FillAmountが1.0未満の場合、ボタンを無効化する
            if (uiManager.GaugeImages[gameConstants.RedCard].fillAmount < gameConstants.GaugeFillAmountThresholdFull)
            {
                uiManager.Buttons[gameConstants.RedCardButton].interactable = false;
            }
        }
        // FillAmountが1.0に達したらボタンを無効化する
        else
        {
            uiManager.Buttons[gameConstants.RedCardButton].interactable = true;
        }
    }

    public void OnClickButton()
    {
        if(gameStateManager.TeacherGuidePlayed&&!TeacherGuidePlayed)
        {
            TeacherGuidePlayed = true;
            TeacherGuide.SetBool("isTeacherGuide", false);
        }
        if (!isTeacherWork)
        {
            Debug.Log("work");
            StartCoroutine(AnimateWithDelay());
        }
        // ボタンが既にクリックされている場合は、DecreaseGauge()を呼び出さない
        if (
            !gameStateManager.IsButtonClicked && 
            !gameStateManager.IsStudents[gameConstants.StudentRED]&&
            !gameStateManager.IsStudents[gameConstants.StudentPURPLE] &&
            !gameStateManager.IsStudents[gameConstants.StudentWHITE]
            )
        {
            Debug.Log("true");
            IncreaseGauge();
            gameStateManager.IsButtonClicked = true; // ボタンがクリックされたことを示すフラグを設定
        }
    }
    private IEnumerator AnimateWithDelay()
    {
        Debug.Log("入った");
        isTeacherWork = true;
        // 50%の確率でどちらかのアニメーショントリガーをセット
        if (Random.value < 0.5f)
        {
            Debug.Log("PC");
            Teacher.SetTrigger("PCWork");
        }
        else
        {
            Debug.Log("Document");
            Teacher.SetTrigger("DocumentWork");
        }
        // 変化したアニメーションが終了するまで待機
        yield return new WaitForSeconds(DelayTime);

        Teacher.SetTrigger("Wait");

        isTeacherWork = false;
    }   
    public void ClearScore()
    {
        GameClearPanel.SetBool("isScore", true);

        if (gameConstants.TotalTime > gameConstants.BonusThresholdTime)
        {
            Bonus = gameConstants.BonusBaseScore;
        }

        score = Bonus + gameConstants.BaseScore + 
            uiManager.GetAnimationController().red_arrival * gameConstants.RedArrivalScoreMultiplier +
            uiManager.GetAnimationController().purple_arrival * gameConstants.PurpleArrivalScoreMultiplier + 
            uiManager.GetAnimationController().white_arrival * gameConstants.WhiteArrivalScoreMultiplier;

        Debug.Log("Score");
        Invoke("UpdateCounterText", 1.5f);
    } 

    void UpdateCounterText()
    {
        if (uiManager.CounterText != null)
        {
            uiManager.CounterText.text = "スコア : " + Score.ToString("N0");
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
        Teacher.SetBool("vsRed", true);
        if (gameStateManager.RedGuidePlayed && !RedGuidePlayed)
        {
            RedGuidePlayed = true;
            RedGuide.SetBool("isRedGuide", false);
        }
        if (uiManager.GaugeImages[gameConstants.RedGauge].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            IncreaseGauge_W();
            uiManager.Buttons[gameConstants.RedButtonLeft].enabled = false;
        }
        else if (!gameStateManager.IsButton2Enabled) // ボタン2が押されていない場合のみボタン1を処理する
        {
            Debug.Log("1");
            RedCardIncreaseFillAmount();
            IncreaseGauge_W();
            gameStateManager.IsButton1Enabled = false;
            gameStateManager.IsButton2Enabled = true;
            uiManager.GaugeImages[gameConstants.RedButtonLeftArrow].enabled = true;
            uiManager.GaugeImages[gameConstants.RedButtonRightArrow].enabled = false;
            uiManager.Buttons[gameConstants.RedButtonLeft].enabled = false;
            uiManager.Buttons[gameConstants.RedButtonRight].enabled = true;
        }
    }

    // ボタン2がクリックされた時の処理
    public void OnRedButton2Clicked()
    {
        Teacher.SetBool("vsRed", true);
        if (gameStateManager.RedGuidePlayed && !RedGuidePlayed)
        {
            RedGuidePlayed = true;
            RedGuide.SetBool("isRedGuide", false);
        }
        Debug.Log("2");
        if (uiManager.GaugeImages[gameConstants.RedGauge].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            IncreaseGauge_W();
            uiManager.Buttons[gameConstants.RedButtonRight].enabled = false;
        }
        else if (!gameStateManager.IsButton1Enabled) // ボタン1が押されていない場合のみボタン2を処理する
        {
            Debug.Log("2");
            RedCardIncreaseFillAmount();
            IncreaseGauge_W();
            gameStateManager.IsButton1Enabled = true;
            gameStateManager.IsButton2Enabled = false;
            uiManager.GaugeImages[gameConstants.RedButtonLeftArrow].enabled = false;
            uiManager.GaugeImages[gameConstants.RedButtonRightArrow].enabled = true; ;
            uiManager.Buttons[gameConstants.RedButtonLeft].enabled = true;
            uiManager.Buttons[gameConstants.RedButtonRight].enabled = false;
        }
    }

    // ゲージを増加させる関数
    void IncreaseGauge()
    {
        // ゲージの値を増加させる
        uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount += gameConstants.IncreaseAmount;
        if (uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount >= gameConstants.GaugeFillAmountThresholdFull && !gameStateManager.IsClear)
        {
            gameStateManager.IsClear = true;
            GameClearPanel.SetBool("isClear", true);
            Invoke("ClearScore", 1.5f);
        }
        // ゲージの値を0から1の範囲にクランプする
        uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount = Mathf.Clamp01(uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount);
    }

    void IncreaseGauge_W()
    {
        if (uiManager.GaugeImages[gameConstants.RedGauge].fillAmount < gameConstants.GaugeFillAmountThresholdFull)
        {
            // ゲージの値を増加させる
            uiManager.GaugeImages[gameConstants.RedGauge].fillAmount += gameConstants.RedIncreaseAmount;
        }
        else if(uiManager.GaugeImages[gameConstants.RedGauge].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            gameStateManager.IsButton1Enabled = false;
            gameStateManager.IsButton2Enabled = false;
            gameStateManager.IsStudents[gameConstants.StudentRED] = false;
            OnGaugeFull_red();
        }
        // ゲージの値を0から1の範囲にクランプする
        uiManager.GaugeImages[gameConstants.RedGauge].fillAmount = Mathf.Clamp01(uiManager.GaugeImages[gameConstants.RedGauge].fillAmount);
    }

    // ゲージを減少させる関数
    void DecreaseGauge()
    {
        // ゲージの値を毎秒の減少率に応じて減少させる
        //gaugeImage.fillAmount -= currentDecreaseRate * Time.deltaTime;
        uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount -= gameConstants.DecreaseRateDecreaseAmount * Time.deltaTime;

        // ゲージの値を0から1の範囲にクランプする
        uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount = Mathf.Clamp01(uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount);
    }

    public void RedCard()
    {
        uiManager.GaugeImages[gameConstants.RedCard].fillAmount = gameConstants.GaugeFillAmountThresholdReset;
        uiManager.RedHide(gameConstants.StartDisplayImageRed, gameConstants.EndDisplayImageRed);
        uiManager.PurpleHide(gameConstants.StartDisplayImagePurple, gameConstants.EndDisplayImagePurple);
        uiManager.WhiteHide(gameConstants.StartDisplayImageWhite, gameConstants.EndDisplayImageWhite);
        gameStateManager.IsStudents[gameConstants.StudentRED] = false;
        gameStateManager.IsStudents[gameConstants.StudentPURPLE] = false;
        gameStateManager.IsStudents[gameConstants.StudentWHITE] = false;
        SeitoRed.SetBool("isRedCard", true);
        SeitoPurple.SetBool("isRedCard", true);
        SeitoWhite.SetBool("isRedCard", true);
        Invoke("ResetAnimation", 7.0f);
    }

    public void ResetAnimation()
    {
        gameStateManager.IsStudents[gameConstants.StudentRED] = true;
        gameStateManager.IsStudents[gameConstants.StudentPURPLE] = true;
        gameStateManager.IsStudents[gameConstants.StudentWHITE] = true;
        uiManager.RedShow(gameConstants.StartDisplayImageRed, gameConstants.EndDisplayImageRed);
        uiManager.PurpleShow(gameConstants.StartDisplayImagePurple, gameConstants.EndDisplayImagePurple);
        uiManager.WhiteShow(gameConstants.StartDisplayImageWhite, gameConstants.EndDisplayImageWhite);
        SeitoRed.SetBool("isRedCard", false);
        SeitoPurple.SetBool("isRedCard", false);
        SeitoWhite.SetBool("isRedCard", false);
    }



    public void OnGaugeFull_red()
    {
        SeitoRed.SetBool("isRed", false);
        Debug.Log("isRed");
        gameStateManager.IsButton1Enabled = false;
        gameStateManager.IsButton2Enabled = false;
        Teacher.SetBool("vsRed", false);
        uiManager.RedHide(gameConstants.StartDisplayImageRed, gameConstants.EndDisplayImageRed);


        StartCoroutine(RedCoroutine());
    }

    private IEnumerator RedCoroutine()
    {
        yield return new WaitForSeconds(gameConstants.ExitWaitingTime);
        if(uiManager.GetAnimationController() != null)
        {
            uiManager.GetAnimationController().Restart_Purple();
            
            StopCoroutine(RedCoroutine());
        }
    }
    

    public void OnGaugeFull_purple()
    {
        Debug.Log("isPurple");
        SeitoPurple.SetBool("isPurple", false);
        uiManager.PurpleHide(gameConstants.StartDisplayImagePurple, gameConstants.EndDisplayImagePurple);

         StartCoroutine(PurpleCoroutine());
    }

    private IEnumerator PurpleCoroutine()
    {
        Debug.Log("restart_purple");
        yield return new WaitForSeconds(gameConstants.ExitWaitingTime);
        if (uiManager.GetAnimationController() != null)
        {
            uiManager.GetAnimationController().Restart_Purple();
            
            StopCoroutine(PurpleCoroutine());
        }
    }

    public void OnGaugeFullWhite()
    {
        Debug.Log("isWhite");
        SeitoWhite.SetBool("isWhite", false);
        Phone.SetBool("isCall", false);
         
        uiManager.WhiteHide(gameConstants.StartDisplayImageWhite, gameConstants.EndDisplayImageWhite);
 
        StartCoroutine(WhiteCoroutine());
    }

    private IEnumerator WhiteCoroutine()
    {
        Debug.Log("restart_White");
        yield return new WaitForSeconds(gameConstants.ExitWaitingTime);
        if (uiManager.GetAnimationController() != null)
        {
            Debug.Log("stop_White");
            uiManager.GetAnimationController().Restart_White();
            
            StopCoroutine(WhiteCoroutine());
        }
    }
}

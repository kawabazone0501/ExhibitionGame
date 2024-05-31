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

    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private GameManager gameManager;

    private int Bonus = 0;
    private int score = 0;
    public int Score => score;
    
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
        }        
    }
    
    void Update()
    {
        // �{�^�����N���b�N����Ă��Ȃ��Ƃ��̂݌���������
        if (!gameStateManager.IsButtonClicked  && !gameStateManager.IsClear)
        {
            DecreaseGauge();
        }
        else if (gameManager.GaugeImages[gameConstants.PlayerGauge].fillAmount > gameConstants.GaugeFillAmountThresholdReset && !gameStateManager.IsClear)
        {
            DecreaseGauge();
        }
    }

    public void RedCardIncreaseFillAmount()
    {
        Debug.Log("red");
        // FillAmount��1.0�����̏ꍇ�̂�FillAmount�𑝂₷
        if (gameManager.GaugeImages[gameConstants.RedCard].fillAmount < gameConstants.GaugeFillAmountThresholdFull)
        {
            Debug.Log("card");
            // FillAmount�𑝂₷
            gameManager.GaugeImages[gameConstants.RedCard].fillAmount += gameConstants.RedCardFillAmountIncrement;

            // FillAmount��1.0�����̏ꍇ�A�{�^���𖳌�������
            if (gameManager.GaugeImages[gameConstants.RedCard].fillAmount < gameConstants.GaugeFillAmountThresholdFull)
            {
                gameManager.Buttons[gameConstants.RedCardButton].interactable = false;
            }
        }
        // FillAmount��1.0�ɒB������{�^���𖳌�������
        else
        {
            gameManager.Buttons[gameConstants.RedCardButton].interactable = true;
        }
    }

    public void OnClickButton()
    {
        // �{�^�������ɃN���b�N����Ă���ꍇ�́ADecreaseGauge()���Ăяo���Ȃ�
        if (
            !gameStateManager.IsButtonClicked && 
            !gameStateManager.IsStudents[gameConstants.StudentRED]&&
            !gameStateManager.IsStudents[gameConstants.StudentPURPLE] &&
            !gameStateManager.IsStudents[gameConstants.StudentWHITE]
            )
        {
            Debug.Log("true");
            IncreaseGauge();
            gameStateManager.IsButtonClicked = true; // �{�^�����N���b�N���ꂽ���Ƃ������t���O��ݒ�
        }
    }

    public void ClearScore()
    {
        GameClearPanel.SetBool("isScore", true);

        if (gameConstants.TotalTime > gameConstants.BonusThresholdTime)
        {
            Bonus = gameConstants.BonusBaseScore;
        }

        score = Bonus + gameConstants.BaseScore + 
            gameManager.GetAnimationController().red_arrival * gameConstants.RedArrivalScoreMultiplier +
            gameManager.GetAnimationController().purple_arrival * gameConstants.PurpleArrivalScoreMultiplier + 
            gameManager.GetAnimationController().white_arrival * gameConstants.WhiteArrivalScoreMultiplier;

        Debug.Log("Score");
        Invoke("UpdateCounterText", 1.5f);
    } 

    void UpdateCounterText()
    {
        if (gameManager.CounterText != null)
        {
            gameManager.CounterText.text = "�X�R�A : " + Score.ToString("N0");
        }
        else
        {
            Debug.LogError("counterText is not set!");
        }
    }

    // �{�^����������Ȃ��Ȃ����Ƃ��̏���
    public void OnButtonReleased()
    {
        gameStateManager.IsButtonClicked = false; // �{�^�����N���b�N���ꂽ�t���O�����Z�b�g����DecreaseGauge()���ĂьĂяo�����悤�ɂ���
        Debug.Log("false");
    }

    // �{�^��1���N���b�N���ꂽ���̏���
    public void OnRedButton1Clicked()
    {
        if (gameManager.GaugeImages[gameConstants.RedGauge].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            IncreaseGauge_W();
            gameManager.Buttons[gameConstants.RedButtonLeft].enabled = false;
        }
        else if (!gameStateManager.IsButton2Enabled) // �{�^��2��������Ă��Ȃ��ꍇ�̂݃{�^��1����������
        {
            Debug.Log("1");
            RedCardIncreaseFillAmount();
            IncreaseGauge_W();
            gameStateManager.IsButton1Enabled = false;
            gameStateManager.IsButton2Enabled = true;
            gameManager.GaugeImages[gameConstants.RedButtonLeftArrow].enabled = true;
            gameManager.GaugeImages[gameConstants.RedButtonRightArrow].enabled = false;
            gameManager.Buttons[gameConstants.RedButtonLeft].enabled = false;
            gameManager.Buttons[gameConstants.RedButtonRight].enabled = true;
        }
    }

    // �{�^��2���N���b�N���ꂽ���̏���
    public void OnRedButton2Clicked()
    {
        Debug.Log("2");
        if (gameManager.GaugeImages[gameConstants.RedGauge].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            IncreaseGauge_W();
            gameManager.Buttons[gameConstants.RedButtonRight].enabled = false;
        }
        else if (!gameStateManager.IsButton1Enabled) // �{�^��1��������Ă��Ȃ��ꍇ�̂݃{�^��2����������
        {
            Debug.Log("2");
            RedCardIncreaseFillAmount();
            IncreaseGauge_W();
            gameStateManager.IsButton1Enabled = true;
            gameStateManager.IsButton2Enabled = false;
            gameManager.GaugeImages[gameConstants.RedButtonLeftArrow].enabled = false;
            gameManager.GaugeImages[gameConstants.RedButtonRightArrow].enabled = true; ;
            gameManager.Buttons[gameConstants.RedButtonLeft].enabled = true;
            gameManager.Buttons[gameConstants.RedButtonRight].enabled = false;
        }
    }

    // �Q�[�W�𑝉�������֐�
    void IncreaseGauge()
    {
        // �Q�[�W�̒l�𑝉�������
        gameManager.GaugeImages[gameConstants.PlayerGauge].fillAmount += gameConstants.IncreaseAmount;
        if (gameManager.GaugeImages[gameConstants.PlayerGauge].fillAmount >= gameConstants.GaugeFillAmountThresholdFull && !gameStateManager.IsClear)
        {
            gameStateManager.IsClear = true;
            GameClearPanel.SetBool("isClear", true);
            Invoke("ClearScore", 1.5f);
        }
        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        gameManager.GaugeImages[gameConstants.PlayerGauge].fillAmount = Mathf.Clamp01(gameManager.GaugeImages[gameConstants.PlayerGauge].fillAmount);
    }

    void IncreaseGauge_W()
    {
        if (gameManager.GaugeImages[gameConstants.RedGauge].fillAmount < gameConstants.GaugeFillAmountThresholdFull)
        {
            // �Q�[�W�̒l�𑝉�������
            gameManager.GaugeImages[gameConstants.RedGauge].fillAmount += gameConstants.RedIncreaseAmount;
        }
        else if(gameManager.GaugeImages[gameConstants.RedGauge].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            gameStateManager.IsButton1Enabled = false;
            gameStateManager.IsButton2Enabled = false;
            gameStateManager.IsStudents[gameConstants.StudentRED] = false;
            OnGaugeFull_red();
        }
        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        gameManager.GaugeImages[gameConstants.RedGauge].fillAmount = Mathf.Clamp01(gameManager.GaugeImages[gameConstants.RedGauge].fillAmount);
    }

    // �Q�[�W������������֐�
    void DecreaseGauge()
    {
        // �Q�[�W�̒l�𖈕b�̌������ɉ����Č���������
        //gaugeImage.fillAmount -= currentDecreaseRate * Time.deltaTime;
        gameManager.GaugeImages[gameConstants.PlayerGauge].fillAmount -= gameConstants.DecreaseRateDecreaseAmount * Time.deltaTime;

        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        gameManager.GaugeImages[gameConstants.PlayerGauge].fillAmount = Mathf.Clamp01(gameManager.GaugeImages[gameConstants.PlayerGauge].fillAmount);
    }

    public void RedCard()
    {
        gameManager.GaugeImages[gameConstants.RedCard].fillAmount = gameConstants.GaugeFillAmountThresholdReset;
        SeitoRed.SetBool("isRedCard", true);
        SeitoPurple.SetBool("isRedCard", true);
        SeitoWhite.SetBool("isRedCard", true);
        Invoke("ResetAnimation", 7.0f);
    }

    public void ResetAnimation()
    {
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

        gameManager.RedHide(gameConstants.StartDisplayImageRed, gameConstants.EndDisplayImageRed);


        StartCoroutine(RedCoroutine());
    }

    private IEnumerator RedCoroutine()
    {
        yield return new WaitForSeconds(gameConstants.ExitWaitingTime);
        if(gameManager.GetAnimationController() != null)
        {
            gameManager.GetAnimationController().Restart_Purple();
            
            StopCoroutine(RedCoroutine());
        }
    }
    

    public void OnGaugeFull_purple()
    {
        Debug.Log("isPurple");
        SeitoPurple.SetBool("isPurple", false);
        gameManager.PurpleHide(gameConstants.StartDisplayImagePurple, gameConstants.EndDisplayImagePurple);

         StartCoroutine(PurpleCoroutine());
    }

    private IEnumerator PurpleCoroutine()
    {
        Debug.Log("restart_purple");
        yield return new WaitForSeconds(gameConstants.ExitWaitingTime);
        if (gameManager.GetAnimationController() != null)
        {
            gameManager.GetAnimationController().Restart_Purple();
            
            StopCoroutine(PurpleCoroutine());
        }
    }

    public void OnGaugeFullWhite()
    {
        Debug.Log("isWhite");
        SeitoWhite.SetBool("isWhite", false);
        Phone.SetBool("isCall", false);
         
        gameManager.WhiteHide(gameConstants.StartDisplayImageWhite, gameConstants.EndDisplayImageWhite);
 
        StartCoroutine(WhiteCoroutine());
    }

    private IEnumerator WhiteCoroutine()
    {
        Debug.Log("restart_White");
        yield return new WaitForSeconds(gameConstants.ExitWaitingTime);
        if (gameManager.GetAnimationController() != null)
        {
            gameManager.GetAnimationController().Restart_White();
            
            StopCoroutine(WhiteCoroutine());
        }
    }
}

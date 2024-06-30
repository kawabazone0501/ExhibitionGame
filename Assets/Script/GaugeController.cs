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
        // �{�^�����N���b�N����Ă��Ȃ��Ƃ��̂݌���������
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
        // FillAmount��1.0�����̏ꍇ�̂�FillAmount�𑝂₷
        if (uiManager.GaugeImages[gameConstants.RedCard].fillAmount < gameConstants.GaugeFillAmountThresholdFull)
        {
            Debug.Log("card");
            // FillAmount�𑝂₷
            uiManager.GaugeImages[gameConstants.RedCard].fillAmount += gameConstants.RedCardFillAmountIncrement;

            // FillAmount��1.0�����̏ꍇ�A�{�^���𖳌�������
            if (uiManager.GaugeImages[gameConstants.RedCard].fillAmount < gameConstants.GaugeFillAmountThresholdFull)
            {
                uiManager.Buttons[gameConstants.RedCardButton].interactable = false;
            }
        }
        // FillAmount��1.0�ɒB������{�^���𖳌�������
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
    private IEnumerator AnimateWithDelay()
    {
        Debug.Log("������");
        isTeacherWork = true;
        // 50%�̊m���łǂ��炩�̃A�j���[�V�����g���K�[���Z�b�g
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
        // �ω������A�j���[�V�������I������܂őҋ@
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
            uiManager.CounterText.text = "�X�R�A : " + Score.ToString("N0");
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
        else if (!gameStateManager.IsButton2Enabled) // �{�^��2��������Ă��Ȃ��ꍇ�̂݃{�^��1����������
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

    // �{�^��2���N���b�N���ꂽ���̏���
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
        else if (!gameStateManager.IsButton1Enabled) // �{�^��1��������Ă��Ȃ��ꍇ�̂݃{�^��2����������
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

    // �Q�[�W�𑝉�������֐�
    void IncreaseGauge()
    {
        // �Q�[�W�̒l�𑝉�������
        uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount += gameConstants.IncreaseAmount;
        if (uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount >= gameConstants.GaugeFillAmountThresholdFull && !gameStateManager.IsClear)
        {
            gameStateManager.IsClear = true;
            GameClearPanel.SetBool("isClear", true);
            Invoke("ClearScore", 1.5f);
        }
        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount = Mathf.Clamp01(uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount);
    }

    void IncreaseGauge_W()
    {
        if (uiManager.GaugeImages[gameConstants.RedGauge].fillAmount < gameConstants.GaugeFillAmountThresholdFull)
        {
            // �Q�[�W�̒l�𑝉�������
            uiManager.GaugeImages[gameConstants.RedGauge].fillAmount += gameConstants.RedIncreaseAmount;
        }
        else if(uiManager.GaugeImages[gameConstants.RedGauge].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            gameStateManager.IsButton1Enabled = false;
            gameStateManager.IsButton2Enabled = false;
            gameStateManager.IsStudents[gameConstants.StudentRED] = false;
            OnGaugeFull_red();
        }
        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        uiManager.GaugeImages[gameConstants.RedGauge].fillAmount = Mathf.Clamp01(uiManager.GaugeImages[gameConstants.RedGauge].fillAmount);
    }

    // �Q�[�W������������֐�
    void DecreaseGauge()
    {
        // �Q�[�W�̒l�𖈕b�̌������ɉ����Č���������
        //gaugeImage.fillAmount -= currentDecreaseRate * Time.deltaTime;
        uiManager.GaugeImages[gameConstants.PlayerGauge].fillAmount -= gameConstants.DecreaseRateDecreaseAmount * Time.deltaTime;

        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
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

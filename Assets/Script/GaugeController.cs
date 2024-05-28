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
  
    public Button redCardButton; // FillAmount�𑝂₷�Ώۂ̃{�^��

    [SerializeField] private Image redCardImage; // FillAmount�𒲐����邽�߂̃C���[�W
    public Image RedCardImage=> redCardImage;

    [SerializeField]
    private Text counterText;

    private Coroutine purpleCoroutine; // purpleCoroutine�̎Q�Ƃ�ێ�����

    [SerializeField]
    private GameConstants gameConstants;
    [SerializeField]
    private GameStateManager gameStateManager;


    

    private int Bonus = 0;

    private int score = 0;
    public int Score => score;
    
    //�v���C���[�̃Q�[�W�pImage 
    [SerializeField] private Image gaugeImage;
    //�Ԃ̐��k�̃Q�[�W�pImages
    [SerializeField] private Image[] redGaugeImages;
    [SerializeField] private Button redButtonLeft;
    [SerializeField] private Button redButtonRight;
  
    public Image GaugeImage => gaugeImage;
    public Image[] RedGaugeImages => redGaugeImages;
    public Button RedButtonLeft => redButtonLeft;
    public Button RedButtonRight => redButtonRight;


    //���̐��k�̃{�^��
    [SerializeField]
    private Button WhiteButton;
    // �Q�[�W��Image
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
        // AudioSource�R���|�[�l���g���擾
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
        // �^�[�Q�b�g�{�^����Image�R���|�[�l���g���擾
        redCardImage = redCardButton.GetComponent<Image>();
       
        
    }
    void Start()
    {
        audioSource.loop = true;
        // �I�[�f�B�I���Đ�����
        audioSource.Play();
        redCardButton.interactable = false;
    }

    void Update()
    {
        // �{�^�����N���b�N����Ă��Ȃ��Ƃ��̂݌���������
        if (!gameStateManager.IsButtonClicked && gameStateManager.IsStudent && !gameStateManager.IsClear)
        {
            DecreaseGauge();
        }
        else if (gaugeImage.fillAmount > 0.0f && !gameStateManager.IsClear)
        {
            DecreaseGauge();
        }
    }

    //�摜���\���ɂ���֐�(�Ԃ̐��k�̂�)
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

    //�Ԃ̐��k�̓���̉摜��\������֐�
    public void ShowImagesInRange(int start, int end)
    {
        if (start < 0 || end >= redGaugeImages.Length)
        {
            Debug.LogError("Invalid range provided.");
            return;
        }

        // ���ׂẲ摜����U��\���ɂ���
        HideImages();

        // �w�肳�ꂽ�͈͓��̉摜�݂̂�\������
        for (int i = start; i <= end; i++)
        {
            if (redGaugeImages[i] != null)
            {
                redGaugeImages[i].enabled = true;  // �摜��\������
            }
        }
        if (redButtonLeft != null)
        {
            redButtonLeft.gameObject.SetActive(true);  // �{�^��1��\������
        }
        if (redButtonRight != null)
        {
            redButtonRight.gameObject.SetActive(true);  // �{�^��2��\������
        }
    }

    // ����͈̔͂̉摜���\���ɂ���֐�
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
        // FillAmount��1.0�����̏ꍇ�̂�FillAmount�𑝂₷
        if (redCardImage.fillAmount < gameConstants.GaugeFillAmountThresholdFull)
        {
            Debug.Log("card");
            // FillAmount�𑝂₷
            redCardImage.fillAmount += gameConstants.RedCardFillAmountIncrement;

            // FillAmount��1.0�����̏ꍇ�A�{�^���𖳌�������
            if (redCardImage.fillAmount < gameConstants.GaugeFillAmountThresholdFull)
            {
                redCardButton.interactable = false;
            }
        }
        // FillAmount��1.0�ɒB������{�^���𖳌�������
        else
        {
            redCardButton.interactable = true;
        }
    }

    public void OnClickButton()
    {
        // �{�^�������ɃN���b�N����Ă���ꍇ�́ADecreaseGauge()���Ăяo���Ȃ�
        if (!gameStateManager.IsButtonClicked && !gameStateManager.IsStudent)
        {
            Debug.Log("true");
            IncreaseGauge();
            gameStateManager.IsButtonClicked = true; // �{�^�����N���b�N���ꂽ���Ƃ������t���O��ݒ�
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
            counterText.text = "�X�R�A : " + Score.ToString("N0");
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
        if (redGaugeImages[3].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            IncreaseGauge_W();
            redGaugeImages[1].enabled = false;
        }
        else if (!gameStateManager.IsButton2Enabled) // �{�^��2��������Ă��Ȃ��ꍇ�̂݃{�^��1����������
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

    // �{�^��2���N���b�N���ꂽ���̏���
    public void OnRedButton2Clicked()
    {
        if (redGaugeImages[3].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            IncreaseGauge_W();
            redGaugeImages[0].enabled = false;
        }
        else if (!gameStateManager.IsButtonClicked) // �{�^��1��������Ă��Ȃ��ꍇ�̂݃{�^��2����������
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

    // �Q�[�W�𑝉�������֐�
    void IncreaseGauge()
    {
        // �Q�[�W�̒l�𑝉�������
        gaugeImage.fillAmount += gameConstants.IncreaseAmount;
        if (gaugeImage.fillAmount >= gameConstants.GaugeFillAmountThresholdFull && !gameStateManager.IsClear)
        {
            gameStateManager.IsClear = true;
            GameClearPanel.SetBool("isClear", true);
            Invoke("Clear_Score", 1.5f);
        }
        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        gaugeImage.fillAmount = Mathf.Clamp01(gaugeImage.fillAmount);
    }

    void IncreaseGauge_W()
    {
        if (redGaugeImages[3].fillAmount < 1.0f)
        {
            // �Q�[�W�̒l�𑝉�������
            redGaugeImages[3].fillAmount += gameConstants.RedIncreaseAmount;
        }
        else if(redGaugeImages[3].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            gameStateManager.IsStudent = false;
            OnGaugeFull_red();
        }
        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        redGaugeImages[3].fillAmount = Mathf.Clamp01(redGaugeImages[3].fillAmount);
    }

    // �Q�[�W������������֐�
    void DecreaseGauge()
    {
        // �Q�[�W�̒l�𖈕b�̌������ɉ����Č���������
        //gaugeImage.fillAmount -= currentDecreaseRate * Time.deltaTime;
        gaugeImage.fillAmount -= gameConstants.DecreaseRateDecreaseAmount * Time.deltaTime;

        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        gaugeImage.fillAmount = Mathf.Clamp01(gaugeImage.fillAmount);
    }

    public void RedCard()
    {
        redCardImage.fillAmount = 0.0f;
        gameStateManager.IsStudent = false;
        SeitoRed.SetBool("isRedCard", true);
        SeitoPurple.SetBool("isRedCard", true);
        SeitoWhite.SetBool("isRedCard", true);
        // red�̉摜��\��
        redButtonLeft.gameObject.SetActive(false);
        redButtonRight.gameObject.SetActive(false);
        for (int i = 0; i < redGaugeImages.Length; i++)
        {
            redGaugeImages[i].gameObject.SetActive(false);
        }
        // purple�̉摜��\��
        for (int i = 4; i < 11; i++)
        {
            GameManager.Instance.GetAnimationController().GaugeImages[i].gameObject.SetActive(false);
        }
        // gray�̉摜��\��
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
        // red�̉摜�\��
        redButtonLeft.gameObject.SetActive(true);
        redButtonRight.gameObject.SetActive(true);
        gameStateManager.IsButton1Enabled = false;
        gameStateManager.IsButton2Enabled = false;
        for (int i = 1; i < redGaugeImages.Length; i++)
        {
            redGaugeImages[i].gameObject.SetActive(true);
        }
        // purple�̉摜�\��
        //stickController.gaugeImage.gameObject.SetActive(true);
        for (int i = 1; i < 11; i++)
        {
            GameManager.Instance.GetAnimationController().GaugeImages[i].gameObject.SetActive(true);
        }
        // gray�̉摜�\��
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
       
        // �ȑO�̃R���[�`�������s���ł���΃L�����Z������
        if (purpleCoroutine != null)
        {
            StopCoroutine(purpleCoroutine);
        }
        // �V�����R���[�`�����J�n���A�Q�Ƃ�ێ�����
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
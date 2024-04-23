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

    public Image gaugeImage; // �Q�[�W�̃C���[�W
    public Image w_Image; // �Q�[�W�̃C���[�W
    public Button increaseButton1; // �Q�[�W�𑝉�������{�^��1
    public Button increaseButton2; // �Q�[�W�𑝉�������{�^��2
    public Image[] images;
    
    public float increaseAmount = 0.0001f; // �Q�[�W�̑�����
    public float increaseAmount_w = 0.05f; // �Q�[�W�̑�����
    public float initialDecreaseRate = 0.001f; // �����̃Q�[�W�̖��b�̌�����
    public float decreaseRateDecreaseAmount = 0.001f; // ���b�̌����������炷��

    private bool button1Enabled = true; // �{�^��1���L�����ǂ���
    private bool button2Enabled = false; // �{�^��2���L�����ǂ���

    private bool buttonClicked = false; // �{�^�����N���b�N���ꂽ���ǂ����̃t���O

    // �{�^���𒷉������鎞�Ԃ�臒l
    public float longPressThreshold = 1.0f;

    public Button targetButton; // FillAmount�𑝂₷�Ώۂ̃{�^��
    public float fillAmountIncrement = 0.1f; // FillAmount�𑝂₷��

    private Image targetImage; // FillAmount�𒲐����邽�߂̃C���[�W
    public Text counterText;

    public StickController stickController;
    public LongPressButton longPressButton;
    public TimerCountDown timerCountDown;

    private Coroutine purpleCoroutine; // purpleCoroutine�̎Q�Ƃ�ێ�����

    public bool isStudent = false;
    public bool isClear = false;
    //private bool isCounting = false;

    public int Score = 0; 
    private int Bonus = 0;

    public Button button;

    private void Awake()
    {
        // AudioSource�R���|�[�l���g���擾
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        audioSource.loop = true;
        // �I�[�f�B�I���Đ�����
        audioSource.Play();

        red_animator = redObj.GetComponent<Animator>();
        purple_animator = purpleObj.GetComponent<Animator>();
        gray_animator = grayObj.GetComponent<Animator>();
        Score_animator= Score_obj.GetComponent<Animator>();
        
        // �^�[�Q�b�g�{�^����Image�R���|�[�l���g���擾
        targetImage = targetButton.GetComponent<Image>();
        targetButton.interactable = false;
        increaseButton2.gameObject.SetActive(false); 
    }

    void Update()
    {
        // �{�^�����N���b�N����Ă��Ȃ��Ƃ��̂݌���������
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
        // FillAmount��1.0�����̏ꍇ�̂�FillAmount�𑝂₷
        if (targetImage.fillAmount < 1.0f)
        {
            Debug.Log("card");
            // FillAmount�𑝂₷
            targetImage.fillAmount += fillAmountIncrement;

            // FillAmount��1.0�����̏ꍇ�A�{�^���𖳌�������
            if (targetImage.fillAmount < 1.0f)
            {
                targetButton.interactable = false;
            }
        }
        // FillAmount��1.0�ɒB������{�^���𖳌�������
        else
        {
            targetButton.interactable = true;
        }
    }

    public void OnClickButton()
    {
        // �{�^�������ɃN���b�N����Ă���ꍇ�́ADecreaseGauge()���Ăяo���Ȃ�
        if (!buttonClicked && !isStudent)
        {
            Debug.Log("true");
            IncreaseGauge();
            buttonClicked = true; // �{�^�����N���b�N���ꂽ���Ƃ������t���O��ݒ�
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
        buttonClicked = false; // �{�^�����N���b�N���ꂽ�t���O�����Z�b�g����DecreaseGauge()���ĂьĂяo�����悤�ɂ���
        Debug.Log("false");
    }

    // �{�^��1���N���b�N���ꂽ���̏���
    public void OnIncreaseButton1Clicked()
    {
        if (w_Image.fillAmount >= 0.96f)
        {
            IncreaseGauge_W();
            images[1].gameObject.SetActive(false);
        }
        else if (!button2Enabled) // �{�^��2��������Ă��Ȃ��ꍇ�̂݃{�^��1����������
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

    // �{�^��2���N���b�N���ꂽ���̏���
    public void OnIncreaseButton2Clicked()
    {
        if(w_Image.fillAmount >= 0.96f)
        {
            IncreaseGauge_W();
            images[0].gameObject.SetActive(false);
        }
        else if (!button1Enabled) // �{�^��1��������Ă��Ȃ��ꍇ�̂݃{�^��2����������
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

    // �Q�[�W�𑝉�������֐�
    void IncreaseGauge()
    {
        // �Q�[�W�̒l�𑝉�������
        gaugeImage.fillAmount += increaseAmount;
        if (gaugeImage.fillAmount >= 0.99 && !isClear)
        {
            isClear = true;
            Score_animator.SetBool("isClear", true);
            Invoke("Clear_Score", 1.5f);
        }
        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        gaugeImage.fillAmount = Mathf.Clamp01(gaugeImage.fillAmount);
    }

    void IncreaseGauge_W()
    {
        if (w_Image.fillAmount < 1.0f)
        {

            // �Q�[�W�̒l�𑝉�������
            w_Image.fillAmount += increaseAmount_w;
        }
        else if(w_Image.fillAmount >= 0.96f)
        {
            isStudent = false;
            OnGaugeFull_red();
        }
        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        w_Image.fillAmount = Mathf.Clamp01(w_Image.fillAmount);
    }

    // �Q�[�W������������֐�
    void DecreaseGauge()
    {
        // �Q�[�W�̒l�𖈕b�̌������ɉ����Č���������
        //gaugeImage.fillAmount -= currentDecreaseRate * Time.deltaTime;
        gaugeImage.fillAmount -= decreaseRateDecreaseAmount * Time.deltaTime;

        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        gaugeImage.fillAmount = Mathf.Clamp01(gaugeImage.fillAmount);
    }

    public void RedCard()
    {
        targetImage.fillAmount = 0.0f;
        isStudent = false;
        red_animator.SetBool("isRedCard", true);
        purple_animator.SetBool("isRedCard", true);
        gray_animator.SetBool("isRedCard", true);
        // red�̉摜��\��
        increaseButton1.gameObject.SetActive(false);
        increaseButton2.gameObject.SetActive(false);
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(false);
        }
        // purple�̉摜��\��
        stickController.gaugeImage.gameObject.SetActive(false);
        for (int i = 4; i < 11; i++)
        {
            Ani_con.gauge_Images[i].gameObject.SetActive(false);
        }
        // gray�̉摜��\��
        longPressButton.gray_Button.gameObject.SetActive(false);

        Invoke("ResetAnimatoin", 7.0f);
    }

    public void ResetAnimatoin()
    {
        isStudent = true;
        red_animator.SetBool("isRedCard", false);
        purple_animator.SetBool("isRedCard", false);
        gray_animator.SetBool("isRedCard", false);
        // red�̉摜�\��
        increaseButton1.gameObject.SetActive(true);
        increaseButton2.gameObject.SetActive(true);
        button1Enabled = false;
        button2Enabled = false;
        for (int i = 1; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(true);
        }
        // purple�̉摜�\��
        stickController.gaugeImage.gameObject.SetActive(true);
        for (int i = 1; i < 11; i++)
        {
            Ani_con.gauge_Images[i].gameObject.SetActive(true);
        }
        // gray�̉摜�\��
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
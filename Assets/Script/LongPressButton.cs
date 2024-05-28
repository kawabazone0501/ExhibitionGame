using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LongPressButton : MonoBehaviour
{
    [SerializeField]
    private GameConstants gameConstants;
    [SerializeField]
    private GameStateManager gameStateManager;
    // �{�^����������Ă��邩�ǂ����������t���O
    private bool isButtonPressed = false;
    //// �Q�[�W�̌��݂̒l
    private float currentGaugeValue = 0.0f;

    private Animator SeitoWhite;
    private Animator Teacher;
    private Animator Phone;

    //���̐��k�̃{�^���pImage 
    [SerializeField]
    private Button whiteButton;
    // ���̐��k�̃Q�[�W�̊O���pImage
    [SerializeField]
    private Image whiteOutGaugeImage;
    //���̐��k�̃Q�[�W�̓����pImage(FillAmount��M���)
    [SerializeField]
    private Image whiteInGaugeImage;
    
    //�Q�Ƃ��锒�̐��k�̃{�^��Image 
    public Button WhiteButton => whiteButton;
    //�Q�Ƃ��锒�̐��k�̃Q�[�W�̊O���pImage
    public Image WhiteOutGaugeImage => whiteOutGaugeImage;
    //�Q�Ƃ��锒�̐��k�̃Q�[�W�̓����pImage(FillAmount��M���)
    public Image WhiteInGaugeImage => whiteInGaugeImage;

    //private bool isGaugeFull_gray = false; // �Q�[�W�����^�����ǂ����̃t���O
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
    }
    private void Start()
    {

        AnimatorController animatorController = FindAnyObjectByType<AnimatorController>();
        if (animatorController != null)
        {
            SeitoWhite = animatorController.SeitoWhite;
            Teacher = animatorController.Teacher;
            Phone = animatorController.Phone;
        }
    }

    public void HideImages()
    {
        if (whiteOutGaugeImage != null)
        {
            whiteOutGaugeImage.enabled = false;
        }
        if (whiteButton != null)
        {
            whiteButton.gameObject.SetActive(false);// Button���̂��\���ɂ���
        }
        if (whiteInGaugeImage != null)
        {
            whiteInGaugeImage.fillAmount = 0.0f;
            whiteInGaugeImage.enabled = false;
        }
    }

    public void ShowImages()
    {
        if (whiteOutGaugeImage != null)
        {
            whiteOutGaugeImage.enabled = true;
        }
        if (whiteButton != null)
        {
            whiteButton.gameObject.SetActive(true);
        }
        if(whiteInGaugeImage != null)
        {
            whiteInGaugeImage.enabled = true;
        }
    }

    // �{�^���������ꂽ�Ƃ��̏���
    public void OnPointerDown()
    {
        isButtonPressed = true;
        SeitoWhite.SetBool("isWhite", true);
        Phone.SetBool("isSupport", true);
        Phone.SetBool("isCall", false);
        Teacher.SetBool("vsWhite", true);
    }

    // �{�^���������ꂽ�Ƃ��̏���
    public void OnPointerUp()
    {
        isButtonPressed = false;
        Phone.SetBool("isSupport", false);
        Phone.SetBool("isCall", true);
        Teacher.SetBool("vsWhite", false);
    }

    // �X�V����
    void Update()
    {
        if (isButtonPressed)
        {
            if (currentGaugeValue < gameConstants.GaugeFillAmountThresholdFull)
            {
                currentGaugeValue += gameConstants.GaugeIncreaseRate * Time.deltaTime;
                UpdateGauge();
            }
            else if (whiteInGaugeImage.fillAmount >= gameConstants.GaugeFillAmountThreshold)
            {
                gameStateManager.IsStudent= false;
                Teacher.SetBool("vsWhite", false);
                Phone.SetBool("isSupport", false);
                isButtonPressed = false;
                currentGaugeValue = 0.0f;
                GameManager.Instance.GetGaugeController().OnGaugeFull_gray();
            }
        }
    }
    // �Q�[�W�̍X�V
    private void UpdateGauge()
    {
        whiteInGaugeImage.fillAmount = currentGaugeValue / gameConstants.GaugeFillAmountThresholdFull;

        whiteInGaugeImage.fillAmount=Mathf.Clamp01(whiteInGaugeImage.fillAmount);
    }
}
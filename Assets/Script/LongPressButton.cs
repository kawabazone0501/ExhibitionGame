using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LongPressButton : MonoBehaviour
{
    Animator KB_animator;
    public GameObject kobaObj;

    public GaugeController g_Controller;

    // �{�^���𒷉������鎞�Ԃ�臒l
    public float longPressThreshold = 1.0f;

    // �{�^����������Ă��邩�ǂ����������t���O
    private bool isButtonPressed = false;

    // �{�^����������n�߂�����
    private float buttonPressedTime = 0.0f;

    public Button gray_Button;

    // �Q�[�W��Image
    public Image gaugeImage;

    // �Q�[�W�̑������x
    public float gaugeIncreaseRate = 0.5f;

    // �Q�[�W�̍ő�l
    public float maxGaugeValue = 1.0f;

    // �Q�[�W�̌��݂̒l
    private float currentGaugeValue = 0.0f;

    public Animator gray_animator;
    public Animator phone_animator;

    public GameObject grayObj;
    public GameObject phoneObj;

    //private bool isGaugeFull_gray = false; // �Q�[�W�����^�����ǂ����̃t���O

    private void Start()
    {
        gray_animator = grayObj.GetComponent<Animator>();
        phone_animator = phoneObj.GetComponent<Animator>();
        KB_animator = kobaObj.GetComponent<Animator>();
    }

    // �{�^���������ꂽ�Ƃ��̏���
    public void OnPointerDown()
    {
        isButtonPressed = true;
        gray_animator.SetBool("isGray", true);
        phone_animator.SetBool("isSupport", true);
        phone_animator.SetBool("isCall", false);
        KB_animator.SetBool("vsGray", true);
        buttonPressedTime = Time.time;
    }

    // �{�^���������ꂽ�Ƃ��̏���
    public void OnPointerUp()
    {
        isButtonPressed = false;
        phone_animator.SetBool("isSupport", false);
        phone_animator.SetBool("isCall", true);
        KB_animator.SetBool("vsGray", false);
    }

    // �X�V����
    void Update()
    {
        if (isButtonPressed)
        {
            if (currentGaugeValue < maxGaugeValue)
            {
                currentGaugeValue += gaugeIncreaseRate * Time.deltaTime;
                UpdateGauge();
            }
            else if (gaugeImage.fillAmount >= 0.96f)
            {
                g_Controller.isStudent = false;
                KB_animator.SetBool("vsGray", false);
                phone_animator.SetBool("isSupport", false);
                isButtonPressed = false;
                currentGaugeValue = 0.0f;
                g_Controller.OnGaugeFull_gray();
            }
        }
    }
    // �Q�[�W�̍X�V
    private void UpdateGauge()
    {
        gaugeImage.fillAmount = currentGaugeValue / maxGaugeValue;

        gaugeImage.fillAmount=Mathf.Clamp01(gaugeImage.fillAmount);
    }
}
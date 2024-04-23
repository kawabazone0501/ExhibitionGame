using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LongPressButton : MonoBehaviour
{
    Animator KB_animator;
    public GameObject kobaObj;

    public GaugeController g_Controller;

    // ボタンを長押しする時間の閾値
    public float longPressThreshold = 1.0f;

    // ボタンが押されているかどうかを示すフラグ
    private bool isButtonPressed = false;

    // ボタンが押され始めた時間
    private float buttonPressedTime = 0.0f;

    public Button gray_Button;

    // ゲージのImage
    public Image gaugeImage;

    // ゲージの増加速度
    public float gaugeIncreaseRate = 0.5f;

    // ゲージの最大値
    public float maxGaugeValue = 1.0f;

    // ゲージの現在の値
    private float currentGaugeValue = 0.0f;

    public Animator gray_animator;
    public Animator phone_animator;

    public GameObject grayObj;
    public GameObject phoneObj;

    //private bool isGaugeFull_gray = false; // ゲージが満タンかどうかのフラグ

    private void Start()
    {
        gray_animator = grayObj.GetComponent<Animator>();
        phone_animator = phoneObj.GetComponent<Animator>();
        KB_animator = kobaObj.GetComponent<Animator>();
    }

    // ボタンが押されたときの処理
    public void OnPointerDown()
    {
        isButtonPressed = true;
        gray_animator.SetBool("isGray", true);
        phone_animator.SetBool("isSupport", true);
        phone_animator.SetBool("isCall", false);
        KB_animator.SetBool("vsGray", true);
        buttonPressedTime = Time.time;
    }

    // ボタンが離されたときの処理
    public void OnPointerUp()
    {
        isButtonPressed = false;
        phone_animator.SetBool("isSupport", false);
        phone_animator.SetBool("isCall", true);
        KB_animator.SetBool("vsGray", false);
    }

    // 更新処理
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
    // ゲージの更新
    private void UpdateGauge()
    {
        gaugeImage.fillAmount = currentGaugeValue / maxGaugeValue;

        gaugeImage.fillAmount=Mathf.Clamp01(gaugeImage.fillAmount);
    }
}
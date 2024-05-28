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
    // ボタンが押されているかどうかを示すフラグ
    private bool isButtonPressed = false;
    //// ゲージの現在の値
    private float currentGaugeValue = 0.0f;

    private Animator SeitoWhite;
    private Animator Teacher;
    private Animator Phone;

    //白の生徒のボタン用Image 
    [SerializeField]
    private Button whiteButton;
    // 白の生徒のゲージの外側用Image
    [SerializeField]
    private Image whiteOutGaugeImage;
    //白の生徒のゲージの内側用Image(FillAmountを弄る方)
    [SerializeField]
    private Image whiteInGaugeImage;
    
    //参照する白の生徒のボタンImage 
    public Button WhiteButton => whiteButton;
    //参照する白の生徒のゲージの外側用Image
    public Image WhiteOutGaugeImage => whiteOutGaugeImage;
    //参照する白の生徒のゲージの内側用Image(FillAmountを弄る方)
    public Image WhiteInGaugeImage => whiteInGaugeImage;

    //private bool isGaugeFull_gray = false; // ゲージが満タンかどうかのフラグ
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
            whiteButton.gameObject.SetActive(false);// Button自体を非表示にする
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

    // ボタンが押されたときの処理
    public void OnPointerDown()
    {
        isButtonPressed = true;
        SeitoWhite.SetBool("isWhite", true);
        Phone.SetBool("isSupport", true);
        Phone.SetBool("isCall", false);
        Teacher.SetBool("vsWhite", true);
    }

    // ボタンが離されたときの処理
    public void OnPointerUp()
    {
        isButtonPressed = false;
        Phone.SetBool("isSupport", false);
        Phone.SetBool("isCall", true);
        Teacher.SetBool("vsWhite", false);
    }

    // 更新処理
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
    // ゲージの更新
    private void UpdateGauge()
    {
        whiteInGaugeImage.fillAmount = currentGaugeValue / gameConstants.GaugeFillAmountThresholdFull;

        whiteInGaugeImage.fillAmount=Mathf.Clamp01(whiteInGaugeImage.fillAmount);
    }
}
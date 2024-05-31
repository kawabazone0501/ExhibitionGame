using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class LongPressButton : MonoBehaviour
{
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private GameManager gameManager;

    private Animator SeitoWhite;
    private Animator Teacher;
    private Animator Phone;
    // �Q�[�W�̌��݂̒l
    private float currentGaugeValue = 0.0f;
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
    // �{�^���������ꂽ�Ƃ��̏���
    public void OnPointerDown()
    {
        gameStateManager.IsButtonPressed = true;
        SeitoWhite.SetBool("isWhite", true);
        Phone.SetBool("isSupport", true);
        Phone.SetBool("isCall", false);
        Teacher.SetBool("vsWhite", true);
    }

    // �{�^���������ꂽ�Ƃ��̏���
    public void OnPointerUp()
    {
        gameStateManager.IsButtonPressed = false;
        Phone.SetBool("isSupport", false);
        Phone.SetBool("isCall", true);
        Teacher.SetBool("vsWhite", false);
    }

    // �X�V����
    void Update()
    {
        if (gameStateManager.IsButtonPressed)
        {
            if (currentGaugeValue < gameConstants.GaugeFillAmountThresholdFull)
            {
                currentGaugeValue += gameConstants.GaugeIncreaseRate * Time.deltaTime;
                UpdateGauge();
            }
            else if (gameManager.GaugeImages[gameConstants.WhiteGauge].fillAmount >= gameConstants.GaugeFillAmountThreshold)
            {
                Teacher.SetBool("vsWhite", false);
                Phone.SetBool("isSupport", false);
                gameStateManager.IsButtonPressed = false;
                gameStateManager.IsStudents[gameConstants.StudentWHITE] = false;
                currentGaugeValue = 0.0f;
                gameManager.GetGaugeController().OnGaugeFullWhite();
            }
        }
    }
    // �Q�[�W�̍X�V
    private void UpdateGauge()
    {
        gameManager.GaugeImages[gameConstants.WhiteGauge].fillAmount = currentGaugeValue / gameConstants.GaugeFillAmountThresholdFull;

        gameManager.GaugeImages[gameConstants.WhiteGauge].fillAmount=Mathf.Clamp01(gameManager.GaugeImages[gameConstants.WhiteGauge].fillAmount);
    }
}
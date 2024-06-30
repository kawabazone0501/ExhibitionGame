using UnityEngine;

public class LongPressButton : MonoBehaviour
{
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private UIManager uiManager;

    private Animator SeitoWhite;
    private Animator Teacher;
    private Animator Phone;
    private Animator WhiteGuide;
    // �Q�[�W�̌��݂̒l
    private float currentGaugeValue = 0.0f;

    private bool isGuidePlayed = false;
    private void Start()
    {

        AnimatorController animatorController = FindAnyObjectByType<AnimatorController>();
        if (animatorController != null)
        {
            SeitoWhite = animatorController.SeitoWhite;
            Teacher = animatorController.Teacher;
            Phone = animatorController.Phone;
            WhiteGuide = animatorController.WhiteGuide;
        }
    }
    // �{�^���������ꂽ�Ƃ��̏���
    public void OnPointerDown()
    {
        if (gameStateManager.WhiteGuidePlayed && !isGuidePlayed)
        {
            isGuidePlayed = true;
            WhiteGuide.SetBool("isWhiteGuide", false);
        }
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
            else if (uiManager.GaugeImages[gameConstants.WhiteGauge].fillAmount >= gameConstants.GaugeFillAmountThreshold)
            {
                Teacher.SetBool("vsWhite", false);
                Phone.SetBool("isSupport", false);
                gameStateManager.IsButtonPressed = false;
                gameStateManager.IsStudents[gameConstants.StudentWHITE] = false;
                Debug.Log(gameStateManager.IsStudents[gameConstants.StudentWHITE]);
                currentGaugeValue = 0.0f;
                uiManager.GetGaugeController().OnGaugeFullWhite();
            }
        }
    }
    // �Q�[�W�̍X�V
    private void UpdateGauge()
    {
        uiManager.GaugeImages[gameConstants.WhiteGauge].fillAmount = currentGaugeValue / gameConstants.GaugeFillAmountThresholdFull;

        uiManager.GaugeImages[gameConstants.WhiteGauge].fillAmount=Mathf.Clamp01(uiManager.GaugeImages[gameConstants.WhiteGauge].fillAmount);
    }
}
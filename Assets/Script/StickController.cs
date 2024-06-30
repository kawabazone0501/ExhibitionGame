using UnityEngine;
using UnityEngine.EventSystems;

public class StickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Animator Teacher;
    private Animator PurpleGuide;
    [SerializeField]
    private GameConstants gameConstants;
    [SerializeField]
    private GameStateManager gameStateManager;
    [SerializeField]
    private UIManager uiManager;

    private RectTransform stickTransform; // �X�e�B�b�N��RectTransform
    private RectTransform backgroundTransform; // �X�e�B�b�N�̔w�i��RectTransform

    private Vector2 stickStartPosition; // �X�e�B�b�N�̏����ʒu
    private Vector2 stickDirection; // �X�e�B�b�N�̕���
    private float totalRotation = 0f; // �X�e�B�b�N�̑���]��
    public float TotalRotation => totalRotation;
    private Vector2 prevStickDirection; // �O�̃t���[���̃X�e�B�b�N�̕���

    // �����ʒu���L�^���邽�߂̕ϐ�
    private Vector2 initialPosition;

    // UI�摜��RectTransform
    public RectTransform imageRectTransform;

    private bool isGuidePlayed = false;

    
    
    void Awake()
    {
       
        AnimatorController animatorController = FindAnyObjectByType<AnimatorController>();
        if (animatorController != null)
        {
            Teacher = animatorController.Teacher;
            PurpleGuide = animatorController.PurpleGuide;
        }
        stickTransform = transform.GetChild(0).GetComponent<RectTransform>(); // �X�e�B�b�N�̎q�v�f����RectTransform���擾
        backgroundTransform = GetComponent<RectTransform>(); // �X�e�B�b�N�̔w�i��RectTransform���擾
    }

    void Start()
    {
        stickStartPosition = stickTransform.anchoredPosition; // �X�e�B�b�N�̏����ʒu���擾
        prevStickDirection = Vector2.right; // �����l��ݒ�
        // UI�v�f�̏����ʒu���L�^����
        initialPosition = imageRectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameStateManager.PurpleGuidePlayed && !isGuidePlayed)
        {
            isGuidePlayed = true;
            PurpleGuide.SetBool("isPurpleGuide", false);
        }
        OnDrag(eventData);
        Teacher.SetBool("vsPurple", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Teacher.SetBool("vsPurple", false);
        stickTransform.anchoredPosition = stickStartPosition; // �X�e�B�b�N�������ʒu�ɖ߂�
        stickDirection = Vector2.zero; // �X�e�B�b�N�̕��������Z�b�g����
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / backgroundTransform.sizeDelta.x);
            pos.y = (pos.y / backgroundTransform.sizeDelta.y);

            Vector2 pivot = backgroundTransform.pivot;
            float x = (backgroundTransform.sizeDelta.x * pivot.x) + pos.x * backgroundTransform.sizeDelta.x;
            float y = (backgroundTransform.sizeDelta.y * pivot.y) + pos.y * backgroundTransform.sizeDelta.y;

            Vector2 movePos = new Vector2(x, y);
            Vector2 clampPos = movePos - backgroundTransform.sizeDelta / 2f;

            // �X�e�B�b�N���������Ƃ��ł���͈͂𐧌�����
            if (Vector2.Distance(stickStartPosition, clampPos) <= gameConstants.MaxStickDistance)
            {
                stickTransform.anchoredPosition = clampPos;
            }
            else
            {
                stickTransform.anchoredPosition = stickStartPosition + (clampPos - stickStartPosition).normalized * gameConstants.MaxStickDistance;
            }

            stickDirection = (stickTransform.anchoredPosition - stickStartPosition).normalized; // �X�e�B�b�N�̕������v�Z����

            // �X�e�B�b�N�̉�]�ʂ��X�V����
            float rotationAmount = Vector2.SignedAngle(prevStickDirection, stickDirection);
            totalRotation += rotationAmount;

            // �O�̃t���[���̃X�e�B�b�N�̕������X�V����
            prevStickDirection = stickDirection;

            // �X�e�B�b�N��1�������ꍇ�A�Q�[�W�𑝂₵�ă��Z�b�g����
            if (Mathf.Abs(totalRotation) >= gameConstants.RotationThreshold)
            {
                IncreaseGauge();
                totalRotation = 0f; // ����]�ʂ����Z�b�g����
            }
        }
    }

    public Vector2 GetStickDirection()
    {
        return stickDirection; // ���݂̃X�e�B�b�N�̕�����Ԃ�
    }

    private void IncreaseGauge()
    {
        if (uiManager.GaugeImages[gameConstants.PurpleGauge].fillAmount < 1.0f)
        {
            // �Q�[�W�̒l�𑝉�������
            Debug.Log("����");
            uiManager.GaugeImages[gameConstants.PurpleGauge].fillAmount += gameConstants.PurpleIncreaseAmount;
        }
        else if (uiManager.GaugeImages[gameConstants.PurpleGauge].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            Debug.Log("�I��");
            Teacher.SetBool("vsPurple", false);
            ResetToInitialPosition();
            gameStateManager.IsStudents[gameConstants.StudentPURPLE] = false;
            uiManager.GetGaugeController().OnGaugeFull_purple();
            totalRotation = 0f;
        }
        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        uiManager.GaugeImages[gameConstants.PurpleGauge].fillAmount = Mathf.Clamp01(uiManager.GaugeImages[gameConstants.PurpleGauge].fillAmount);
    }

    // ���W�������ʒu�Ƀ��Z�b�g����֐�
    public void ResetToInitialPosition()
    {
        Teacher.SetBool("vsPurple", false);
        imageRectTransform.anchoredPosition = initialPosition;
    }
}
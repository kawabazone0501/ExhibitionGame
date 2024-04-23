using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class StickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    Animator KB_animator;
    public GameObject kobaObj;

    public GaugeController g_Controller;
    

    private RectTransform stickTransform; // �X�e�B�b�N��RectTransform
    private RectTransform backgroundTransform; // �X�e�B�b�N�̔w�i��RectTransform

    private Vector2 stickStartPosition; // �X�e�B�b�N�̏����ʒu
    private Vector2 stickDirection; // �X�e�B�b�N�̕���

    private float maxStickDistance = 75f; // �X�e�B�b�N���������Ƃ��ł���ő勗��

    private float totalRotation = 0f; // �X�e�B�b�N�̑���]��
    private float rotationThreshold = 360f; // 1���Ƃ݂Ȃ���]�ʂ�臒l

    public Image gaugeImage; // �Q�[�W�̃C���[�W
    public Image buttonImage;
    public float increaseAmount = 0.1f; // �Q�[�W�̑�����

    private Vector2 prevStickDirection; // �O�̃t���[���̃X�e�B�b�N�̕���

    //private bool isGaugeFull = false; // �Q�[�W�����^�����ǂ����̃t���O

    // �����ʒu���L�^���邽�߂̕ϐ�
    private Vector2 initialPosition;

    // UI�摜��RectTransform
    public RectTransform imageRectTransform;

    void Start()
    {
        KB_animator = kobaObj.GetComponent<Animator>();
        stickTransform = transform.GetChild(0).GetComponent<RectTransform>(); // �X�e�B�b�N�̎q�v�f����RectTransform���擾
        backgroundTransform = GetComponent<RectTransform>(); // �X�e�B�b�N�̔w�i��RectTransform���擾
        stickStartPosition = stickTransform.anchoredPosition; // �X�e�B�b�N�̏����ʒu���擾
        prevStickDirection = Vector2.right; // �����l��ݒ�
        // UI�v�f�̏����ʒu���L�^����
        initialPosition = imageRectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        KB_animator.SetBool("vsPurple", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        KB_animator.SetBool("vsPurple", false);
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
            if (Vector2.Distance(stickStartPosition, clampPos) <= maxStickDistance)
            {
                stickTransform.anchoredPosition = clampPos;
            }
            else
            {
                stickTransform.anchoredPosition = stickStartPosition + (clampPos - stickStartPosition).normalized * maxStickDistance;
            }

            stickDirection = (stickTransform.anchoredPosition - stickStartPosition).normalized; // �X�e�B�b�N�̕������v�Z����

            // �X�e�B�b�N�̉�]�ʂ��X�V����
            float rotationAmount = Vector2.SignedAngle(prevStickDirection, stickDirection);
            totalRotation += rotationAmount;

            // �O�̃t���[���̃X�e�B�b�N�̕������X�V����
            prevStickDirection = stickDirection;

            // �X�e�B�b�N��1�������ꍇ�A�Q�[�W�𑝂₵�ă��Z�b�g����
            if (Mathf.Abs(totalRotation) >= rotationThreshold)
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
        if (gaugeImage.fillAmount < 1.0f)
        {
            // �Q�[�W�̒l�𑝉�������
            gaugeImage.fillAmount += increaseAmount;
        }
        else if (gaugeImage.fillAmount >= 0.96f)
        {
            g_Controller.isStudent = false;
            KB_animator.SetBool("vsPurple", false);
            ResetToInitialPosition();
            g_Controller.OnGaugeFull_purple();
        }


        // �Q�[�W�̒l��0����1�͈̔͂ɃN�����v����
        gaugeImage.fillAmount = Mathf.Clamp01(gaugeImage.fillAmount);
    }

    // ���W�������ʒu�Ƀ��Z�b�g����֐�
    public void ResetToInitialPosition()
    {
        KB_animator.SetBool("vsPurple", false);
        imageRectTransform.anchoredPosition = initialPosition;
    }
}
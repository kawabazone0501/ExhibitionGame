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
    

    private RectTransform stickTransform; // スティックのRectTransform
    private RectTransform backgroundTransform; // スティックの背景のRectTransform

    private Vector2 stickStartPosition; // スティックの初期位置
    private Vector2 stickDirection; // スティックの方向

    private float maxStickDistance = 75f; // スティックが動くことができる最大距離

    private float totalRotation = 0f; // スティックの総回転量
    private float rotationThreshold = 360f; // 1周とみなす回転量の閾値

    public Image gaugeImage; // ゲージのイメージ
    public Image buttonImage;
    public float increaseAmount = 0.1f; // ゲージの増加量

    private Vector2 prevStickDirection; // 前のフレームのスティックの方向

    //private bool isGaugeFull = false; // ゲージが満タンかどうかのフラグ

    // 初期位置を記録するための変数
    private Vector2 initialPosition;

    // UI画像のRectTransform
    public RectTransform imageRectTransform;

    void Start()
    {
        KB_animator = kobaObj.GetComponent<Animator>();
        stickTransform = transform.GetChild(0).GetComponent<RectTransform>(); // スティックの子要素からRectTransformを取得
        backgroundTransform = GetComponent<RectTransform>(); // スティックの背景のRectTransformを取得
        stickStartPosition = stickTransform.anchoredPosition; // スティックの初期位置を取得
        prevStickDirection = Vector2.right; // 初期値を設定
        // UI要素の初期位置を記録する
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
        stickTransform.anchoredPosition = stickStartPosition; // スティックを初期位置に戻す
        stickDirection = Vector2.zero; // スティックの方向をリセットする
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

            // スティックが動くことができる範囲を制限する
            if (Vector2.Distance(stickStartPosition, clampPos) <= maxStickDistance)
            {
                stickTransform.anchoredPosition = clampPos;
            }
            else
            {
                stickTransform.anchoredPosition = stickStartPosition + (clampPos - stickStartPosition).normalized * maxStickDistance;
            }

            stickDirection = (stickTransform.anchoredPosition - stickStartPosition).normalized; // スティックの方向を計算する

            // スティックの回転量を更新する
            float rotationAmount = Vector2.SignedAngle(prevStickDirection, stickDirection);
            totalRotation += rotationAmount;

            // 前のフレームのスティックの方向を更新する
            prevStickDirection = stickDirection;

            // スティックが1周した場合、ゲージを増やしてリセットする
            if (Mathf.Abs(totalRotation) >= rotationThreshold)
            {
                IncreaseGauge();
                totalRotation = 0f; // 総回転量をリセットする
            }
        }
    }

    public Vector2 GetStickDirection()
    {
        return stickDirection; // 現在のスティックの方向を返す
    }

    private void IncreaseGauge()
    {
        if (gaugeImage.fillAmount < 1.0f)
        {
            // ゲージの値を増加させる
            gaugeImage.fillAmount += increaseAmount;
        }
        else if (gaugeImage.fillAmount >= 0.96f)
        {
            g_Controller.isStudent = false;
            KB_animator.SetBool("vsPurple", false);
            ResetToInitialPosition();
            g_Controller.OnGaugeFull_purple();
        }


        // ゲージの値を0から1の範囲にクランプする
        gaugeImage.fillAmount = Mathf.Clamp01(gaugeImage.fillAmount);
    }

    // 座標を初期位置にリセットする関数
    public void ResetToInitialPosition()
    {
        KB_animator.SetBool("vsPurple", false);
        imageRectTransform.anchoredPosition = initialPosition;
    }
}
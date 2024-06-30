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

    private RectTransform stickTransform; // スティックのRectTransform
    private RectTransform backgroundTransform; // スティックの背景のRectTransform

    private Vector2 stickStartPosition; // スティックの初期位置
    private Vector2 stickDirection; // スティックの方向
    private float totalRotation = 0f; // スティックの総回転量
    public float TotalRotation => totalRotation;
    private Vector2 prevStickDirection; // 前のフレームのスティックの方向

    // 初期位置を記録するための変数
    private Vector2 initialPosition;

    // UI画像のRectTransform
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
        stickTransform = transform.GetChild(0).GetComponent<RectTransform>(); // スティックの子要素からRectTransformを取得
        backgroundTransform = GetComponent<RectTransform>(); // スティックの背景のRectTransformを取得
    }

    void Start()
    {
        stickStartPosition = stickTransform.anchoredPosition; // スティックの初期位置を取得
        prevStickDirection = Vector2.right; // 初期値を設定
        // UI要素の初期位置を記録する
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
            if (Vector2.Distance(stickStartPosition, clampPos) <= gameConstants.MaxStickDistance)
            {
                stickTransform.anchoredPosition = clampPos;
            }
            else
            {
                stickTransform.anchoredPosition = stickStartPosition + (clampPos - stickStartPosition).normalized * gameConstants.MaxStickDistance;
            }

            stickDirection = (stickTransform.anchoredPosition - stickStartPosition).normalized; // スティックの方向を計算する

            // スティックの回転量を更新する
            float rotationAmount = Vector2.SignedAngle(prevStickDirection, stickDirection);
            totalRotation += rotationAmount;

            // 前のフレームのスティックの方向を更新する
            prevStickDirection = stickDirection;

            // スティックが1周した場合、ゲージを増やしてリセットする
            if (Mathf.Abs(totalRotation) >= gameConstants.RotationThreshold)
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
        if (uiManager.GaugeImages[gameConstants.PurpleGauge].fillAmount < 1.0f)
        {
            // ゲージの値を増加させる
            Debug.Log("増加");
            uiManager.GaugeImages[gameConstants.PurpleGauge].fillAmount += gameConstants.PurpleIncreaseAmount;
        }
        else if (uiManager.GaugeImages[gameConstants.PurpleGauge].fillAmount >= gameConstants.GaugeFillAmountThreshold)
        {
            Debug.Log("終了");
            Teacher.SetBool("vsPurple", false);
            ResetToInitialPosition();
            gameStateManager.IsStudents[gameConstants.StudentPURPLE] = false;
            uiManager.GetGaugeController().OnGaugeFull_purple();
            totalRotation = 0f;
        }
        // ゲージの値を0から1の範囲にクランプする
        uiManager.GaugeImages[gameConstants.PurpleGauge].fillAmount = Mathf.Clamp01(uiManager.GaugeImages[gameConstants.PurpleGauge].fillAmount);
    }

    // 座標を初期位置にリセットする関数
    public void ResetToInitialPosition()
    {
        Teacher.SetBool("vsPurple", false);
        imageRectTransform.anchoredPosition = initialPosition;
    }
}
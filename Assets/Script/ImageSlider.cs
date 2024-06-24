using UnityEngine;
using UnityEngine.UI;

public class ImageSlider : MonoBehaviour
{
    public Image image1;
    public Image image2;

    private bool showingImage1 = true;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float minSwipeDistance = 50f;

    void Update()
    {
        //DetectSwipe();
    }

    void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                if (IsSwipe())
                {
                    ToggleImages();
                }
            }
        }

#if UNITY_EDITOR
        // エディタでのテスト用にマウス操作も追加
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = (Vector2)Input.mousePosition;
            if (IsSwipe())
            {
                ToggleImages();
            }
        }
#endif
    }

    bool IsSwipe()
    {
        return Vector2.Distance(startTouchPosition, endTouchPosition) >= minSwipeDistance;
    }

    void ToggleImages()
    {
        showingImage1 = !showingImage1;
        image1.enabled = showingImage1;
        image2.enabled = !showingImage1;
    }

    // パラメータなしのメソッドを追加
    public void SwitchImages()
    {
        ToggleImages();
    }
}
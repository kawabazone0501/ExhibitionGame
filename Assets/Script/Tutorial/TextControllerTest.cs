using UnityEngine;
using UnityEngine.UI;

public class TextControllerTest : MonoBehaviour
{
    public TextDisplay textDisplay; // ← 先ほどの TextDisplay スクリプトをアタッチしたオブジェクトを指定
    public KeyCode nextKey = KeyCode.Space; // 次の文章に進むキー
    public KeyCode toggleKey = KeyCode.H;   // 表示/非表示を切り替えるキー

    void Update()
    {
        // スペースキーで次の文章へ
        if (Input.GetKeyDown(nextKey))
        {
            textDisplay.NextSection();
        }

        // Hキーでテキストを隠したり、再表示したり
        if (Input.GetKeyDown(toggleKey))
        {
            if (textDisplay.gameObject.activeSelf)
            {
                textDisplay.HideSection();
            }
            else
            {
                textDisplay.ShowNextSection();
            }
        }
    }
}

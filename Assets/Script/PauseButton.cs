using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseButton : MonoBehaviour
{
    private int previousSceneIndex;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameConstants gameConstants;

    private Animator FadePanel;

    // Start is called before the first frame update
    private void Awake()
    {
        AnimatorController animatorController = uiManager.GetComponent<AnimatorController>();
        if(animatorController != null )
        {
            FadePanel = animatorController.FadePanel;
        }
    }

    
    public void OnPausePanel()
    {
        uiManager.PauseShow(6,8);
        Time.timeScale = 0.0f;
    }

    public void OffPausePanel()
    {
        Time.timeScale = 1.0f;
        uiManager.PauseHide(6,8);
    }

    public void OnSelectButton()
    {
        Time.timeScale = 1.0f;

        FadePanel.SetBool("isFadeIn", true);
        StartCoroutine(OnSelectLoad());
    }

    private IEnumerator OnSelectLoad()
    {
        yield return new WaitForSeconds(gameConstants.FadeWaitTime);
        // 前のシーンのインデックスを取得する
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;

        // 前のシーンに戻る
        SceneManager.LoadScene(previousSceneIndex);
    }

    public void OnTitleButton()
    {
        Time.timeScale = 1.0f;
        FadePanel.SetBool("isFadeIn", true);
        StartCoroutine(OnTitleLoad());
    }  


    public IEnumerator OnTitleLoad()
    {
        yield return new WaitForSeconds(gameConstants.FadeWaitTime); 
        // 前のシーンのインデックスを取得する
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 2;

        // 前のシーンに戻る
        SceneManager.LoadScene(previousSceneIndex);
    }
}

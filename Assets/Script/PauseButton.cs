using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private int previousSceneIndex;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameConstants gameConstants;

    private Animator FadePanel;

    // Start is called before the first frame update
    private void Awake()
    {
        AnimatorController animatorController = gameManager.GetComponent<AnimatorController>();
        if(animatorController != null )
        {
            FadePanel = animatorController.FadePanel;
        }
    }

    
    public void OnPausePanel()
    {
       gameManager.PauseShow(6,8);
        Time.timeScale = 0.0f;
    }

    public void OffPausePanel()
    {
        Time.timeScale = 1.0f;
        gameManager.PauseHide(6,8);
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

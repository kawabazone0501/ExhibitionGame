using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerCountDown : MonoBehaviour
{
    private Animator FadePanel;
    private Animator GameOverPanel;
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private UIManager uiManager;
    private float timeRemaining;//ゲームの残り時間

    private void Awake()
    {
        
        AnimatorController animatorController = FindAnyObjectByType<AnimatorController>();
        if(animatorController != null )
        {
            FadePanel = animatorController.FadePanel;
            GameOverPanel = animatorController.GameOverPanel;
        }
        
        FadePanel.SetBool("isFadeOut", true);
    }

    void Start()
    {
        timeRemaining = gameConstants.TotalTime;
        UpdateTimerDisplay();
        InvokeRepeating("UpdateTimer", 1.0f, 1.0f);
        FadePanel.SetBool("isFadeOut",true);
        
    }

    void UpdateTimer()
    {
        FadePanel.SetBool("isFadeOut", false);
        if (timeRemaining > 0)
        {
            timeRemaining -= 1.0f;
            UpdateTimerDisplay();
        }
        else
        {
            // Timer has reached zero
            CancelInvoke("UpdateTimer");
            GameOverPanel.SetBool("isOver", true);
            Invoke("ButtonChoice", gameConstants.ButtonDisplayWaitingTime); ;
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        uiManager.TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ButtonChoice()
    {
        GameOverPanel.SetBool("isChoice", true);
    }

    public void TitleButton()
    {
        if (uiManager.GetAnimationController().MaxObjectsSpawn == gameConstants.FirstSeason)
        {
            PlayerPrefs.SetInt("Score_1", uiManager.GetGaugeController().Score);
            PlayerPrefs.Save();
        }
        if(uiManager.GetAnimationController().MaxObjectsSpawn == gameConstants.SecondSeason)
        {
            PlayerPrefs.SetInt("Score_2", uiManager.GetGaugeController().Score);
            Debug.Log(uiManager.GetGaugeController().Score);
            PlayerPrefs.Save();
        }
        if(uiManager.GetAnimationController().MaxObjectsSpawn == gameConstants.ThirdSeason)
        {
            PlayerPrefs.SetInt("Score_3", uiManager.GetGaugeController().Score);
            Debug.Log(uiManager.GetGaugeController().Score);
            PlayerPrefs.Save();
        }
       FadePanel.SetBool("isFadeIn", true);

        StartCoroutine(TitleSceneLoad());
    }
    public void SelectButton()
    {
       FadePanel.SetBool("isFadeIn", true);
       StartCoroutine(SelectSceneLoad());
    }

    private IEnumerator SelectSceneLoad()
    {
        yield return new WaitForSeconds(gameConstants.FadeWaitTime);
        SceneManager.LoadScene("StageSelectScene");
    }

    private IEnumerator TitleSceneLoad()
    {
        yield return new WaitForSeconds(gameConstants.FadeWaitTime);
        SceneManager.LoadScene("TitleScene");
    }
}

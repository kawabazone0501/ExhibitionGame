using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerCountDown : MonoBehaviour
{
    private Animator FadePanel;
    private Animator GameOverPanel;

    public GameConstants GameConstants;

    public Text timerText;
    private float timeRemaining;//ÉQÅ[ÉÄÇÃécÇËéûä‘

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null in Start.");
        }
        else
        {
            Debug.Log("GameManager instance is found in Start.");
        }
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
        timeRemaining = GameConstants.TotalTime;
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
            Invoke("ButtonChoice", 2.0f); ;
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ButtonChoice()
    {
        GameOverPanel.SetBool("isChoice", true);
    }

    public void TitleButton()
    {
        if (GameManager.Instance.GetAnimationController().MaxObjectsSpawn == 1)
        {
            PlayerPrefs.SetInt("Score_1", GameManager.Instance.GetGaugeController().Score);
            PlayerPrefs.Save();
        }
        else if(GameManager.Instance.GetAnimationController().MaxObjectsSpawn == 2)
        {
            PlayerPrefs.SetInt("Score_2", GameManager.Instance.GetGaugeController().Score);
            PlayerPrefs.Save();
        }
        else if(GameManager.Instance.GetAnimationController().MaxObjectsSpawn == 3)
        {
            PlayerPrefs.SetInt("Score_3", GameManager.Instance.GetGaugeController().Score);
            PlayerPrefs.Save();
        }
       FadePanel.SetBool("isFadeOut", true);

        Invoke("TitleScene", 3.0f);
    }
    public void SelectButton()
    {
        FadePanel.SetBool("isFadeOut", true);
        Invoke("StageSelectScene", 3.0f);
    }

    public void SelectScene()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

    public void TitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}

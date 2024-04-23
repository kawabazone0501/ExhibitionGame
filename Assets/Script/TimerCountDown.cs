using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerCountDown : MonoBehaviour
{
    Animator Fade_animator;
    public GameObject FadeObj;
    Animator Over_animator;
    public GameObject OverObj;

    public AnimationController a_Controller;
    public GaugeController g_Conttroller;

    public Text timerText;
    public float totalTime = 180.0f; // 3 minutes in seconds
    private float timeRemaining;

    private void Awake()
    {
        Fade_animator = FadeObj.GetComponent<Animator>();
        Fade_animator.SetBool("isFadeOut", true);
    }

    void Start()
    {
        Fade_animator = FadeObj.GetComponent<Animator>();
        Over_animator = OverObj.GetComponent<Animator>();
        timeRemaining = totalTime;
        UpdateTimerDisplay();
        InvokeRepeating("UpdateTimer", 1.0f, 1.0f);
        Fade_animator.SetBool("isFadeOut",true);
        
    }

    void UpdateTimer()
    {
        Fade_animator.SetBool("isFadeOut", false);
        if (timeRemaining > 0)
        {
            timeRemaining -= 1.0f;
            UpdateTimerDisplay();
        }
        else
        {
            // Timer has reached zero
            CancelInvoke("UpdateTimer");
            Over_animator.SetBool("isOver", true);
            Invoke("ButtonChoise", 2.0f); ;
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ButtonChoise()
    {
        Over_animator.SetBool("isChoise", true);
    }

    public void TitleButton()
    {
        if (a_Controller.maxObjectsToSpawn == 1)
        {
            PlayerPrefs.SetInt("Score_1", g_Conttroller.Score);
            PlayerPrefs.Save();
        }
        else if(a_Controller.maxObjectsToSpawn == 2)
        {
            PlayerPrefs.SetInt("Score_2", g_Conttroller.Score);
            PlayerPrefs.Save();
        }
        else if(a_Controller.maxObjectsToSpawn == 3)
        {
            PlayerPrefs.SetInt("Score_3", g_Conttroller.Score);
            PlayerPrefs.Save();
        }
       Fade_animator.SetBool("isFadeOut", true);

        Invoke("TitleScene", 3.0f);
    }
    public void SelectButton()
    {
        Fade_animator.SetBool("isFadeOut", true);
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

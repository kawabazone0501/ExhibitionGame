using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Tilemaps;

public class SelectStageController : MonoBehaviour
{
    [SerializeField] private GameConstants gameConstants;

    Animator Fade_animator;
    [SerializeField] private GameObject FadeObj;

    Animator animator;

    [SerializeField] private Text[] highScoreTexts;

    [SerializeField] private GameObject profileObj;

    [SerializeField] private Button[] CloseButtons;

    private int MaxSpawn;
    [SerializeField] private int[] season;
    public int[] Season => season;

    [SerializeField] private int[] maxScores;
    public int[] MaxScores => maxScores;
    
    private int profileCurrent;



    private void Awake()
    {
        Season[gameConstants.FirstScore] = PlayerPrefs.GetInt("Score_1", 0);
        Season[gameConstants.SecondScore] = PlayerPrefs.GetInt("Score_2", 0);
        Season[gameConstants.ThirdScore] = PlayerPrefs.GetInt("Score_3", 0);
        
        Fade_animator = FadeObj.GetComponent<Animator>();
        Fade_animator.SetBool("isFadeOut", true);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = profileObj.GetComponent<Animator>();

        if (Season[gameConstants.FirstScore] > MaxScores[gameConstants.FirstScore])
        {
            MaxScores[gameConstants.FirstScore] = Season[gameConstants.FirstScore];
            PlayerPrefs.SetInt("HighScore_1", MaxScores[gameConstants.FirstScore]);
            PlayerPrefs.Save();
        }
        if (Season[gameConstants.SecondScore] > MaxScores[gameConstants.SecondScore])
        {
            MaxScores[gameConstants.SecondScore] = Season[gameConstants.SecondScore];
            PlayerPrefs.SetInt("HighScore_2", MaxScores[gameConstants.SecondScore]);
            PlayerPrefs.Save();
        }
        if (Season[gameConstants.ThirdScore] > MaxScores[gameConstants.ThirdScore])
        {
            MaxScores[gameConstants.ThirdScore] = Season[gameConstants.ThirdScore];
            PlayerPrefs.SetInt("HighScore_3", MaxScores[gameConstants.ThirdScore]);
            PlayerPrefs.Save();
        }

        UpdateHighScoreText();
    }
    void UpdateHighScoreText()
    {
        for (int i = 0; i < highScoreTexts.Length; i++)
        {
            highScoreTexts[i].text = "High Score: " + MaxScores[i].ToString("N0");
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void First_Season()
    {
        Fade_animator.SetBool("isFadeIn", true);
        MaxSpawn = gameConstants.FirstScore;
        PlayerPrefs.SetInt("isMax", MaxSpawn);
        PlayerPrefs.Save();
        Invoke("LoadGameScene", gameConstants.FadeWaitTime);
    }

    public void Second_Season()
    {
        Fade_animator.SetBool("isFadeIn", true);
        MaxSpawn = gameConstants.SecondScore;
        PlayerPrefs.SetInt("isMax", MaxSpawn);
        PlayerPrefs.Save();
        Invoke("LoadGameScene", gameConstants.FadeWaitTime);
    }

    public void Third_Season()
    {
        Fade_animator.SetBool("isFadeIn", true);
        MaxSpawn = gameConstants.ThirdScore;
        PlayerPrefs.SetInt("isMax", MaxSpawn);
        PlayerPrefs.Save();
        Invoke("LoadGameScene", gameConstants.FadeWaitTime);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void RedCall()
    {
        animator.SetBool("isRed", true);
        profileCurrent = gameConstants.StudentRED;
        Invoke("OpenProfile", gameConstants.ProfileDisplayWaitingTime);
    }
    public void PurpleCall()
    {
        animator.SetBool("isPurple", true);
        profileCurrent = gameConstants.StudentPURPLE;
        Invoke("OpenProfile", gameConstants.ProfileDisplayWaitingTime);
    }
    public void WhiteCall()
    {
        animator.SetBool("isWhite", true);
        profileCurrent = gameConstants.StudentWHITE;
        Invoke("OpenProfile", gameConstants.ProfileDisplayWaitingTime);
    }
    public void TeacherCall()
    {
        animator.SetBool("isKobayashi", true);
        profileCurrent = gameConstants.Teacher;
        Invoke("OpenProfile", gameConstants.ProfileDisplayWaitingTime);
    }
    public void OpenProfile()
    {
        CloseButtons[profileCurrent].gameObject.SetActive(true);
    }
    public void CloseProfile()
    {
        CloseButtons[profileCurrent].gameObject.SetActive(false);
        animator.SetBool("isRed", false);
        animator.SetBool("isPurple", false);
        animator.SetBool("isWhite", false);
        animator.SetBool("isKobayashi", false);
    }
}


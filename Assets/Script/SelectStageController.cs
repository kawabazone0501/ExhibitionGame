using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SelectStageController : MonoBehaviour
{
    public AudioSource audioSource;

    Animator Fade_animator;
    public GameObject FadeObj;

    Animator animator;

    public Text[] highScoreTexts;

    public GameObject profileObj;

    public Button[] CloseButtons;

    public int MaxSpawn;

    public int Score_first;
    public int Score_second;
    public int Score_third;

    private int Profile_current;

    public int[] MaxScores;




    private void Awake()
    {
        Score_first = PlayerPrefs.GetInt("Score_1", 0);
        Score_second = PlayerPrefs.GetInt("Score_2", 0);
        Score_third = PlayerPrefs.GetInt("Score_3", 0);
        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();
        Fade_animator = FadeObj.GetComponent<Animator>();
        Fade_animator.SetBool("isFadeOut", true);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = profileObj.GetComponent<Animator>();
        audioSource.loop = true;
        // オーディオを再生する
        audioSource.Play();

        if (Score_first > MaxScores[0])
        {
            MaxScores[0] = Score_first;
            PlayerPrefs.SetInt("HighScore_1", MaxScores[0]);
            PlayerPrefs.Save();
        }
        if (Score_second > MaxScores[1])
        {
            MaxScores[1] = Score_second;
            PlayerPrefs.SetInt("HighScore_2", MaxScores[1]);
            PlayerPrefs.Save();
        }
        if (Score_third > MaxScores[2])
        {
            MaxScores[2] = Score_third;
            PlayerPrefs.SetInt("HighScore_3", MaxScores[2]);
            PlayerPrefs.Save();
        }

        UpdateHighScoreText();
    }
    void UpdateHighScoreText()
    {
        for (int i = 0; i < 3; i++)
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
        MaxSpawn = 1;
        PlayerPrefs.SetInt("isMax", MaxSpawn);
        PlayerPrefs.Save();
        Invoke("LoadGameScene", 3.0f);
    }

    public void Second_Season()
    {
        Fade_animator.SetBool("isFadeIn", true);
        MaxSpawn = 2;
        PlayerPrefs.SetInt("isMax", MaxSpawn);
        PlayerPrefs.Save();
        Invoke("LoadGameScene", 3.0f);
    }

    public void Third_Season()
    {
        Fade_animator.SetBool("isFadeIn", true);
        MaxSpawn = 3;
        PlayerPrefs.SetInt("isMax", MaxSpawn);
        PlayerPrefs.Save();
        Invoke("LoadGameScene", 3.0f);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Red_Call()
    {
        animator.SetBool("isRed", true);
        Profile_current = 0;
        Invoke("Open_profile", 1.0f);
    }
    public void Purple_Call()
    {
        animator.SetBool("isPurple", true);
        Profile_current = 1;
        Invoke("Open_profile", 1.0f);
    }
    public void White_Call()
    {
        animator.SetBool("isWhite", true);
        Profile_current = 2;
        Invoke("Open_profile", 1.0f);
    }
    public void Kobayashi_Call()
    {
        animator.SetBool("isKobayashi", true);
        Profile_current = 3;
        Invoke("Open_profile", 1.0f);
    }
    public void Open_profile()
    {
        CloseButtons[Profile_current].gameObject.SetActive(true);
    }
    public void Close_profile()
    {
        CloseButtons[Profile_current].gameObject.SetActive(false);
        animator.SetBool("isRed", false);
        animator.SetBool("isPurple", false);
        animator.SetBool("isWhite", false);
        animator.SetBool("isKobayashi", false);
    }
}


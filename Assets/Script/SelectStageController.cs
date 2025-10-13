using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        Season[gameConstants.FirstSeason - 1] = PlayerPrefs.GetInt("Score_1", 0);
        Season[gameConstants.SecondSeason - 1] = PlayerPrefs.GetInt("Score_2", 0);
        Season[gameConstants.ThirdSeason - 1] = PlayerPrefs.GetInt("Score_3", 0);

        Fade_animator = FadeObj.GetComponent<Animator>();
        Fade_animator.SetBool("isFadeOut", true);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = profileObj.GetComponent<Animator>();

        if (Season[gameConstants.FirstSeason - 1] > MaxScores[gameConstants.FirstSeason - 1])
        {
            Debug.Log(Season[gameConstants.FirstSeason - 1]);
            MaxScores[gameConstants.FirstSeason - 1] = Season[gameConstants.FirstSeason - 1];
            PlayerPrefs.SetInt("HighScore_1", MaxScores[gameConstants.FirstSeason - 1]);
            PlayerPrefs.Save();
        }
        if (Season[gameConstants.SecondSeason - 1] > MaxScores[gameConstants.SecondSeason - 1])
        {
            Debug.Log(Season[gameConstants.SecondSeason - 1]);
            MaxScores[gameConstants.SecondSeason - 1] = Season[gameConstants.SecondSeason - 1];
            PlayerPrefs.SetInt("HighScore_2", MaxScores[gameConstants.SecondSeason - 1]);
            PlayerPrefs.Save();
        }
        if (Season[gameConstants.ThirdSeason - 1] > MaxScores[gameConstants.ThirdSeason - 1])
        {
            Debug.Log(Season[gameConstants.ThirdSeason - 1]);
            MaxScores[gameConstants.ThirdSeason - 1] = Season[gameConstants.ThirdSeason - 1];
            PlayerPrefs.SetInt("HighScore_3", MaxScores[gameConstants.ThirdSeason - 1]);
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
    
    public void First_Season()
    {
        /*Fade_animator.SetBool("isFadeIn", true);
        MaxSpawn = gameConstants.FirstSeason;
        PlayerPrefs.SetInt("isMax", MaxSpawn);
        Debug.Log(MaxSpawn);
        PlayerPrefs.Save();
        Invoke("LoadGameScene", gameConstants.FadeWaitTime);*/
        SeasonChoice(gameConstants.FirstSpawn);

    }
    public void Second_Season()
    {
        /*Fade_animator.SetBool("isFadeIn", true);
        MaxSpawn = gameConstants.SecondSeason;
        PlayerPrefs.SetInt("isMax", MaxSpawn);
        Debug.Log(MaxSpawn);
        PlayerPrefs.Save();
        Invoke("LoadGameScene", gameConstants.FadeWaitTime);*/
        SeasonChoice(gameConstants.SecondSpawn);

    }

    public void Third_Season()
    {
        /*  //Fade_animator.SetBool("isFadeIn", true);
          //MaxSpawn = gameConstants.ThirdSeason;
          //PlayerPrefs.SetInt("isMax", MaxSpawn);
          //Debug.Log(MaxSpawn);
          //PlayerPrefs.Save();
          //Invoke("LoadGameScene", gameConstants.FadeWaitTime);*/
        SeasonChoice(gameConstants.ThirdSpawn);
    }

    public void SeasonChoice(int maxSpawn)
    {
        Fade_animator.SetBool("isFadeIn", true);
        MaxSpawn = maxSpawn;
        PlayerPrefs.SetInt("isMax", MaxSpawn);
        Debug.Log(MaxSpawn);
        PlayerPrefs.Save();
        Invoke("LoadGameScene", gameConstants.FadeWaitTime);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void RedCall()
    {
        /*animator.SetBool("isRed", true);
        profileCurrent = gameConstants.StudentRED;
        Invoke("OpenProfile", gameConstants.ProfileDisplayWaitingTime);*/

        ProfileCall("isRed", gameConstants.StudentRED);
    }
    public void PurpleCall()
    {
        /*animator.SetBool("isPurple", true);
        profileCurrent = gameConstants.StudentPURPLE;
        Invoke("OpenProfile", gameConstants.ProfileDisplayWaitingTime);*/

        ProfileCall("isPurple", gameConstants.StudentPURPLE);
    }
    public void WhiteCall()
    {
        /*animator.SetBool("isWhite", true);
        profileCurrent = gameConstants.StudentWHITE;
        Invoke("OpenProfile", gameConstants.ProfileDisplayWaitingTime);*/

        ProfileCall("isWhite", gameConstants.StudentWHITE);
    }
    public void TeacherCall()
    {
        /*animator.SetBool("isTeacher", true);
        profileCurrent = gameConstants.Teacher;
        Invoke("OpenProfile", gameConstants.ProfileDisplayWaitingTime);*/

        ProfileCall("isTeacher", gameConstants.Teacher);
    }


    public void ProfileCall(string animationName,int ProfileCurrent)
    {
        animator.SetBool(animationName, true);
        profileCurrent = ProfileCurrent;
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
        animator.SetBool("isTeacher", false);
    }
}


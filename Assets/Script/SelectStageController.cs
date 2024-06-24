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
        Season[gameConstants.FirstSeason] = PlayerPrefs.GetInt("Score_1", 0);
        Season[gameConstants.SecondSeason] = PlayerPrefs.GetInt("Score_2", 0);
        Season[gameConstants.ThirdSeason] = PlayerPrefs.GetInt("Score_3", 0);
        
        Fade_animator = FadeObj.GetComponent<Animator>();
        Fade_animator.SetBool("isFadeOut", true);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = profileObj.GetComponent<Animator>();

        if (Season[gameConstants.FirstSeason] > MaxScores[gameConstants.FirstSeason])
        {
            MaxScores[gameConstants.FirstSeason] = Season[gameConstants.FirstSeason];
            PlayerPrefs.SetInt("HighScore_1", MaxScores[gameConstants.FirstSeason]);
            PlayerPrefs.Save();
        }
        if (Season[gameConstants.SecondSeason] > MaxScores[gameConstants.SecondSeason])
        {
            MaxScores[gameConstants.SecondSeason] = Season[gameConstants.SecondSeason];
            PlayerPrefs.SetInt("HighScore_2", MaxScores[gameConstants.SecondSeason]);
            PlayerPrefs.Save();
        }
        if (Season[gameConstants.ThirdSeason] > MaxScores[gameConstants.ThirdSeason])
        {
            MaxScores[gameConstants.ThirdSeason] = Season[gameConstants.ThirdSeason];
            PlayerPrefs.SetInt("HighScore_3", MaxScores[gameConstants.ThirdSeason]);
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
        MaxSpawn = gameConstants.FirstSeason;
        PlayerPrefs.SetInt("isMax", MaxSpawn);
        Debug.Log(MaxSpawn);
        PlayerPrefs.Save();
        Invoke("LoadGameScene", gameConstants.FadeWaitTime);
    }

    public void Second_Season()
    {
        Fade_animator.SetBool("isFadeIn", true);
        MaxSpawn = gameConstants.SecondSeason;
        PlayerPrefs.SetInt("isMax", MaxSpawn);
        Debug.Log(MaxSpawn);
        PlayerPrefs.Save();
        Invoke("LoadGameScene", gameConstants.FadeWaitTime);
    }

    public void Third_Season()
    {
        Fade_animator.SetBool("isFadeIn", true);
        MaxSpawn = gameConstants.ThirdSeason;
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
        animator.SetBool("isTeacher", true);
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
        animator.SetBool("isTeacher", false);
    }
}


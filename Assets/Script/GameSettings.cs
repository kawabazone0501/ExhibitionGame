using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private float gaugeFillAmountThreshold;                        // ゲージが満タンになったかを確認するための定数
    private float gaugeFillAmountThresholdFull;                     // ゲージの量が満タンに達しているかを確認する定数(達してなければゲージを増やす)
    private float gaugeFillAmountThresholdReset;                    // ゲージ量をリセットする定数
    private int bonusThresholdTime;                  // ボーナススコアを得られるタイムかを図るしきい値
    private int baseScore;                         // ゲームクリア時の基礎スコア
    private int redArrivalScoreMultiplier;           // 赤の生徒を対応した際のスコア
    private int purpleArrivalScoreMultiplier;        // 紫の生徒を対応した際のスコア
    private int whiteArrivalScoreMultiplier;         // 白の生徒のを対応した際のスコア
    private int bonusBaseScore;                    // ボーナススコア
    private float totalTime;                      //制限時間　３分
    //------------------------------------------------------------------------------------------------
    // Player関連の定数
    private float increaseAmount;                                  // ゲージの増加量
    private float decreaseRateDecreaseAmount;                       // 毎秒の減少率を減らす量
    //-------------------------------------------------------------------------------------------------
    // 赤の生徒関連の定数
    private float redIncreaseAmount;                                // ゲージの増加量
    private float redCardFillAmountIncrement;                        //レッドカードのゲージの増加量
    //-------------------------------------------------------------------------------------------------
    // 紫の生徒関連の定数
    private float maxStickDistance;                                   // スティックが動くことができる最大距離
    private float rotationThreshold;                                 // 1周とみなす回転量の閾値
    private float purpleIncreaseAmount;                              // ゲージの増加量
    //------------------------------------------------------------------------------------------------
    // 白の生徒関連の定数
    private float gaugeIncreaseRate;                                 // ゲージの増加速度
    //--------------------------------------------------------------------------------------------------
    // 難易度ごとの背景のオブジェクトを保存する定数
    private int FirstSeasonBackGround;                                         //前期ステージの背景
    private int FirstSeasonCounter;                                         //前期ステージのカウンター
    private int SecondSeasonBackGround;                                         //夏休みステージの背景
    private int SecondSeasonCounter;                                         //夏休みステージのカウンター
    private int ThirdSeasonBackGround;                                         //後期ステージの背景
    private int ThirdSeasonCounter;                                         //後期ステージのカウンター
    //--------------------------------------------------------------------------------------------------
    // 3人の生徒の配列の定数
    private int studentRED;
    private int studentPURPLE;
    private int studentWHITE;
    // 先生の定数
    private int teacher;
    //--------------------------------------------------------------------------------------------------
    // ボタンや画像の非表示にする範囲を指定する(常に表示するボタンやUIがあるため)
    private int neverDisplayImage;
    private int neverDisplayButton;
    private int endDisplayImage;
    private int endDisplayButton;

    private int startDisplayImageRed;
    private int endDisplayImageRed;
    private int startDisplayButtonRed;
    private int endDisplayButtonRed;

    private int startDisplayImagePurple;
    private int endDisplayImagePurple;

    private int startDisplayImageWhite;
    private int endDisplayImageWhite;

    private int displayButtonWhite;

    private int fadePanel;
    //--------------------------------------------------------------------------------------------------

    // 難易度ごとの背景のオブジェクトの切り替えを行うための比較のための定数
    private int firstSeason;
    private int secondSeason;
    private int thirdSeason;
    //---------------------------------------------------------------------------------------------------
    // アニメーションの待ち時間(待ち時間の秒数だけ待った後に処理を行う)
    private float waitAnimationTime;
    private float profileDisplayWaitingTime;
    private float fadeWaitTime;
    private float buttonDisplayWaitingTime;
    private float exitWaitingTime;
    private float waitTimeIfNotPlayed;
    //---------------------------------------------------------------------------------------------------
    // ボタンやゲージの配列でFillAmountの値を変更するゲージや値を変動させるボタンを指定する定数
    private int playerGauge;
    private int redGauge;
    private int purpleGauge;
    private int whiteGauge;
    private int redCard;

    private int redButtonLeft;
    private int redButtonRight;
    private int redCardButton;
    //---------------------------------------------------------------------------------------------------
    // 赤の生徒のボタンの矢印のImageの切り替えのための定数
    private int redButtonLeftArrow;
    private int redButtonRightArrow;


    private void AllVariableAssignment()
    {
        gaugeFillAmountThreshold        =   GameConstants.Instance.GaugeFillAmountThreshold;                        
        gaugeFillAmountThresholdFull    =   GameConstants.Instance.GaugeFillAmountThresholdFull;                   
        bonusThresholdTime              =   GameConstants.Instance.BonusThresholdTime;                  
        baseScore                       =   GameConstants.Instance.BaseScore;                        
        redArrivalScoreMultiplier       =   GameConstants.Instance.RedArrivalScoreMultiplier;           
        purpleArrivalScoreMultiplier    =   GameConstants.Instance.PurpleArrivalScoreMultiplier;       
        whiteArrivalScoreMultiplier     =   GameConstants.Instance.WhiteArrivalScoreMultiplier;         
        bonusBaseScore                  =   GameConstants.Instance.BonusBaseScore;                    
        totalTime                       =   GameConstants.Instance.TotalTime;                     

        //increaseAmount                  =   GameConstants.Instance.IncreaseAmount;                                  
        //decreaseRateDecreaseAmount      =   GameConstants.Instance.DecreaseRateDecreaseAmount;                      

        redIncreaseAmount               =   GameConstants.Instance.RedIncreaseAmount;                                
        redCardFillAmountIncrement      =   GameConstants.Instance.RedCardFillAmountIncrement;                        
        maxStickDistance                =   GameConstants.Instance.MaxStickDistance;                                 
        rotationThreshold               =   GameConstants.Instance.RotationThreshold;                                 
        purpleIncreaseAmount            =   GameConstants.Instance.PurpleIncreaseAmount;                              
        gaugeIncreaseRate               =   GameConstants.Instance.GaugeIncreaseRate;                                
        FirstSeasonBackGround           =   GameConstants.Instance.OBJECT_A;                                         
        FirstSeasonCounter              =   GameConstants.Instance.OBJECT_B;                                         
        SecondSeasonBackGround          =   GameConstants.Instance.OBJECT_C;                                       
        SecondSeasonCounter             =   GameConstants.Instance.OBJECT_D;                                         
        ThirdSeasonBackGround           =   GameConstants.Instance.OBJECT_E;                                        
        ThirdSeasonCounter              =   GameConstants.Instance.OBJECT_F;                                         
        studentRED                      =   GameConstants.Instance.StudentRED;
        studentPURPLE                   =   GameConstants.Instance.StudentPURPLE;
        studentWHITE                    =   GameConstants.Instance.StudentWHITE;
        teacher                         =   GameConstants.Instance.Teacher;
        neverDisplayImage               =   GameConstants.Instance.NeverDisplayImage;
        neverDisplayButton              =   GameConstants.Instance.NeverDisplayButton;
        endDisplayImage                 =   GameConstants.Instance.EndDisplayImage;
        endDisplayButton                =   GameConstants.Instance.EndDisplayButton;

        startDisplayImageRed            =   GameConstants.Instance.StartDisplayImageRed;
        endDisplayImageRed              =   GameConstants.Instance.EndDisplayImageRed;
        startDisplayButtonRed           =   GameConstants.Instance.StartDisplayButtonRed;
        endDisplayButtonRed             =   GameConstants.Instance.EndDisplayButtonRed;

        startDisplayImagePurple         =   GameConstants.Instance.StartDisplayImagePurple;
        endDisplayImagePurple           =   GameConstants.Instance.EndDisplayImagePurple;

        startDisplayImageWhite          =   GameConstants.Instance.StartDisplayImageWhite;
        endDisplayImageWhite            =   GameConstants.Instance.EndDisplayImageWhite;

        displayButtonWhite              =   GameConstants.Instance.DisplayButtonWhite;

        fadePanel                       =   GameConstants.Instance.FadePanel;
        firstSeason                     =   GameConstants.Instance.FirstSeason;
        secondSeason                    =   GameConstants.Instance.SecondSeason;
        thirdSeason                     =   GameConstants.Instance.ThirdSeason;
        waitAnimationTime               =   GameConstants.Instance.WaitAnimationTime;
        profileDisplayWaitingTime       =   GameConstants.Instance.ProfileDisplayWaitingTime;
        fadeWaitTime                    =   GameConstants.Instance.FadeWaitTime;
        buttonDisplayWaitingTime        =   GameConstants.Instance.ButtonDisplayWaitingTime;
        exitWaitingTime                 =   GameConstants.Instance.ExitWaitingTime;
        waitTimeIfNotPlayed             =   GameConstants.Instance.WaitTimeIfNotPlayed;
        playerGauge                     =   GameConstants.Instance.PlayerGauge;
        redGauge                        =   GameConstants.Instance.RedGauge;
        purpleGauge                     =   GameConstants.Instance.PurpleGauge;
        whiteGauge                      =   GameConstants.Instance.WhiteGauge;
        redCard                         =   GameConstants.Instance.RedCard;

        redButtonLeft                   =   GameConstants.Instance.RedButtonLeft;
        redButtonRight                  =   GameConstants.Instance.RedButtonRight;
        redCardButton                   =   GameConstants.Instance.RedCardButton;
        redButtonLeftArrow              =   GameConstants.Instance.RedButtonLeftArrow;
        redButtonRightArrow             =   GameConstants.Instance.RedButtonRightArrow;

    }
    private static GameSettings _instance;
    public static GameSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameSettings>();
                if (_instance == null)
                {
                    Debug.LogError("GameSettings instance not found in the scene");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        AllVariableAssignment();
    }
}

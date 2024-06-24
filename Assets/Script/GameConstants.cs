using UnityEngine;

[CreateAssetMenu(fileName = "GameConstants", menuName = "Constants/GameConstants")]
public class GameConstants : ScriptableObject
{
    /*
        GameConstants
        
           ゲーム中、特定のタイミングでのみ使用される
            定数,bool値等を保存している。

            例：
                各生徒のゲージの増加量やゲームクリア時の基礎スコアやボーナススコア
     */
    //------------------------------------------------------------------------------------------------
    // ゲーム関連の定数
     private float gaugeFillAmountThreshold = 0.96f;                        // ゲージが満タンになったかを確認するための定数
     private float gaugeFillAmountThresholdFull = 1.0f;                     // ゲージの量が満タンに達しているかを確認する定数(達してなければゲージを増やす)
     private float gaugeFillAmountThresholdReset = 0.0f;                    // ゲージ量をリセットする定数
    [SerializeField] private int bonusThresholdTime = 120;                  // ボーナススコアを得られるタイムかを図るしきい値
    [SerializeField] private int baseScore = 18000;                         // ゲームクリア時の基礎スコア
    [SerializeField] private int redArrivalScoreMultiplier = 700;           // 赤の生徒を対応した際のスコア
    [SerializeField] private int purpleArrivalScoreMultiplier = 600;        // 紫の生徒を対応した際のスコア
    [SerializeField] private int whiteArrivalScoreMultiplier = 500;         // 白の生徒のを対応した際のスコア
    [SerializeField] private int bonusBaseScore = 30000;                    // ボーナススコア
    [SerializeField] private float totalTime = 180.0f;                      //制限時間　３分
    //------------------------------------------------------------------------------------------------
    // Player関連の定数
    private float increaseAmount = 0.025f;                                  // ゲージの増加量
    private float decreaseRateDecreaseAmount = 0.01f;                       // 毎秒の減少率を減らす量
    //-------------------------------------------------------------------------------------------------
    // 赤の生徒関連の定数
    private float redIncreaseAmount = 0.05f;                                // ゲージの増加量
    private float redCardFillAmountIncrement = 0.1f;                        //レッドカードのゲージの増加量
    //-------------------------------------------------------------------------------------------------
    // 紫の生徒関連の定数
    private float maxStickDistance = 75f;                                   // スティックが動くことができる最大距離
    private float rotationThreshold = 360f;                                 // 1周とみなす回転量の閾値
    private float purpleIncreaseAmount = 0.1f;                              // ゲージの増加量
    //------------------------------------------------------------------------------------------------
    // 白の生徒関連の定数
    private float gaugeIncreaseRate = 0.5f;                                 // ゲージの増加速度
    //------------------------------------------------------------------------------------------------
    // 各種生徒のボタンで使う bool値
    // Playerのボタン
    private bool isButtonClicked = false;                                   // ボタンがクリックされたかどうかのフラグ
    // 赤の生徒のボタン
    private bool isButton1Enabled = true;                                   // ボタン1が有効かどうか
    private bool isButton2Enabled = false;                                  // ボタン2が有効かどうか
    // 白の生徒のボタン
    private bool isButtonPressed = false;                                   // ボタンが押されているかどうかを示すフラグ
    // レッドカード 
    private bool isRedCard = false;                                         // レッドカードが押せるかどうかを判断するフラグ
    //--------------------------------------------------------------------------------------------------
    // 生徒の出現、退場に関連する bool値とゲームクリアの bool値
    private bool[] isStudents = new bool[3];                                // 3人の生徒がそれぞれ出現しているかの個別のフラグ
    private bool isStudentLock = false;                                     // ↑の boolが変更される前にロックをかけ、確実に trueになったのを判断する
    private bool isObjectAllowed = false;                                   // 生徒が出現したことを示すフラグ
    private bool isClear = false;                                           // ゲームクリアの条件を満たしたかを判断するフラグ
    //--------------------------------------------------------------------------------------------------
    // 難易度ごとの背景のオブジェクトを保存する定数
    private const int object_A = 0;                                         //前期ステージの背景
    private const int object_B = 1;                                         //前期ステージのカウンター
    private const int object_C = 2;                                         //夏休みステージの背景
    private const int object_D = 3;                                         //夏休みステージのカウンター
    private const int object_E = 4;                                         //後期ステージの背景
    private const int object_F = 5;                                         //後期ステージのカウンター
    //--------------------------------------------------------------------------------------------------
    // 3人の生徒の配列の定数
    private const int studentRED = 0;
    private const int studentPURPLE = 1;
    private const int studentWHITE = 2;
    // 先生の定数
    private const int teacher = 3;
    //--------------------------------------------------------------------------------------------------
    // ボタンや画像の非表示にする範囲を指定する(常に表示するボタンやUIがあるため)
    private const int neverDisplayImage = 1;
    private const int neverDisplayButton = 2;
    private const int endDisplayImage = 11;
    private const int endDisplayButton = 11;

    private const int startDisplayImageRed = 2;
    private const int endDisplayImageRed = 4;
    private const int startDisplayButtonRed = 2;
    private const int endDisplayButtonRed = 3;

    private const int startDisplayImagePurple = 5;
    private const int endDisplayImagePurple = 9;

    private const int startDisplayImageWhite = 10;
    private const int endDisplayImageWhite = 11;
    
    private const int displayButtonWhite = 5;

    private const int fadePanel = 13;
    //--------------------------------------------------------------------------------------------------

    // 難易度ごとの背景のオブジェクトの切り替えを行うための比較のための定数
    private const int firstSeason = 0;
    private const int secondSeason = 1;
    private const int thirdSeason = 2;
    //--------------------------------------------------------------------------------------------------
    // 再生するアニメーションクリップ
    [SerializeField] private AnimationClip[] animationClips;

    //---------------------------------------------------------------------------------------------------
    // アニメーションの待ち時間(待ち時間の秒数だけ待った後に処理を行う)
    private const float waitAnimationTime = 0.5f;
    private const float profileDisplayWaitingTime = 1.0f;
    private const float fadeWaitTime = 3.0f;
    private const float buttonDisplayWaitingTime = 2.0f;
    private const float exitWaitingTime = 5.0f;
    private const float waitTimeIfNotPlayed = 5.0f;
    //---------------------------------------------------------------------------------------------------
    // ボタンやゲージの配列でFillAmountの値を変更するゲージや値を変動させるボタンを指定する定数
    private const int playerGauge = 0;
    private const int redGauge = 4;
    private const int purpleGauge = 6;
    private const int whiteGauge = 10;
    private const int redCard = 12;

    private const int redButtonLeft = 2;
    private const int redButtonRight = 3;
    private const int redCardButton = 4;
    //---------------------------------------------------------------------------------------------------
    // 赤の生徒のボタンの矢印のImageの切り替えのための定数
    private const int redButtonLeftArrow = 1;
    private const int redButtonRightArrow = 2;
    private const int greenGauge = 5;
    //---------------------------------------------------------------------------------------------------

    
    // プロパティを参照して読み込む
    public float GaugeFillAmountThreshold => gaugeFillAmountThreshold;
    public float GaugeFillAmountThresholdFull => gaugeFillAmountThresholdFull;
    public float GaugeFillAmountThresholdReset => gaugeFillAmountThresholdReset;
    public int BonusThresholdTime => bonusThresholdTime;                 
    public int BaseScore => baseScore;
    public int RedArrivalScoreMultiplier => redArrivalScoreMultiplier;
    public int PurpleArrivalScoreMultiplier => purpleArrivalScoreMultiplier;
    public int WhiteArrivalScoreMultiplier => whiteArrivalScoreMultiplier;
    public int BonusBaseScore => bonusBaseScore;
    public float TotalTime => totalTime;
    //---------------------------------------------------------------------------------------------------
    public float IncreaseAmount => increaseAmount;
    public float DecreaseRateDecreaseAmount => decreaseRateDecreaseAmount;
    //---------------------------------------------------------------------------------------------------
    public float RedIncreaseAmount => redIncreaseAmount;
    public float RedCardFillAmountIncrement => redCardFillAmountIncrement;
    //---------------------------------------------------------------------------------------------------
    public float MaxStickDistance => maxStickDistance;
    public float RotationThreshold => rotationThreshold;
    public float PurpleIncreaseAmount => purpleIncreaseAmount;
    //---------------------------------------------------------------------------------------------------
    public float GaugeIncreaseRate => gaugeIncreaseRate;
    //---------------------------------------------------------------------------------------------------
    public int OBJECT_A => object_A;
    public int OBJECT_B => object_B;
    public int OBJECT_C => object_C;
    public int OBJECT_D => object_D;
    public int OBJECT_E => object_E;
    public int OBJECT_F => object_F;
    //---------------------------------------------------------------------------------------------------
    public int StudentRED => studentRED;
    public int StudentPURPLE => studentPURPLE;
    public int StudentWHITE => studentWHITE;
    public int Teacher => teacher;
    //---------------------------------------------------------------------------------------------------
    public int NeverDisplayImage => neverDisplayImage;
    public int NeverDisplayButton => neverDisplayButton;
    public int EndDisplayImage => endDisplayImage;
    public int EndDisplayButton => endDisplayButton;
    //---------------------------------------------------------------------------------------------------
    public int FirstSeason => firstSeason;
    public int SecondSeason => secondSeason;
    public int ThirdSeason => thirdSeason;
    //---------------------------------------------------------------------------------------------------
    public AnimationClip[] AnimationClips => animationClips;
    //---------------------------------------------------------------------------------------------------
    public float WaitAnimationTime => waitAnimationTime;
    public float ProfileDisplayWaitingTime => profileDisplayWaitingTime;
    public float FadeWaitTime => fadeWaitTime;
    public float ButtonDisplayWaitingTime => buttonDisplayWaitingTime;
    public float ExitWaitingTime => exitWaitingTime;
    public float WaitTimeIfNotPlayed => waitTimeIfNotPlayed;
    //---------------------------------------------------------------------------------------------------
    public int StartDisplayImageRed => startDisplayImageRed;
    public int EndDisplayImageRed => endDisplayImageRed;
    public int StartDisplayButtonRed => startDisplayButtonRed;
    public int EndDisplayButtonRed => endDisplayButtonRed;
    public int StartDisplayImagePurple => startDisplayImagePurple;
    public int EndDisplayImagePurple => endDisplayImagePurple;
    public int StartDisplayImageWhite => startDisplayImageWhite;
    public int EndDisplayImageWhite => endDisplayImageWhite;
    public int DisplayButtonWhite => displayButtonWhite;
    //---------------------------------------------------------------------------------------------------
    public int PlayerGauge => playerGauge;
    public int RedGauge => redGauge;
    public int PurpleGauge => purpleGauge;
    public int WhiteGauge => whiteGauge;
    public int RedCard => redCard;
    public int RedButtonLeft => redButtonLeft;
    public  int RedButtonRight => redButtonRight;
    public  int RedCardButton => redCardButton;
    public int FadePanel => fadePanel;
    //---------------------------------------------------------------------------------------------------
    public int RedButtonLeftArrow=> redButtonLeftArrow;
    public int RedButtonRightArrow => redButtonRightArrow;
    //---------------------------------------------------------------------------------------------------
    public bool IsButtonClicked
    {
        get => isButtonClicked;
        set => isButtonClicked = value;
    }

    public bool IsButton1Enabled
    {
        get => isButton1Enabled;
        set => isButton1Enabled = value;
    }

    public bool IsButton2Enabled
    {
        get => isButton2Enabled;
        set => isButton2Enabled = value;
    }

    public bool IsButtonPressed
    {
        get => isButtonPressed;
        set => isButtonPressed = value;
    }

    public bool IsRedCard
    {
        get => isRedCard;
        set => isRedCard = value;
    }

    public bool[] IsStudents => isStudents;

    

    public bool IsStudentLock
    {
        get => isStudentLock;
        set => isStudentLock = value;
    }

    public bool IsObjectAllowed
    {
        get => isObjectAllowed;
        set => isObjectAllowed = value;
    }

    public bool IsClear
    {
        get => isClear;
        set => isClear = value;
    }
   
}

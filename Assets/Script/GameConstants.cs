using UnityEngine;

[CreateAssetMenu(fileName = "GameConstants", menuName = "Constants/GameConstants")]
public class GameConstants : ScriptableObject
{
    /*
        GameConstants
        
           �Q�[�����A����̃^�C�~���O�ł̂ݎg�p�����
            �萔,bool�l����ۑ����Ă���B

            ��F
                �e���k�̃Q�[�W�̑����ʂ�Q�[���N���A���̊�b�X�R�A��{�[�i�X�X�R�A
     */
    //------------------------------------------------------------------------------------------------
    // �Q�[���֘A�̒萔
     private float gaugeFillAmountThreshold = 0.96f;                        // �Q�[�W�����^���ɂȂ��������m�F���邽�߂̒萔
     private float gaugeFillAmountThresholdFull = 1.0f;                     // �Q�[�W�̗ʂ����^���ɒB���Ă��邩���m�F����萔(�B���ĂȂ���΃Q�[�W�𑝂₷)
     private float gaugeFillAmountThresholdReset = 0.0f;                    // �Q�[�W�ʂ����Z�b�g����萔
    [SerializeField] private int bonusThresholdTime = 120;                  // �{�[�i�X�X�R�A�𓾂���^�C������}�邵�����l
    [SerializeField] private int baseScore = 18000;                         // �Q�[���N���A���̊�b�X�R�A
    [SerializeField] private int redArrivalScoreMultiplier = 700;           // �Ԃ̐��k��Ή������ۂ̃X�R�A
    [SerializeField] private int purpleArrivalScoreMultiplier = 600;        // ���̐��k��Ή������ۂ̃X�R�A
    [SerializeField] private int whiteArrivalScoreMultiplier = 500;         // ���̐��k�̂�Ή������ۂ̃X�R�A
    [SerializeField] private int bonusBaseScore = 30000;                    // �{�[�i�X�X�R�A
    [SerializeField] private float totalTime = 180.0f;                      //�������ԁ@�R��
    //------------------------------------------------------------------------------------------------
    // Player�֘A�̒萔
    private float increaseAmount = 0.025f;                                  // �Q�[�W�̑�����
    private float decreaseRateDecreaseAmount = 0.01f;                       // ���b�̌����������炷��
    //-------------------------------------------------------------------------------------------------
    // �Ԃ̐��k�֘A�̒萔
    private float redIncreaseAmount = 0.05f;                                // �Q�[�W�̑�����
    private float redCardFillAmountIncrement = 0.1f;                        //���b�h�J�[�h�̃Q�[�W�̑�����
    //-------------------------------------------------------------------------------------------------
    // ���̐��k�֘A�̒萔
    private float maxStickDistance = 75f;                                   // �X�e�B�b�N���������Ƃ��ł���ő勗��
    private float rotationThreshold = 360f;                                 // 1���Ƃ݂Ȃ���]�ʂ�臒l
    private float purpleIncreaseAmount = 0.1f;                              // �Q�[�W�̑�����
    //------------------------------------------------------------------------------------------------
    // ���̐��k�֘A�̒萔
    private float gaugeIncreaseRate = 0.5f;                                 // �Q�[�W�̑������x
    //------------------------------------------------------------------------------------------------
    // �e�퐶�k�̃{�^���Ŏg�� bool�l
    // Player�̃{�^��
    private bool isButtonClicked = false;                                   // �{�^�����N���b�N���ꂽ���ǂ����̃t���O
    // �Ԃ̐��k�̃{�^��
    private bool isButton1Enabled = true;                                   // �{�^��1���L�����ǂ���
    private bool isButton2Enabled = false;                                  // �{�^��2���L�����ǂ���
    // ���̐��k�̃{�^��
    private bool isButtonPressed = false;                                   // �{�^����������Ă��邩�ǂ����������t���O
    // ���b�h�J�[�h 
    private bool isRedCard = false;                                         // ���b�h�J�[�h�������邩�ǂ����𔻒f����t���O
    //--------------------------------------------------------------------------------------------------
    // ���k�̏o���A�ޏ�Ɋ֘A���� bool�l�ƃQ�[���N���A�� bool�l
    private bool[] isStudents = new bool[3];                                // 3�l�̐��k�����ꂼ��o�����Ă��邩�̌ʂ̃t���O
    private bool isStudentLock = false;                                     // ���� bool���ύX�����O�Ƀ��b�N�������A�m���� true�ɂȂ����̂𔻒f����
    private bool isObjectAllowed = false;                                   // ���k���o���������Ƃ������t���O
    private bool isClear = false;                                           // �Q�[���N���A�̏����𖞂��������𔻒f����t���O
    //--------------------------------------------------------------------------------------------------
    // ��Փx���Ƃ̔w�i�̃I�u�W�F�N�g��ۑ�����萔
    private const int object_A = 0;                                         //�O���X�e�[�W�̔w�i
    private const int object_B = 1;                                         //�O���X�e�[�W�̃J�E���^�[
    private const int object_C = 2;                                         //�ċx�݃X�e�[�W�̔w�i
    private const int object_D = 3;                                         //�ċx�݃X�e�[�W�̃J�E���^�[
    private const int object_E = 4;                                         //����X�e�[�W�̔w�i
    private const int object_F = 5;                                         //����X�e�[�W�̃J�E���^�[
    //--------------------------------------------------------------------------------------------------
    // 3�l�̐��k�̔z��̒萔
    private const int studentRED = 0;
    private const int studentPURPLE = 1;
    private const int studentWHITE = 2;
    // �搶�̒萔
    private const int teacher = 3;
    //--------------------------------------------------------------------------------------------------
    // �{�^����摜�̔�\���ɂ���͈͂��w�肷��(��ɕ\������{�^����UI�����邽��)
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

    // ��Փx���Ƃ̔w�i�̃I�u�W�F�N�g�̐؂�ւ����s�����߂̔�r�̂��߂̒萔
    private const int firstSeason = 0;
    private const int secondSeason = 1;
    private const int thirdSeason = 2;
    //--------------------------------------------------------------------------------------------------
    // �Đ�����A�j���[�V�����N���b�v
    [SerializeField] private AnimationClip[] animationClips;

    //---------------------------------------------------------------------------------------------------
    // �A�j���[�V�����̑҂�����(�҂����Ԃ̕b�������҂�����ɏ������s��)
    private const float waitAnimationTime = 0.5f;
    private const float profileDisplayWaitingTime = 1.0f;
    private const float fadeWaitTime = 3.0f;
    private const float buttonDisplayWaitingTime = 2.0f;
    private const float exitWaitingTime = 5.0f;
    private const float waitTimeIfNotPlayed = 5.0f;
    //---------------------------------------------------------------------------------------------------
    // �{�^����Q�[�W�̔z���FillAmount�̒l��ύX����Q�[�W��l��ϓ�������{�^�����w�肷��萔
    private const int playerGauge = 0;
    private const int redGauge = 4;
    private const int purpleGauge = 6;
    private const int whiteGauge = 10;
    private const int redCard = 12;

    private const int redButtonLeft = 2;
    private const int redButtonRight = 3;
    private const int redCardButton = 4;
    //---------------------------------------------------------------------------------------------------
    // �Ԃ̐��k�̃{�^���̖���Image�̐؂�ւ��̂��߂̒萔
    private const int redButtonLeftArrow = 1;
    private const int redButtonRightArrow = 2;
    private const int greenGauge = 5;
    //---------------------------------------------------------------------------------------------------

    
    // �v���p�e�B���Q�Ƃ��ēǂݍ���
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

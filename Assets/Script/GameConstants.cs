using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    [SerializeField] private float gaugeFillAmountThreshold = 0.96f;        // �Q�[�W�����^���ɂȂ��������m�F���邽�߂̒萔
    [SerializeField] private float gaugeFillAmountThresholdFull = 1.0f;     // �Q�[�W�̗ʂ����^���ɒB���Ă��邩���m�F����萔(�B���ĂȂ���΃Q�[�W�𑝂₷)
    [SerializeField] private int bonusThresholdTime = 120;                  // �{�[�i�X�X�R�A�𓾂���^�C������}�邵�����l
    [SerializeField] private int baseScore = 18000;                         // �Q�[���N���A���̊�b�X�R�A
    [SerializeField] private int redArrivalScoreMultiplier = 700;           // �Ԃ̐��k��Ή������ۂ̃X�R�A
    [SerializeField] private int purpleArrivalScoreMultiplier = 600;        // ���̐��k��Ή������ۂ̃X�R�A
    [SerializeField] private int whiteArrivalScoreMultiplier = 500;         // ���̐��k�̂�Ή������ۂ̃X�R�A
    [SerializeField] private int bonusBaseScore = 30000;                    // �{�[�i�X�X�R�A
    [SerializeField] private float totalTime = 180.0f;                      //�������ԁ@�R��
    //------------------------------------------------------------------------------------------------
    // Player�֘A�̒萔
    [SerializeField] private float increaseAmount = 0.025f;                 // �Q�[�W�̑�����
    [SerializeField] private float decreaseRateDecreaseAmount = 0.01f;      // ���b�̌����������炷��
    //-------------------------------------------------------------------------------------------------
    // �Ԃ̐��k�֘A�̒萔
    [SerializeField] private float redIncreaseAmount = 0.05f;               // �Q�[�W�̑�����
    [SerializeField] private float redCardFillAmountIncrement = 0.1f;       //���b�h�J�[�h�̃Q�[�W�̑�����
    //-------------------------------------------------------------------------------------------------
    // ���̐��k�֘A�̒萔
    [SerializeField] private float maxStickDistance = 75f;                  // �X�e�B�b�N���������Ƃ��ł���ő勗��
    [SerializeField] private float rotationThreshold = 360f;                // 1���Ƃ݂Ȃ���]�ʂ�臒l
    [SerializeField] private float purpleIncreaseAmount = 0.1f;             // �Q�[�W�̑�����
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
    private bool isStudent = false;                                         // ���k�����邩�̃t���O(player�̃{�^���������邩���f����)
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
    private const int student_RED = 0;
    private const int student_PURPLE = 1;
    private const int student_WHITE = 2;

    // �v���p�e�B��ʂ��ĊO������萔��ǂݎ��
    public float GaugeFillAmountThreshold => gaugeFillAmountThreshold;
    public float GaugeFillAmountThresholdFull => gaugeFillAmountThresholdFull;
    public int BonusThresholdTime => bonusThresholdTime;
    public int BaseScore => baseScore;
    public int RedArrivalScoreMultiplier => redArrivalScoreMultiplier;
    public int PurpleArrivalScoreMultiplier => purpleArrivalScoreMultiplier;
    public int WhiteArrivalScoreMultiplier => whiteArrivalScoreMultiplier;
    public int BonusBaseScore => bonusBaseScore;
    public float TotalTime => totalTime;
    public float IncreaseAmount => increaseAmount;
    public float DecreaseRateDecreaseAmount => decreaseRateDecreaseAmount;
    public float RedIncreaseAmount => redIncreaseAmount;
    public float RedCardFillAmountIncrement => redCardFillAmountIncrement;
    public float MaxStickDistance => maxStickDistance;
    public float RotationThreshold => rotationThreshold;
    public float PurpleIncreaseAmount => purpleIncreaseAmount;
    public float GaugeIncreaseRate => gaugeIncreaseRate;
    public int OBJECT_A => object_A;
    public int OBJECT_B => object_B;
    public int OBJECT_C => object_C;
    public int OBJECT_D => object_D;
    public int OBJECT_E => object_E;
    public int OBJECT_F => object_F;
    public int Student_RED => student_RED;
    public int Student_PURPLE => student_PURPLE;
    public int Student_WHITE => student_WHITE;
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

    public bool IsStudent
    {
        get => isStudent;
        set => isStudent = value;
    }

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

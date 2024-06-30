using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    // �e�A�j���[�^�[���C���X�y�N�^�[����ݒ�ł���悤��[SerializeField]���g�p
    [SerializeField] private Animator seitoRed;
    [SerializeField] private Animator seitoPurple;
    [SerializeField] private Animator seitoWhite;
    [SerializeField] private Animator teacher;
    [SerializeField] private Animator phone;
    [SerializeField] private Animator fadePanel;
    [SerializeField] private Animator gameClearPanel;
    [SerializeField] private Animator gameOverPanel;
    [SerializeField] private Animator redGuide;
    [SerializeField] private Animator purpleGuide;
    [SerializeField] private Animator whiteGuide;
    [SerializeField] private Animator teacherGuide;

    // ���J�v���p�e�B���g�p���ăA�j���[�^�[�ւ̃A�N�Z�X���
    public Animator SeitoRed => seitoRed;
    public Animator SeitoPurple => seitoPurple;
    public Animator SeitoWhite => seitoWhite;
    public Animator Teacher => teacher;
    public Animator Phone => phone;
    public Animator FadePanel => fadePanel;
    public Animator GameClearPanel => gameClearPanel;
    public Animator GameOverPanel => gameOverPanel;
    public Animator RedGuide => redGuide;
    public Animator PurpleGuide => purpleGuide;
    public Animator WhiteGuide => whiteGuide;
    public Animator TeacherGuide => teacherGuide;

    // Awake���\�b�h�̓I�u�W�F�N�g���L���ɂȂ�Ƃ����ɌĂяo�����
    private void Awake()
    {

        // �����ǂꂩ�̃A�j���[�^�[���ݒ肳��Ă��Ȃ���΃G���[���b�Z�[�W��\��
        if (seitoRed        == null ||
            seitoPurple     == null ||
            seitoWhite      == null ||
            teacher         == null ||
            phone           == null ||
            fadePanel       == null ||
            gameClearPanel  == null ||
            gameOverPanel   == null ||
            redGuide        == null ||
            purpleGuide     == null ||
            whiteGuide      == null ||
           teacherGuide     == null )
        {
            Debug.LogError("One or more Animator references are missing.");
        }
    }
}

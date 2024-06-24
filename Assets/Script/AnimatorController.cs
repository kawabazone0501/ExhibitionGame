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

    // ���J�v���p�e�B���g�p���ăA�j���[�^�[�ւ̃A�N�Z�X���
    public Animator SeitoRed => seitoRed;
    public Animator SeitoPurple => seitoPurple;
    public Animator SeitoWhite => seitoWhite;
    public Animator Teacher => teacher;
    public Animator Phone => phone;
    public Animator FadePanel => fadePanel;
    public Animator GameClearPanel => gameClearPanel;
    public Animator GameOverPanel => gameOverPanel;

    // Awake���\�b�h�̓I�u�W�F�N�g���L���ɂȂ�Ƃ����ɌĂяo�����
    private void Awake()
    {
        
        // �����ǂꂩ�̃A�j���[�^�[���ݒ肳��Ă��Ȃ���΃G���[���b�Z�[�W��\��
        if (seitoRed == null || seitoPurple == null || seitoWhite == null || teacher == null || phone == null)
        {
            Debug.LogError("One or more Animator references are missing.");
        }
    }
}

using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    // 各アニメーターをインスペクターから設定できるように[SerializeField]を使用
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

    // 公開プロパティを使用してアニメーターへのアクセスを提供
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

    // Awakeメソッドはオブジェクトが有効になるとすぐに呼び出される
    private void Awake()
    {

        // もしどれかのアニメーターが設定されていなければエラーメッセージを表示
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

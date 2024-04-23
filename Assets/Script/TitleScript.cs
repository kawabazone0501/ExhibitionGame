using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour
{
    public AudioSource audioSource;

    Animator fade_animator;
    public GameObject fadeObj;

    public Button onButton;
    //public Button[] onButtons;

    public Button offButton;
    //public Button[] offButtons;
    public Image panel;

    private int CallBack;

    private void Awake()
    {
        fade_animator = fadeObj.GetComponent<Animator>();
        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
       
        fade_animator.SetBool("isFadeOut", true);
        audioSource.loop = true;
        // オーディオを再生する
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectButton()
    {
        fade_animator.SetBool("isFadeIn", true);
        Invoke("SelectScene",3.0f);
    }

    public void SelectScene()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

    public void OnButton()
    {
        panel.gameObject.SetActive(true);
        offButton.gameObject.SetActive(true);
    }

    public void OffButton()
    {
        panel.gameObject.SetActive(false);
        offButton.gameObject.SetActive(false);
    }
}

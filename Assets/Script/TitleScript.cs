using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour
{
    [SerializeField] private GameConstants gameConstants;

    

    Animator fade_animator;
    [SerializeField] private GameObject fadeObj;

    [SerializeField] private Button onButton;
    //public Button[] onButtons;

    [SerializeField] private Button offButton;
    //public Button[] offButtons;
    [SerializeField] private Image panel;

   
    private void Awake()
    {
        Screen.SetResolution(600, 900, false);
        fade_animator = fadeObj.GetComponent<Animator>();
       
    }

   
    // Start is called before the first frame update
    void Start()
    {
       
        fade_animator.SetBool("isFadeOut", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectButton()
    {
        fade_animator.SetBool("isFadeIn", true);
        Invoke("SelectScene",gameConstants.FadeWaitTime);
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

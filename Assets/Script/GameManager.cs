using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    /*
        GameManager
            
        UIを1つのスクリプトでまとめて参照させる
        
        各スクリプトの
     
     */

    private static GameManager instance;

    [SerializeField] private StickController stickController;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private GaugeController gaugeController;
    [SerializeField] private LongPressButton longPressButton;
    [SerializeField] private PauseButton pauseButton;

    //private Dictionary<string, Action> showImageActions;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    Debug.LogError("GameManager instance not found in the scene.");
                }
                else
                {
                    DontDestroyOnLoad(instance.gameObject);
                    Debug.Log("GameManager instance set to DontDestroyOnLoad.");
                }
            }
            return instance;
        }
    }

    public StickController GetStickController() => stickController;
    public AnimationController GetAnimationController() => animationController;
    public GaugeController GetGaugeController() => gaugeController;
    public LongPressButton GetLongPressButton() => longPressButton;
    public PauseButton GetPauseButton() => pauseButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager instance created and set to DontDestroyOnLoad.");
        }
        else if (instance != this)
        {
            Debug.Log("Another instance of GameManager already exists, destroying this instance.");
            Destroy(gameObject);
        }
    }


    // コルーチンを開始するメソッド
    //public void StartAnimation(float waitTime, string controllerName)
    //{
    //    StartCoroutine(AnimationCoroutine(waitTime, controllerName));
    //}

    //private IEnumerator AnimationCoroutine(float waitTime, string controllerName)
    //{
    //    yield return new WaitForSeconds(waitTime);

    //    if (showImageActions.TryGetValue(controllerName, out var showImages))
    //    {
    //        showImages?.Invoke();
    //    }
    //    else
    //    {
    //        Debug.LogError("Invalid controller name specified.");
    //    }
    //}

    public void HideAllImages()
    {
        stickController?.HideImages();
        animationController?.HideImages();
        gaugeController?.HideImages();
        longPressButton?.HideImages();
        pauseButton?.HideButtons();
    }
    private void Start()
    {
        Debug.Log("GameManager Start method called.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

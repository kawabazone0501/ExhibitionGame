using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private GameStateManager gameStateManager;
    [SerializeField]
    private GameConstants gameConstants;

    private Animator SeitoRed;
    private Animator SeitoPurple;
    private Animator SeitoWhite;
    private Animator Teacher;
    private Animator Phone;


    // 定数の定義
    private const int OBJECT_A = 0;
    private const int OBJECT_B = 1;
    private const int OBJECT_C = 2;
    private const int OBJECT_D = 3;
    private const int OBJECT_E = 4;
    private const int OBJECT_F = 5;

    // 再生するアニメーションクリップ
    [SerializeField]
    private AnimationClip[] animationClips;

    // アニメーションが再生されなかった場合の待機時間
    [SerializeField]
    private float waitTimeIfNotPlayed = 1.0f;

    [SerializeField]
    private Image[] gaugeImages;

    public Image[] GaugeImages => gaugeImages;
    [SerializeField]
    private Image[] roomImages;

    public IEnumerator redColoutine;
    public IEnumerator purpleColoutine;
    public IEnumerator whiteColoutine;

   

    public int red_arrival = 0;
    public int purple_arrival = 0;
    public int white_arrival = 0;

    // 出現したオブジェクトの数
    private int objectsSpawned = 0;

    // 出現する最大数
    private int maxObjectsToSpawn;

    public int MaxObjectsSpawn => maxObjectsToSpawn;


   
    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null in Start.");
        }
        else
        {
            Debug.Log("GameManager instance is found in Start.");
        }
        gameStateManager = GameStateManager.Instance;
        
        GameManager.Instance.HideAllImages();

        // PlayerPrefsから設定値を取得
        maxObjectsToSpawn = PlayerPrefs.GetInt("isMax");

        // 条件に応じたオブジェクトの非表示処理
        if (maxObjectsToSpawn == 1)
        {
            for (int i = OBJECT_C; i <= OBJECT_F; i++)
            {
                roomImages[i].gameObject.SetActive(false);
            }
        }
        else if (maxObjectsToSpawn == 2)
        {
            roomImages[OBJECT_A].gameObject.SetActive(false);
            roomImages[OBJECT_B].gameObject.SetActive(false);
            roomImages[OBJECT_E].gameObject.SetActive(false);
            roomImages[OBJECT_F].gameObject.SetActive(false);
        }
        else if (maxObjectsToSpawn == 3)
        {
            for (int i = OBJECT_A; i <= OBJECT_D; i++)
            {
                roomImages[i].gameObject.SetActive(false);
            }
        }

        PlayerPrefs.DeleteKey("isMax");
        PlayerPrefs.Save();

        AnimatorController animatorController = FindAnyObjectByType<AnimatorController>();
        if (animatorController != null)
        {
            SeitoRed = animatorController.SeitoRed;
            SeitoPurple = animatorController.SeitoPurple;
            SeitoWhite = animatorController.SeitoWhite;
            Teacher = animatorController.Teacher;
            Phone = animatorController.Phone;
        }
       
        redColoutine = redAnimation();
        purpleColoutine = purpleAnimation();
        whiteColoutine = whiteAnimation();
        GameStateManager.Instance.SetAllStudentsFalse();
    }

    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("void Start");
        StartCoroutine(redColoutine);
        StartCoroutine(purpleColoutine);
        StartCoroutine(whiteColoutine);
        
    }

    

    // Update is called once per frame
    void Update()
    {

    }

    public void HideImages()
    {
        foreach(var image in gaugeImages)
        {
            if (image != null)
            {
                image.enabled = false;
            }
        }
    }

    public IEnumerator redAnimation()
    {
        while (true)
        {

            if (!gameStateManager.IsStudents[gameConstants.Student_RED] && gameStateManager.IsClear)
            {
                gameStateManager.IsStudents[gameConstants.Student_RED] = true;
                Debug.Log("Stop_red");
                gameStateManager.IsClear = true;
                StopCoroutine(redColoutine);
            }
            //指定した確率でアニメーションを再生
            else if (Random.Range(0.0f, 1.0f) <= 0.3f && !gameStateManager.IsStudents[gameConstants.Student_RED] && objectsSpawned < maxObjectsToSpawn && !gameConstants.IsObjectAllowed)
            {
                red_arrival++;
                objectsSpawned++;
                if (objectsSpawned >= maxObjectsToSpawn)
                {
                    gameConstants.IsObjectAllowed = true;
                }
                if (!gameStateManager.IsStudent)
                {
                    gameStateManager.IsStudent = true;
                }
                gameStateManager.IsStudents[gameConstants.Student_RED] = true;
                Debug.Log("enter_red");
                SeitoRed.SetBool("isRed", true);
                yield return new WaitForSeconds(animationClips[0].length + 0.5f);
                GameManager.Instance.GetGaugeController().ShowImagesInRange(1, 3);
                if (redColoutine != null) // Coroutineが実行中であれば停止
                {
                    Debug.Log("stop_red");
                    StopCoroutine(redColoutine);
                }
            }
            else
            {
                Debug.Log("no_red");
                yield return new WaitForSeconds(waitTimeIfNotPlayed);
            }
        }
    }

    public IEnumerator purpleAnimation()
    {
        while (true)
        {
            /* if (!gameStateManager.IsStudents[gameConstants.Student_PURPLE] && gameStateManager.IsClear)
             {
                 gameStateManager.IsStudents[gameConstants.Student_PURPLE] = true;
                 Debug.Log("Stop_purple");
                 gameStateManager.IsClear = true;
                 StopCoroutine(purpleColoutine);
             }*/
            if (!gameStateManager.IsStudents[gameConstants.Student_PURPLE] && gameStateManager.IsClear)
            {
                StopAnimation(ref whiteColoutine, ref gameStateManager.IsStudents[gameConstants.Student_PURPLE]);
            }
            //指定した確率でアニメーションを再生
                    /*else if (Random.Range(0.0f, 1.0f) <= 0.3f && !gameStateManager.IsStudents[gameConstants.Student_PURPLE] && objectsSpawned < maxObjectsToSpawn && !isObjectAllowed)
                    {
                        purple_arrival++;
                        objectsSpawned++;
                        if (objectsSpawned >= maxObjectsToSpawn)
                        {
                            isObjectAllowed = true;
                        }
                        if (!gameStateManager.IsStudent)
                        {
                           gameStateManager.IsStudent = true;
                        }
                        gameStateManager.IsStudents[gameConstants.Student_PURPLE] = true;    
                        Debug.Log("enter_purple");
                        SeitoPurple.SetBool("isPurple",true);
                        yield return new WaitForSeconds(animationClips[gameConstants.Student_PURPLE].length + 0.25f);
                        GameManager.Instance.GetStickController().ShowImages();
                        if(purpleColoutine != null)
                        {
                            StopCoroutine(purpleColoutine);
                        }
                    }*/
            else if (ShouldPlayAnimation(gameConstants.Student_PURPLE))
            {
                gameStateManager.IsStudentLock = true; // Lock
                PlayAnimation
                  (
                     ref purple_arrival,
                     ref gameStateManager.IsStudents[gameConstants.Student_PURPLE],
                     SeitoPurple,
                     "isPurple",
                     animationClips[gameConstants.Student_PURPLE].length + 0.5f
                   );
                gameStateManager.IsStudentLock = false;
            }
            else
            {
                Debug.Log("no_purple");
                yield return new WaitForSeconds(waitTimeIfNotPlayed);
            }
        }
    }

    public IEnumerator whiteAnimation()
    {
        while (true)
        {
            if (!gameStateManager.IsStudents[gameConstants.Student_WHITE] && gameStateManager.IsClear)
            {
                /*isWhite = true;
                Debug.Log("Stop_gray");
                GameManager.Instance.GetGaugeController().isClear = true;
                StopCoroutine(whiteColoutine);*/
                StopAnimation(ref whiteColoutine, ref gameStateManager.IsStudents[gameConstants.Student_WHITE]);
            }
            //指定した確率でアニメーションを再生
            /*else if (Random.Range(0.0f, 1.0f) <= 0.3f && !isWhite && objectsSpawned < maxObjectsToSpawn && !isObjectAllowed)
            {
                gray_arrival++;
                objectsSpawned++;
                if (objectsSpawned >= maxObjectsToSpawn)
                {
                    isObjectAllowed = true;
                }
                if (!GameManager.Instance.GetGaugeController().isStudent)
                {
                    GameManager.Instance.GetGaugeController().isStudent = true;
                }
                isWhite = true;
                Debug.Log("enter_gray");
                SeitoWhite.SetBool("isWhite", true);
                Phone.SetBool("isCall", true);
                yield return new WaitForSeconds(animationClips[2].length + 0.5f);
                GameManager.Instance.GetLongPressButton().ShowImages();
                if (whiteColoutine != null)
                {
                    Debug.Log("stop_gray");
                    StopCoroutine(whiteColoutine);
                }
            }*/
            else if (ShouldPlayAnimation( gameConstants.Student_WHITE))
            {
                gameStateManager.IsStudentLock = true; // Lock
                PlayAnimation
                  (
                     ref white_arrival, 
                     ref gameStateManager.IsStudents[gameConstants.Student_WHITE], 
                     SeitoWhite,
                     "isWhite",
                     animationClips[gameConstants.Student_WHITE].length + 0.5f
                   );
                
                Phone.SetBool("isCall", true);
                gameStateManager.IsStudentLock = false;
            }
            else
            {
                Debug.Log("no_gray");
                yield return new WaitForSeconds(waitTimeIfNotPlayed);
            }
        }
    }
    
    private bool ShouldPlayAnimation( int studentNumber)
    {
        Debug.Log(gameStateManager.IsStudents[studentNumber]);
        Debug.Log(gameConstants.IsObjectAllowed);
        Debug.Log(gameStateManager.IsStudentLock);
        return Random.Range(0.0f, 1.0f) <= 0.3f &&
           !gameStateManager.IsStudents[studentNumber] && 
           objectsSpawned < maxObjectsToSpawn && 
           !gameConstants.IsObjectAllowed && 
           !gameStateManager.IsStudentLock;
    }

    private void PlayAnimation
        (
           ref int arrival, 
           ref bool isPlaying, 
           Animator animator, 
           string animationBool,
           float waitTime
        )
    {
        gameStateManager.IsStudent = true;
        arrival++;
        objectsSpawned++;
        if(objectsSpawned>=maxObjectsToSpawn)
        {
            gameConstants.IsObjectAllowed = true;
        }
        isPlaying = true;
        animator.SetBool(animationBool, true);
        StartCoroutine(AnimationCoroutine(waitTime));
    }

    private IEnumerator AnimationCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameManager.Instance.GetLongPressButton().ShowImages();
        // StopCoroutine を使用せずに、現在のコルーチンを終了します。
        yield break;
    }
    private void StopAnimation
        (
           ref IEnumerator coroutine, 
           ref bool isPlaying
        )
    {
        isPlaying = true;
        gameStateManager.IsClear = true;
        StopCoroutine(coroutine);
    }

    // FillAmount を 0 にする関数
    public void ResetFillAmount()
    {
        gameStateManager.IsRedCard = true;
        SeitoRed.SetBool("isRed", false);
        SeitoPurple.SetBool("isPurple", false);
        SeitoWhite.SetBool("isWhite", false);
        GameManager.Instance.GetGaugeController().RedGaugeImages[3].fillAmount = 0.0f;
        GameManager.Instance.GetStickController().PurpleGaugeImages[1].fillAmount = 0.0f;
        GameManager.Instance.GetLongPressButton().WhiteInGaugeImage.fillAmount = 0.0f;
        GameManager.Instance.HideAllImages();
    }

    public void Restart_Red()
    {
        objectsSpawned--;
        gameConstants.IsObjectAllowed = false;
        StartCoroutine(redColoutine);
    }

    public void Restart_Purple()
    {
        objectsSpawned--;
        gameConstants.IsObjectAllowed = false;
        StartCoroutine(purpleColoutine);
    }

    public void Restart_White()
    {
        objectsSpawned--;
        gameConstants.IsObjectAllowed = false;
        StartCoroutine(whiteColoutine);
    }

    public void OnButton()
    {
        Teacher.SetBool("vsRed", true);
    }

    public void OffButton()
    {
        Teacher.SetBool("vsRed", false);
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private GameStateManager gameStateManager;
    [SerializeField]
    private GameConstants gameConstants;
    [SerializeField]
    private UIManager uiManager;

    private Animator SeitoRed;
    private Animator SeitoPurple;
    private Animator SeitoWhite;
    private Animator Teacher;
    private Animator Phone;
    private Animator RedGuide;
    private Animator PurpleGuide;
    private Animator WhiteGuide;

    
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

        // PlayerPrefsから設定値を取得
        maxObjectsToSpawn = PlayerPrefs.GetInt("isMax");
        Debug.Log(maxObjectsToSpawn);
        // 条件に応じたオブジェクトの非表示処理
        if (maxObjectsToSpawn == gameConstants.FirstSeason)
        {
            Debug.Log("maxObjectsToSpawn=");
            Debug.Log(maxObjectsToSpawn);
            Debug.Log("gameConstants.FirstSeason=");
            Debug.Log(gameConstants.FirstSeason);
            for (int i = gameConstants.OBJECT_C; i <= gameConstants.OBJECT_F; i++)
            {
                uiManager.RoomImages[i].gameObject.SetActive(false);
            }
        }
        else if (maxObjectsToSpawn == gameConstants.SecondSeason)
        {
            Debug.Log("maxObjectsToSpawn=");
            Debug.Log(maxObjectsToSpawn);
            Debug.Log("gameConstants.SecondSeason=");
            Debug.Log(gameConstants.SecondSeason);
            uiManager.RoomImages[gameConstants.OBJECT_A].gameObject.SetActive(false);
            uiManager.RoomImages[gameConstants.OBJECT_B].gameObject.SetActive(false);
            uiManager.RoomImages[gameConstants.OBJECT_E].gameObject.SetActive(false);
            uiManager.RoomImages[gameConstants.OBJECT_F].gameObject.SetActive(false);
        }
        else if (maxObjectsToSpawn == gameConstants.ThirdSeason)
        {
            Debug.Log("maxObjectsToSpawn=");
            Debug.Log(maxObjectsToSpawn);
            Debug.Log("gameConstants.ThirdSeason=");
            Debug.Log(gameConstants.ThirdSeason);
            for (int i = gameConstants.OBJECT_A; i <= gameConstants.OBJECT_D; i++)
            {
                uiManager.RoomImages[i].gameObject.SetActive(false);
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
            RedGuide = animatorController.RedGuide;
            PurpleGuide = animatorController.PurpleGuide;
            WhiteGuide = animatorController.WhiteGuide;
        }
       
        redColoutine = redAnimation();
        purpleColoutine = purpleAnimation();
        whiteColoutine = whiteAnimation();
        GameStateManager.Instance.SetAllStudentsFalse();
        Debug.Log(gameConstants.IsObjectAllowed);
        Debug.Log(gameStateManager.IsStudentLock);
    }

    
    public void Start()
    {
        Debug.Log("void Start");
        StartCoroutine(redColoutine);
        StartCoroutine(purpleColoutine);
        StartCoroutine(whiteColoutine);
        
    }

    public IEnumerator redAnimation()
    {
        while (true)
        {

            if (!gameStateManager.IsStudents[gameConstants.StudentRED] && gameStateManager.IsClear)
            {
               
                StopAnimation
                    (
                       ref purpleColoutine,
                       ref gameStateManager.IsStudents[gameConstants.StudentPURPLE]
                    );
            }
            
            else if(ShouldPlayAnimation(gameConstants.StudentRED))
            {
                gameStateManager.IsStudentLock = true;//Lock;
                
                PlayAnimation
                    (
                       ref red_arrival,
                       ref gameStateManager.IsStudents[gameConstants.StudentRED],
                        SeitoRed,
                       "isRed",
                       gameConstants.AnimationClips[gameConstants.StudentRED].length + gameConstants.WaitAnimationTime
                    );
                gameConstants   .IsStudentLock = false;
                uiManager.InvokeAction
                     (
                         uiManager.RedShow,
                         gameConstants.StartDisplayImageRed,
                         gameConstants.EndDisplayImageRed,
                         gameConstants.AnimationClips[gameConstants.StudentRED].length + gameConstants.WaitAnimationTime
                      );
                if(!gameStateManager.RedGuidePlayed)
                {
                    gameStateManager.RedGuidePlayed = true;
                    RedInvokeGuideAnimation
                   (
                       gameConstants.AnimationClips[gameConstants.StudentRED].length
                       + gameConstants.WaitAnimationTime
                   );
                }
                
                if (redColoutine != null) // Coroutineが実行中であれば停止
                {
                    Debug.Log("stop_red");
                    StopCoroutine(redColoutine);
                }
            }
            else
            {
                Debug.Log("no_red");
                yield return new WaitForSeconds(gameConstants.WaitTimeIfNotPlayed);
            }
            // ループの最後で yield return null; を呼び出して無限ループを回避
            yield return null;
        }
    }

    public IEnumerator purpleAnimation()
    {
        while (true)
        {
           
            if (!gameStateManager.IsStudents[gameConstants.StudentPURPLE] && gameStateManager.IsClear)
            {
                StopAnimation
                    (
                        ref purpleColoutine,
                        ref gameStateManager.IsStudents[gameConstants.StudentPURPLE]
                    );
            }
            //指定した確率でアニメーションを再生
           
            else if (ShouldPlayAnimation(gameConstants.StudentPURPLE))
            {
                gameStateManager.IsStudentLock = true; // Lock
                PlayAnimation
                  (
                     ref purple_arrival,
                     ref gameStateManager.IsStudents[gameConstants.StudentPURPLE],
                     SeitoPurple,
                     "isPurple",
                     gameConstants.AnimationClips[gameConstants.StudentPURPLE].length + 0.5f
                   );
                gameStateManager.IsStudentLock = false;
                uiManager.InvokeAction
                    (
                        uiManager.PurpleShow,
                        gameConstants.StartDisplayImagePurple,
                        gameConstants.EndDisplayImagePurple,
                        gameConstants.AnimationClips[gameConstants.StudentPURPLE].length
                        + gameConstants.WaitAnimationTime
                     );
               if(!gameStateManager.PurpleGuidePlayed)
                {
                    gameStateManager.PurpleGuidePlayed = true;
                    PurpleInvokeGuideAnimation
                        (
                            gameConstants.AnimationClips[gameConstants.StudentPURPLE].length
                            + gameConstants.WaitAnimationTime
                        );
                }
                if (purpleColoutine != null) // Coroutineが実行中であれば停止
                {
                    Debug.Log("stop_purple");
                    StopCoroutine(purpleColoutine);
                }
            }
            else
            {
                Debug.Log("no_purple");
                yield return new WaitForSeconds(gameConstants.WaitTimeIfNotPlayed);
            }
            // ループの最後で yield return null; を呼び出して無限ループを回避
            yield return null;
        }
    }

    public IEnumerator whiteAnimation()
    {
        while (true)
        {
            if (!gameStateManager.IsStudents[gameConstants.StudentWHITE] && gameStateManager.IsClear)
            {
               
                StopAnimation(ref whiteColoutine, ref gameStateManager.IsStudents[gameConstants.StudentWHITE]);
            }
            //指定した確率でアニメーションを再生
            
            else if (ShouldPlayAnimation(gameConstants.StudentWHITE))
            {
                gameStateManager.IsStudentLock = true; // Lock
                PlayAnimation
                  (
                     ref white_arrival, 
                     ref gameStateManager.IsStudents[gameConstants.StudentWHITE], 
                     SeitoWhite,
                     "isWhite",
                     gameConstants.AnimationClips[gameConstants.StudentWHITE].length + gameConstants.WaitAnimationTime
                   );
                
                Phone.SetBool("isCall", true);
                gameStateManager.IsStudentLock = false;
                uiManager.InvokeAction
                    (
                        uiManager.WhiteShow,
                        gameConstants.StartDisplayImageWhite,
                        gameConstants.EndDisplayImageWhite,
                        gameConstants.AnimationClips[gameConstants.StudentWHITE].length
                        + gameConstants.WaitAnimationTime
                     );
                if(!gameStateManager.WhiteGuidePlayed)
                {
                    gameStateManager.WhiteGuidePlayed = true;
                    WhiteInvokeGuideAnimation
                        (
                            gameConstants.AnimationClips[gameConstants.StudentWHITE].length
                            + gameConstants.WaitAnimationTime
                        );
                }
                if(whiteColoutine == null)
                {
                    Debug.Log("stop_white");
                    StopCoroutine(whiteColoutine);
                }
            }
            else
            {
                Debug.Log("no_gray");
                yield return new WaitForSeconds(gameConstants.WaitTimeIfNotPlayed);
            }
            // ループの最後で yield return null; を呼び出して無限ループを回避
            yield return null;
        }
    }
    
    private void RedInvokeGuideAnimation
        (
            float delay
        )
    {
        StartCoroutine(RedInvokeAfterDelay(delay)); 
    }

    private IEnumerator RedInvokeAfterDelay
        (
            float delay
        )
    {
        yield return new WaitForSeconds(delay);
        RedGuide.SetBool("isRedGuide",true);
    }

    private void PurpleInvokeGuideAnimation
        (
            float delay
        )
    {
        StartCoroutine(PurpleInvokeAfterDelay(delay));
    }

    private IEnumerator PurpleInvokeAfterDelay
        (
            float delay
        )
    {
        yield return new WaitForSeconds(delay);
        PurpleGuide.SetBool("isPurpleGuide", true);
    }

    private void WhiteInvokeGuideAnimation
        (
            float delay
        )
    {
        StartCoroutine(WhiteInvokeAfterDelay(delay));
    }

    private IEnumerator WhiteInvokeAfterDelay
        (
            float delay
        )
    {
        yield return new WaitForSeconds(delay);
        WhiteGuide.SetBool("isWhiteGuide", true);
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
        arrival++;
        objectsSpawned++;
        if (objectsSpawned >= maxObjectsToSpawn)
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
        // StopCoroutine を使用せずに、現在のコルーチンを終了する。
        yield break;
    }
    private void StopAnimation
        (
           ref IEnumerator coroutine, 
           ref bool isPlaying
        )
    {
        isPlaying = false;
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
        uiManager.GaugeImages[gameConstants.RedGauge].fillAmount = gameConstants.GaugeFillAmountThresholdReset;
        uiManager.GaugeImages[gameConstants.PurpleGauge].fillAmount = gameConstants.GaugeFillAmountThresholdReset;
        uiManager.GaugeImages[gameConstants.WhiteGauge].fillAmount = gameConstants.GaugeFillAmountThresholdReset;
        UIManager.Instance.HideImagesInRange(gameConstants.NeverDisplayImage, gameConstants.EndDisplayImage);
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

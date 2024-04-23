using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    Animator red_animator;
    Animator purple_animator;
    Animator gray_animator;
    Animator KB_animator;
    Animator Phone_animator;

    public GaugeController g_Contller;
    public LongPressButton longPress_Contller;
    public StickController stick_Contller;

    // 再生するアニメーションクリップ
    public AnimationClip[] animationClips;
    
    public GameObject redObj;
    public GameObject purpleObj;
    public GameObject grayObj;
    public GameObject kobaObj;
    public GameObject PhoneObj;

    // アニメーションを再生する確率（0から1の間）
    public float playProbability = 1.0f;
    // アニメーションが再生されなかった場合の待機時間
    public float waitTimeIfNotPlayed = 1.0f;

    public Image[] gauge_Images;
    public GameObject[] Room_Objects;

    public IEnumerator red_Coloutine;
    public IEnumerator purple_Coloutine;
    public IEnumerator gray_Coloutine;

    public bool isRed = false;
    public bool isPurple = false;
    public bool isGray = false;

    public bool red_card = false;

    public int red_arrival = 0;
    public int purple_arrival = 0;
    public int gray_arrival = 0;

    // 出現したオブジェクトの数
    int objectsSpawned = 0;

    // 出現する最大数
    public int maxObjectsToSpawn;

    // 出現フラグ
    bool isObjectAllowed = false;

    private void Awake()
    {
        maxObjectsToSpawn = PlayerPrefs.GetInt("isMax");
        if (maxObjectsToSpawn == 1)
        {
            for (int i = 2; i < 6; i++)
            {
                Room_Objects[i].gameObject.SetActive(false);
            }
        }
        else if (maxObjectsToSpawn == 2)
        {
            Room_Objects[0].gameObject.SetActive(false);
            Room_Objects[1].gameObject.SetActive(false);
            Room_Objects[4].gameObject.SetActive(false);
            Room_Objects[5].gameObject.SetActive(false);
        }
        else if (maxObjectsToSpawn == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                Room_Objects[i].gameObject.SetActive(false);
            }
        }

        PlayerPrefs.DeleteKey("isMax");
        PlayerPrefs.Save();

        red_animator = redObj.GetComponent<Animator>();
        purple_animator = purpleObj.GetComponent<Animator>();
        gray_animator = grayObj.GetComponent<Animator>();
        KB_animator = kobaObj.GetComponent<Animator>();
        Phone_animator = PhoneObj.GetComponent<Animator>();
        red_Coloutine = red_Animation();
        purple_Coloutine = purple_Animation();
        gray_Coloutine = gray_Animation();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(red_Coloutine);
        StartCoroutine(purple_Coloutine);
        StartCoroutine(gray_Coloutine);

        g_Contller.increaseButton1.gameObject.SetActive(false);
        g_Contller.increaseButton2.gameObject.SetActive(false);
        longPress_Contller.gaugeImage.gameObject.SetActive(false);
        stick_Contller.gaugeImage.gameObject.SetActive(false);
        

        for (int i = 0; i < gauge_Images.Length; i++)
        {
            gauge_Images[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator red_Animation()
    {
        while (true)
        {

            if (!isRed && g_Contller.isClear)
            {
                isRed = true;
                Debug.Log("Stop_red");
                g_Contller.isClear = true;
                StopCoroutine(red_Coloutine);
            }
            //指定した確率でアニメーションを再生
            else if (Random.Range(0.0f, 1.0f) <= 0.3f && !isRed && objectsSpawned < maxObjectsToSpawn && !isObjectAllowed)
            {
                red_arrival++;
                objectsSpawned++;
                if (objectsSpawned >= maxObjectsToSpawn)
                {
                    isObjectAllowed = true;
                }
                if (!g_Contller.isStudent)
                {
                    g_Contller.isStudent = true;
                }
                isRed = true;
                Debug.Log("enter_red");
                red_animator.SetBool("isRed", true);
                yield return new WaitForSeconds(animationClips[0].length + 0.5f);
                for (int i = 1; i < 4; i++)
                {
                    gauge_Images[i].gameObject.SetActive(true);
                }
                g_Contller.increaseButton1.gameObject.SetActive(true);
                g_Contller.increaseButton2.gameObject.SetActive(true);
                if (red_Coloutine != null) // Coroutineが実行中であれば停止
                {
                    StopCoroutine(red_Coloutine);
                }
            }
            else
            {
                Debug.Log("no_red");
                yield return new WaitForSeconds(waitTimeIfNotPlayed);
            }
        }
    }

    public IEnumerator purple_Animation()
    {
        while (true)
        {
            if (!isPurple && g_Contller.isClear)
            {
                isPurple = true;
                Debug.Log("Stop_purple");
                g_Contller.isClear = true;
                StopCoroutine(purple_Coloutine);
            }
            //指定した確率でアニメーションを再生
            else if (Random.Range(0.0f, 1.0f) <= 0.3f && !isPurple && objectsSpawned < maxObjectsToSpawn && !isObjectAllowed)
            {
                purple_arrival++;
                objectsSpawned++;
                if (objectsSpawned >= maxObjectsToSpawn)
                {
                    isObjectAllowed = true;
                }
                if (!g_Contller.isStudent)
                {
                    g_Contller.isStudent = true;
                }
                isPurple = true;    
                Debug.Log("enter_purple");
                purple_animator.SetBool("isPurple",true);
                yield return new WaitForSeconds(animationClips[1].length + 0.25f);
                for (int i = 4; i < 9; i++)
                {
                    gauge_Images[i].gameObject.SetActive(true);
                }
                stick_Contller.gaugeImage.gameObject.SetActive(true);
                if(purple_Coloutine != null)
                {
                    StopCoroutine(purple_Coloutine);
                }
            }
            else
            {
                Debug.Log("no_purple");
                yield return new WaitForSeconds(waitTimeIfNotPlayed);
            }
        }
    }

    public IEnumerator gray_Animation()
    {
        while (true)
        {
            if (!isGray && g_Contller.isClear)
            {
                isGray = true;
                Debug.Log("Stop_gray");
                g_Contller.isClear = true;
                StopCoroutine(gray_Coloutine);
            }
            //指定した確率でアニメーションを再生
            else if (Random.Range(0.0f, 1.0f) <= 0.3f && !isGray && objectsSpawned < maxObjectsToSpawn && !isObjectAllowed)
            {
                gray_arrival++;
                objectsSpawned++;
                if (objectsSpawned >= maxObjectsToSpawn)
                {
                    isObjectAllowed = true;
                }
                if (!g_Contller.isStudent)
                {
                    g_Contller.isStudent = true;
                }
                isGray = true;
                Debug.Log("enter_gray");
                gray_animator.SetBool("isGray", true);
                Phone_animator.SetBool("isCall", true);
                yield return new WaitForSeconds(animationClips[2].length + 0.5f);
                 gauge_Images[9].gameObject.SetActive(true);
                 gauge_Images[10].gameObject.SetActive(true);
                longPress_Contller.gray_Button.gameObject.SetActive(true);
                if (gray_Coloutine != null)
                {
                    Debug.Log("stop_gray");
                    StopCoroutine(gray_Coloutine);
                }
                Phone_animator.SetBool("isCall", true);
            }
            else
            {
                Debug.Log("no_gray");
                yield return new WaitForSeconds(waitTimeIfNotPlayed);
            }
        }
    }

    // FillAmount を 0 にする関数
    public void ResetFillAmount()
    {
        red_card = true;
        red_animator.SetBool("isRed", false);
        purple_animator.SetBool("isPurple", false);
        gray_animator.SetBool("isGray", false);
        g_Contller.w_Image.fillAmount = 0f;
        stick_Contller.gaugeImage.fillAmount = 0f;
        longPress_Contller.gaugeImage.fillAmount = 0f;

        for (int i = 0; i < gauge_Images.Length; i++)
        {
            gauge_Images[i].gameObject.SetActive(false);
        }
    }

    public void Restart_Red()
    {
        objectsSpawned--;
        isObjectAllowed = false;
        StartCoroutine(red_Coloutine);
    }

    public void Restart_Purple()
    {
        objectsSpawned--;
        isObjectAllowed = false;
        StartCoroutine(purple_Coloutine);
    }

    public void Restart_Gray()
    {
        objectsSpawned--;
        isObjectAllowed = false;
        StartCoroutine(gray_Coloutine);
    }

    public void OnButton()
    {
        KB_animator.SetBool("vsRed", true);
    }

    public void OffButton()
    {
        KB_animator.SetBool("vsRed", false);
    }
}

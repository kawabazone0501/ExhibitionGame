using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class TextDisplay : MonoBehaviour
{
    [SerializeField] private Text displayText;   // 画面に表示するTextコンポーネント
    [SerializeField] private string filePath = "操作説明"; // Resourcesフォルダ内の.txtファイル名

    // 複数アニメーションを制御できるようにする
    [SerializeField] private List<Animator> animators;

    //private List<string> sections;

    //private int currentSectionIndex = 0;

    //private bool isDisplaying = false; // テキストを表示中かどうかを示すフラグ

    private List<string> sections;
    private int currentSectionIndex = 0;
    private bool isDisplaying = false;
    private bool isHidden = false;
    private bool isTextVisible = false;         // テキスト表示中かどうか


    void Start()
    {
        LoadTextFromFile();
        //StartCoroutine(DisplayText());
        if (sections.Count > 0)
        {
            StartCoroutine(DisplayText());
        }
    }

    /* void LoadTextFromFile()
     {
         TextAsset textAsset = Resources.Load<TextAsset>(filePath);
         if (textAsset != null)
         {
             // <section>タグでテキストを分割し、各セクションの先頭と末尾の空白をトリム
             string fullText = textAsset.text;
             sections = new List<string>();

             string[] rawSections = fullText.Split(new string[] { "<section>" }, System.StringSplitOptions.None);
             foreach (string section in rawSections)
             {
                 // トリムして空でないセクションを追加
                 string trimmedSection = section.Trim();
                 if (!string.IsNullOrEmpty(trimmedSection))
                 {
                     sections.Add(trimmedSection);
                 }
             }
         }
         else
         {
             Debug.LogError("Text file not found!");
         }
     }



     IEnumerator DisplayText()
     {
         if (currentSectionIndex < sections.Count)
         {
             isDisplaying = true;
             string currentSection = sections[currentSectionIndex];
             displayText.text = "";

             foreach (char c in currentSection)
             {
                 displayText.text += c;
                 yield return new WaitForSeconds(0.05f); // 文字送りのスピードを調整
             }

             isDisplaying = false;
         }

     }

     public void NextSection()
     {
         if (!isDisplaying && currentSectionIndex < sections.Count - 1)
         {
             currentSectionIndex++;
             StartCoroutine(DisplayText());
         }
         else if (currentSectionIndex >= sections.Count - 1)
         {
             displayText.gameObject.SetActive(false); // 全てのセクションが表示されたらテキストを非表示にする
         }
     }*/

    void LoadTextFromFile()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
        if (textAsset != null)
        {
            string fullText = textAsset.text;
            sections = new List<string>();

            // テキストをセクションに分割
            string[] rawSections = fullText.Split(new string[] { "<section>" }, System.StringSplitOptions.None);
            foreach (string section in rawSections)
            {
                string trimmedSection = section.Trim();
                if (!string.IsNullOrEmpty(trimmedSection))
                {
                    // <hide> と <show> タグを分けて処理
                    List<string> processedSections = ProcessTags(trimmedSection);
                    sections.AddRange(processedSections);
                }
            }
        }
        else
        {
            Debug.LogError("Text file not found!");
        }
    }

    List<string> ProcessTags(string text)
    {
        List<string> result = new List<string>();
        string[] lines = text.Split(new string[] { "\n" }, System.StringSplitOptions.None);
        string currentText = "";

        foreach (string line in lines)
        {
            if (line.Contains("<hide>"))
            {
                if (!string.IsNullOrEmpty(currentText))
                {
                    result.Add(currentText);
                    currentText = "";
                }
                result.Add("<hide>");
            }
            else if (line.Contains("<show>"))
            {
                if (!string.IsNullOrEmpty(currentText))
                {
                    result.Add(currentText);
                    currentText = "";
                }
                result.Add("<show>");
            }
            else
            {
                currentText += line + "\n";
            }
        }

        if (!string.IsNullOrEmpty(currentText))
        {
            result.Add(currentText);
        }

        return result;
    }

    IEnumerator DisplayText()
    {
        while (currentSectionIndex < sections.Count)
        {
            isDisplaying = true;
            string currentSection = sections[currentSectionIndex];
            displayText.text = "";

            if (currentSection == "<hide>")
            {
                HideSection();
                yield break; // 非表示時は一旦停止、NextSectionで再開
            }

            if (currentSection == "<show>")
            {
                ShowNextSection();
                yield break; // 再表示時も停止、NextSectionで再開
            }

            // < Animation > タグの検出と処理
            if (currentSection.Contains("<Animation"))
            {
                HandleAnimationTag(currentSection);
                isDisplaying = false;
                yield break; // 次はNextSectionで進める
            }

            // 通常テキスト表示
            string[] lines = currentSection.Split(new string[] { "\n" }, System.StringSplitOptions.None);
            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    displayText.text += c;
                    yield return new WaitForSeconds(0.05f); // 文字送りのスピードを調整
                }
                displayText.text += "\n";
            }

            if (currentSectionIndex < 0) currentSectionIndex = 0;
            if (currentSectionIndex >= sections.Count) currentSectionIndex = sections.Count - 1;


            isDisplaying = false;
            Debug.Log("テキスト終了");
            yield break; // 現在のセクションの表示が完了したら、コルーチンを終了する
        }

        displayText.gameObject.SetActive(false); // 全てのセクションが表示されたらテキストを非表示にする
    }

    public void NextSection()
    {
        if (!isDisplaying && currentSectionIndex < sections.Count - 1)
        {
            currentSectionIndex++;
            StartCoroutine(DisplayText());
        }
        else if (currentSectionIndex >= sections.Count - 1)
        {
            displayText.gameObject.SetActive(false); // 全てのセクションが表示されたらテキストを非表示にする
        }

    }

    public void HideSection()
    {
        if (!isHidden)
        {
            isHidden = true;
            displayText.gameObject.SetActive(false);
        }
    }

    public void ShowNextSection()
    {
        if (isHidden)
        {
            isHidden = false;
            displayText.gameObject.SetActive(true);
            if (currentSectionIndex < sections.Count - 1)
            {
                currentSectionIndex++;
                //StartCoroutine(DisplayText());
            }
        }
    }

    private void HandleAnimationTag(string line)
    {
        // 例: <Animation object="Player" trigger="Jump">
        string objectName = GetAttributeValue(line, "object");
        string triggerName = GetAttributeValue(line, "trigger");

        foreach (var anim in animators)
        {
            if (anim.gameObject.name == objectName)
            {
                anim.SetTrigger(triggerName);
                Debug.Log($"アニメーション再生: {objectName} → {triggerName}");
                break;
            }
        }
    }

    private string GetAttributeValue(string line, string attribute)
    {
        string search = attribute + "=\"";
        int start = line.IndexOf(search) + search.Length;
        int end = line.IndexOf("\"", start);
        return line.Substring(start, end - start);
    }

    // アニメーション呼び出し処理
    private void TriggerAnimation(string line)
    {
        string objectName = GetAttributeValue(line, "object");
        string triggerName = GetAttributeValue(line, "trigger");

        if (string.IsNullOrEmpty(objectName) || string.IsNullOrEmpty(triggerName))
        {
            Debug.LogWarning("Animationタグの属性が不正です: " + line);
            return;
        }

        GameObject target = GameObject.Find(objectName);
        if (target == null)
        {
            Debug.LogWarning("対象オブジェクトが見つかりません: " + objectName);
            return;
        }

        Animator animator = target.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }
}

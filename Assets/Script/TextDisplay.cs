using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class TextDisplay : MonoBehaviour
{
    [SerializeField] private Text displayText;// 画面に表示するTextコンポーネント

    [SerializeField] private string filePath = "操作説明";// Resourcesフォルダ内の.txtファイル名



    //private List<string> sections;

    //private int currentSectionIndex = 0;

    //private bool isDisplaying = false; // テキストを表示中かどうかを示すフラグ

    private List<string> sections;
    private int currentSectionIndex = 0;
    private bool isDisplaying = false;
    private bool isHidden = false;


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
                yield return new WaitUntil(() => !isHidden); // HideSection()が終了するまで待機
                currentSectionIndex++;
                continue; // 次のセクションに進む
            }

            if (currentSection == "<show>")
            {
                ShowNextSection();
                yield return new WaitUntil(() => !isHidden); // ShowNextSection()が終了するまで待機
                currentSectionIndex++;
                continue; // 次のセクションに進む
            }

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
}

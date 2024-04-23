using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private int previousSceneIndex;
    private int max_Button_Current;

    public GameObject Pause_Panel;
    public Button[] Pause_Buttons;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            Pause_Buttons[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPausePanel()
    {
        Pause_Panel.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            Pause_Buttons[i].gameObject.SetActive(true);
        }
        Time.timeScale = 0.0f;
    }

    public void OffPausePanel()
    {
        Time.timeScale = 1.0f;
        Pause_Panel.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            Pause_Buttons[i].gameObject.SetActive(false);
        }
    }

    public void OnSelectLoad()
    {
        Time.timeScale = 1.0f;
        // �O�̃V�[���̃C���f�b�N�X���擾����
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;

        // �O�̃V�[���ɖ߂�
        SceneManager.LoadScene(previousSceneIndex);
    }

    public void OnTitleLoad()
    {
        Time.timeScale = 1.0f;
        // �O�̃V�[���̃C���f�b�N�X���擾����
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 2;

        // �O�̃V�[���ɖ߂�
        SceneManager.LoadScene(previousSceneIndex);
    }
}

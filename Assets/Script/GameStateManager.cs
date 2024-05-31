using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager instance;

    [SerializeField]
    private GameConstants gameConstants;


    private void Awake()
    {
        
    }

    public static GameStateManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameStateManager>();
                if (instance == null)
                {
                    Debug.LogError("GameStateManager instance not found in the scene.");
                }
            }
            return instance;
        }
    }

    public bool IsButtonClicked
    {
        get => gameConstants.IsButtonClicked;
        set
        {
            Debug.Log($"IsButtonClicked changed from {gameConstants.IsButtonClicked} to {value}");
            gameConstants.IsButtonClicked = value;
        }
    }

    public bool IsButton1Enabled
    {
        get => gameConstants.IsButton1Enabled;
        set
        {
            Debug.Log($"IsButton1Enabled changed from {gameConstants.IsButton1Enabled} to {value}");
            gameConstants.IsButton1Enabled = value;
        }
    }

    public bool IsButton2Enabled
    {
        get => gameConstants.IsButton2Enabled;
        set
        {
            Debug.Log($"IsButton2Enabled changed from {gameConstants.IsButton2Enabled} to {value}");
            gameConstants.IsButton2Enabled = value;
        }
    }

    public bool IsButtonPressed
    {
        get => gameConstants.IsButtonPressed;
        set
        {
            Debug.Log($"IsButtonPressed changed from {gameConstants.IsButtonPressed} to {value}");
            gameConstants.IsButtonPressed = value;
        }
    }

    public bool IsRedCard
    {
        get => gameConstants.IsRedCard;
        set
        {
            Debug.Log($"IsButtonPressed changed from {gameConstants.IsRedCard} to {value}");
            gameConstants.IsRedCard = value;
        }
    }

    public bool[] IsStudents => gameConstants.IsStudents;

    

    public bool IsStudentLock
    {
        get => gameConstants.IsStudentLock;
        set
        {
            Debug.Log($"IsStudentLock changed from {gameConstants.IsStudentLock} to {value}");
            gameConstants.IsStudentLock = value;
        }
    }

    public bool IsObjectAllowed
    {
        get => gameConstants.IsObjectAllowed;
        set
        {
            Debug.Log($"IsObjectAllowed changed from {gameConstants.IsObjectAllowed} to {value}");
            gameConstants.IsObjectAllowed = value;
        }
    }

    public bool IsClear
    {
        get => gameConstants.IsClear;
        set
        {
            Debug.Log($"IsClear changed from {gameConstants.IsClear} to {value}");
            gameConstants.IsClear = value;
        }
    }

    public void SetAllStudentsFalse()
    {
        Debug.Log("SetAllStudentsFalse called");
        for (int i = 0; i < IsStudents.Length; i++)
        {
            IsStudents[i] = false;
            Debug.Log($"IsStudents[{i}] set to false");
        }
        IsStudentLock = false;
        IsObjectAllowed = false;
        IsClear = false;
        IsButtonPressed = false;
        IsButton1Enabled = false;
        IsButton2Enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStateManager : MonoBehaviour
{
    public enum TutorialPhase
    {
        Introduction,
        ControlsExplanation,
        StudentIntroduction,
        StudentExplanation,
        RedStudentExplanation,
        PostResponseExplanation,
        PurpleStudentExplanation,
        WhiteStudentExplanation,
        FinalIntroduction,
        TutorialComplete
    }

    // 現在のフェーズを保持するプロパティ
    public TutorialPhase CurrentPhase { get; private set; }

    // フェーズが変更されたときに発生するイベント
    public event System.Action<TutorialPhase> OnPhaseChanged;

    public TutorialStateManager()
    {
        CurrentPhase = TutorialPhase.Introduction;
    }

    // フェーズを変更するメソッド
    public void ChangePhase(TutorialPhase newPhase)
    {
        if (CurrentPhase != newPhase)
        {
            CurrentPhase = newPhase;
            OnPhaseChanged?.Invoke(CurrentPhase);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TutorialStateManager;

public class TutorialManager : MonoBehaviour
{
    private int actionCount = 0;
    private int requiredActions = 3;
    private TutorialStateManager stateManager;

    void Start()
    {
        // 状態管理クラスのインスタンスを作成
        stateManager = new TutorialStateManager();
        stateManager.OnPhaseChanged += HandlePhaseChanged;

        // 初期フェーズを開始
        StartPhase(stateManager.CurrentPhase);
    }

    void HandlePhaseChanged(TutorialPhase newPhase)
    {
        Debug.Log("Phase changed to: " + newPhase.ToString());
        StartPhase(newPhase);
    }

    void StartPhase(TutorialPhase phase)
    {
        switch (phase)
        {
            case TutorialPhase.Introduction:
                // Introductionフェーズの処理
                // フェーズの完了後、次のフェーズに移行
                // stateManager.ChangePhase(TutorialPhase.Movement);
                break;

            case TutorialPhase.ControlsExplanation:
                // ControlsExplanationフェーズの処理
                break;

            case TutorialPhase.StudentIntroduction:
                // StudentIntroductionフェーズの処理
                break;

            case TutorialPhase.StudentExplanation:
                // StudentExplanationフェーズの処理
                break;

            case TutorialPhase.RedStudentExplanation:
                // RedStudentExplanationフェーズの処理
                break;
            case TutorialPhase.PostResponseExplanation:
                // PostResponseExplanationフェーズの処理
                break;
            case TutorialPhase.PurpleStudentExplanation:
                // PurpleStudentExplanationフェーズの処理
                break;
            case TutorialPhase.WhiteStudentExplanation:
                // PurpleStudentExplanationフェーズの処理
                break;
            case TutorialPhase.FinalIntroduction:
                // FinalIntroductionフェーズの処理
                break;
            case TutorialPhase.TutorialComplete:
                // TutorialCompleteフェーズの処理
                break;
        }
    }

    // フェーズ変更を行うためのメソッド
    public void NextPhase()
    {
        switch (stateManager.CurrentPhase)
        {
            case TutorialPhase.Introduction:
                stateManager.ChangePhase(TutorialPhase.ControlsExplanation);
                break;

            case TutorialPhase.ControlsExplanation:
                stateManager.ChangePhase(TutorialPhase.StudentIntroduction);
                break;

            case TutorialPhase.StudentIntroduction:
                stateManager.ChangePhase(TutorialPhase.StudentExplanation);
                break;

            case TutorialPhase.StudentExplanation:
                stateManager.ChangePhase(TutorialPhase.RedStudentExplanation);
                break;
        }
    }
}

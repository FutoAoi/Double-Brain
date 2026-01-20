using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/*
 * シーンネームを入れてシーン移行できるように
 * UIのボタンで動く
 * S、Q、Eキーでそれぞれシーン移行
 */

public class SceneChange : MonoBehaviour
{
    //シーン移行時のボタンは自由に設定可能
    [SerializeField] string _nextSceneName;
    [SerializeField] string _backSceneName;

    //特定キーが押されたら実行
    void Update()
    {
        //S
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            NextStage();
        }
        //Q
        else if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            BackScene();
        }
        //E
        else if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            EndGame();
        }
    }

    //次シーン移行
    public void NextStage()
    {
        if (!string.IsNullOrEmpty(_nextSceneName))
        {
            SceneManager.LoadScene(_nextSceneName);
        }
    }

    //前シーン移行
    public void BackScene()
    {
        if (!string.IsNullOrEmpty(_backSceneName))
        {
            SceneManager.LoadScene(_backSceneName);
        }
    }

    //ゲーム終了(強制終了)
    public void EndGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    //シーン移行時のボタンは自由に設定可能
    [SerializeField] string _nextSceneName;
    [SerializeField] string _backSceneName;

    //次シーン移行
    public void Next()
    {
        if (!string.IsNullOrEmpty(_nextSceneName))
        {
            SceneManager.LoadScene(_nextSceneName);
        }
    }

    //前シーン移行
    public void Back()
    {
        if (!string.IsNullOrEmpty(_backSceneName))
        {
            SceneManager.LoadScene(_backSceneName);
        }
    }

    //ゲーム終了
    public void End()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}

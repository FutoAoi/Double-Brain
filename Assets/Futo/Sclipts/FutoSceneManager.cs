using UnityEngine;
using UnityEngine.SceneManagement;
public enum SceneType
{
    None,
    Title,
    InGame,
    GameOver,
    GameClear
}

public class FutoSceneManager : MonoBehaviour
{
    [SerializeField] private SceneType _thisScene;
    private AudioManager _audioManager;
    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _audioManager.StopBGM();
        switch (_thisScene)
        {
            case SceneType.Title:
                _audioManager.PlayBGM("Title");
                break;
            case SceneType.InGame:
                _audioManager.PlayBGM("InGame");
                break;
            case SceneType.GameOver:
                _audioManager.PlayBGM("GameOver");
                break;
            case SceneType.GameClear:
                _audioManager.PlayBGM("GameClear");
                break;
        }
    }

    public void SceneChange(int number)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(number);
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }   
}
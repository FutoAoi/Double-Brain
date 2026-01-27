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
        //_audioManager = AudioManager.Instance;
        //_audioManager.StopBGM();
        //switch (_thisScene)
        //{
        //    case SceneType.Title:
        //        _audioManager.PlayBGM("Title");
        //        break;
        //    case SceneType.InGame:
        //        break;
        //    case SceneType.GameOver:
        //        break;
        //    case SceneType.GameClear:
        //        break;
        //}
    }
    public void SceneChange(int number)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(number);
    }
    public void EndGame()
    {
        //Escが押された時
        if (Input.GetKey(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
    }
}

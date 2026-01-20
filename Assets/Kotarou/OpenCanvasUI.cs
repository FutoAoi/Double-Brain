using UnityEngine;
using UnityEngine.InputSystem;

/*
 * クリックとキーボード操作で動くように
 * キーボードの場合SPACEで開き、ESCAPEで閉じる
 */

public class OpenCanvasUI : MonoBehaviour
{
    [SerializeField] GameObject panel; //表示、非表示させるパネル(オブジェクト)

    void Start()
    {
        panel.SetActive(false);
    }

    //特定キーが押されたらパネルを表示、非表示
    void Update()
    {
        //スペース
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ShowPanel();
        }
        //エスケープ
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HidePanel();
        }
    }

    //開く
    public void ShowPanel()
    {
        panel.SetActive(true);
    }

    //閉じる
    public void HidePanel()
    {
        panel.SetActive(false);
    }

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }

    void OnMouseDown()
    {
        TogglePanel();
    }
}

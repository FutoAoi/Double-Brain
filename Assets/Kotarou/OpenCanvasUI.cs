using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// オブジェクト、ボタンクリックでキャンバス表示
/// スペースによる表示、エスケープによる非表示
/// </summary>

public class OpenCanvasUI : MonoBehaviour
{
    [SerializeField] GameObject panel;

    void Start()
    {
        panel.SetActive(false);
    }

    //特定キーが押されたらパネルを表示、非表示
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ShowPanel();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HidePanel();
        }
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
    }

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

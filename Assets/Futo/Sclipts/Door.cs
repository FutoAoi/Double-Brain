using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private List<Switch> _flagGimmicks;
    [SerializeField] private bool _noClose = false;
    [SerializeField] private Transform _openTransform;

    private Vector3 _closePosition;
    private Transform _transform;
    private AudioManager _audioManager;

    void Start()
    {
        _closePosition = transform.position;
        _transform = GetComponent<Transform>();
        _audioManager = AudioManager.Instance;
        foreach (var gimmick in _flagGimmicks)
        {
            gimmick.OnActiveChanged += CheckActiveGimmick;
        }
    }

    public void Open()
    {
        _transform.position = _openTransform.position;
        _audioManager.PlaySe("Door");

    }

    public void Close()
    {
        _transform.position = _closePosition;
    }

    private void CheckActiveGimmick(bool active)
    {
        bool flag = true;
        foreach(var gimmick in _flagGimmicks)
        {
            flag = gimmick.IsActiveGimmick;

            if (!flag)
            {
                if(!_noClose)
                {
                    Close();
                }
                return;
            }
        }
        Open();
    }
}
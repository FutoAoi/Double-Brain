using System;
using UnityEngine;

public class Lever : MonoBehaviour, IGimmick
{
    [SerializeField] private Transform _rotateTarget; // 動かすオブジェクト
    [SerializeField] private float _onX = 50f;      // ONのX座標
    [SerializeField] private float _offX = -50f;

    private bool _isActiveGimmick = false;
    public bool IsActiveGimmick => _isActiveGimmick;
    public event Action<bool> OnActiveChanged;

    private AudioManager _audioManager;
    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }
    public void OffGimmick()
    {
        _isActiveGimmick = false;
        UpdateTargetRotation();
        OnActiveChanged?.Invoke(false);
    }

    public void OnGimmick()
    {
        _isActiveGimmick = true;
        UpdateTargetRotation();
        OnActiveChanged?.Invoke(true);
        _audioManager.PlaySe("Lever");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if(!_isActiveGimmick)
            {
                OnGimmick();
            }
            else
            {
                OffGimmick();
            }
        }
    }

    void UpdateTargetRotation()
    {
        if (_rotateTarget == null) return;

        Vector3 rot = _rotateTarget.eulerAngles;

        rot.x = _isActiveGimmick ? _onX : _offX;

        _rotateTarget.rotation = Quaternion.Euler(rot);
    }
}

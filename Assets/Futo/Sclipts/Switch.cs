using System;
using UnityEngine;

public class Switch : MonoBehaviour, IGimmick
{
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
        OnActiveChanged?.Invoke(false);
    }

    public void OnGimmick()
    {
        _isActiveGimmick = true;
        OnActiveChanged?.Invoke(true);
        _audioManager.PlaySe("Swich");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnGimmick();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OffGimmick();
        }
    }
}

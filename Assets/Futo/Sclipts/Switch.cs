using System;
using UnityEngine;

public class Switch : MonoBehaviour, IGimmick
{
    private bool _isActiveGimmick = false;
    public bool IsActiveGimmick => _isActiveGimmick;
    public event Action<bool> OnActiveChanged;

    public void OffGimmick()
    {
        _isActiveGimmick = false;
        OnActiveChanged?.Invoke(false);
    }

    public void OnGimmick()
    {
        _isActiveGimmick = true;
        OnActiveChanged?.Invoke(true);
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

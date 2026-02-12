using System;
using UnityEngine;

public class Switch : MonoBehaviour, IGimmick
{
    [SerializeField] private Renderer _targetRenderer;
    [SerializeField] private Color _pressColor = Color.red;
    private bool _isActiveGimmick = false;
    public bool IsActiveGimmick => _isActiveGimmick;
    public event Action<bool> OnActiveChanged;
    private AudioManager _audioManager;
    private Color _defaultColor;

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _defaultColor = _targetRenderer.material.color;
    }

    public void OffGimmick()
    {
        _isActiveGimmick = false;
        _targetRenderer.material.color = _defaultColor;
        OnActiveChanged?.Invoke(false);
    }

    public void OnGimmick()
    {
        _isActiveGimmick = true;
        _targetRenderer.material.color = _pressColor;
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

using System;

public interface IGimmick
{
    public bool IsActiveGimmick{  get; }
    public event Action<bool> OnActiveChanged;
    public void OnGimmick();
    public void OffGimmick();
}
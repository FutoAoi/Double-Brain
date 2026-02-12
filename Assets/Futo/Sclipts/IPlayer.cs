using UnityEngine;

public interface IPlayer
{
    public GameObject HaveItem { get; }
    public void GetItem(GameObject item);
    public void UseItem();
}

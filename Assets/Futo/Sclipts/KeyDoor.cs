using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Key"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}

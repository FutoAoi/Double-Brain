using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Key"))
        {
            _audioManager.PlaySe("KeyDoor");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}

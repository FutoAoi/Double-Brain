using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private AudioManager _audio;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _audio = AudioManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("destroy"))
        {
            _audio.PlaySe("Rock");
            other.gameObject.SetActive(false);
        }
    }
}

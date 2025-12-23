using System.Collections;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] private float _pickupDisableTime = 0.5f;
    private Rigidbody _rb;
    private Collider _col;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IPlayer player = other.gameObject.GetComponent<IPlayer>();
            if (player.HaveItem == null)
            {
                player.GetItem(this.gameObject);
                _rb.isKinematic = true;
            }
        }
    }

    public void Throw(Vector3 direction, float power)
    {
        transform.parent = null;

        _rb.isKinematic = false;

        _col.enabled = false;

        _rb.AddForce(direction * power, ForceMode.Impulse);

        StartCoroutine(EnableColliderAfterTime());
    }
    IEnumerator EnableColliderAfterTime()
    {
        yield return new WaitForSeconds(_pickupDisableTime);

        _col.enabled = true;
    }
}

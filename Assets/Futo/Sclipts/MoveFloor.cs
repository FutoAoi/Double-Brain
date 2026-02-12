using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    [SerializeField] private List<GameObject> _flagGimmicks;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _waitTime = 1.0f;

    private float _t = 0f;
    private bool _goToB = true;

    private Transform _transform;
    private AudioManager _audioManager;
    private bool _isMove;
    private bool _isWaiting = false;
    private float _waitTimer = 0f;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _audioManager = AudioManager.Instance;
        foreach (var gimmick in _flagGimmicks)
        {
            gimmick.GetComponent<IGimmick>().OnActiveChanged += CheckActiveGimmick;
        }
    }

    public void Update()
    {
        if(_isMove)
        {
            Move();
        }
    }

    private void Move()
    {
        if (_isWaiting)
        {
            _waitTimer += Time.deltaTime;

            if (_waitTimer >= _waitTime)
            {
                _waitTimer = 0f;
                _isWaiting = false;

                _goToB = !_goToB;
                _t = 0f;
            }
            return;
        }

        _t += Time.deltaTime * _moveSpeed;

        if (_goToB)
        {
            _transform.position = Vector3.Lerp(_pointA.position, _pointB.position, _t);
        }
        else
        {
            _transform.position = Vector3.Lerp(_pointB.position, _pointA.position, _t);
        }

        if (_t >= 1f)
        {
            _t = 1f;
            _isWaiting = true;
        }
    }

    private void CheckActiveGimmick(bool active)
    {
        bool flag = true;
        foreach (var gimmick in _flagGimmicks)
        {
            flag = gimmick.GetComponent<IGimmick>().IsActiveGimmick;

            if (!flag)
            {
                _isMove = false;
                return;
            }
            _isMove = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.SetParent(transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.SetParent(null);
    }
}

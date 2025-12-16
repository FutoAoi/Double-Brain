using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClickPlayer : MonoBehaviour, ICharacter
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private string _walkablePosition;
    [SerializeField] private LayerMask _obstacleLayer;

    private Vector3 _targetPosition;
    private Vector2 _mousePos;
    private Ray _ray;
    private RaycastHit _hit;
    private Rigidbody _rb;
    private Animator _playerAnimator;
    private Vector3 _previousPosition;

    public void CharacterSetup()
    {
        _rb = GetComponent<Rigidbody>();
        _playerAnimator = GetComponentInChildren<Animator>();
        _targetPosition = transform.position;
        _targetPosition = transform.position;
        _previousPosition = transform.position;
    }

    public void CharacterUpdate()
    {
        MouseInput();
    }

    public void FixedUpdate()
    {
        MouseMove();
        MouseRotate();
        MouseAnimator();
    }

    void MouseInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _mousePos = Mouse.current.position.ReadValue();

            _ray = Camera.main.ScreenPointToRay(_mousePos);

            if (Physics.Raycast(_ray, out _hit))
            {
                if (_hit.collider.CompareTag(_walkablePosition))
                {
                    _targetPosition = _hit.point;
                }
            }
        }
    }

    private void MouseRotate()
    {
        Vector3 direction = _targetPosition - _rb.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(direction);

        float rotateSpeed = 360f;

        Quaternion newRot = Quaternion.RotateTowards(
            _rb.rotation,
            targetRot,
            rotateSpeed * Time.fixedDeltaTime
        );

        _rb.MoveRotation(newRot);
    }

    private void MouseMove()
    {
        Vector3 direction = _targetPosition - _rb.position;

        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance < 0.01f)
            return;

        Ray ray = new Ray(_rb.position + Vector3.up * 0.5f, direction.normalized);

        if (Physics.Raycast(ray, distance, _obstacleLayer))
        {
            return;
        }
        _rb.MovePosition(
            Vector3.MoveTowards(
                _rb.position,
                _targetPosition,
                _moveSpeed * Time.fixedDeltaTime
            )
        );
    }

    void MouseAnimator()
    {
        Vector3 currentPosition = _rb.position;
        Vector3 delta = currentPosition - _previousPosition;
        float nowSpeed = delta.magnitude / Time.fixedDeltaTime;
        _playerAnimator.SetFloat("PlayerSpeed", nowSpeed / 15f);
        _previousPosition = currentPosition;
    }
}

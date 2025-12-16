using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class KeyBoardPlayer : MonoBehaviour,ICharacter
{
    [SerializeField] private float _playerHp;
    [SerializeField] private float _playerMoveSpeed;

    private Vector2 _moveInput;
    private Rigidbody _rb;
    private Animator _playerAnimator;
    private Transform _tf;
    private float _x, _z;
    private float _currentY;
    Quaternion _targetRot;
    Vector3 _moveVector;

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void CharacterSetup()
    {
        _rb = GetComponent<Rigidbody>();
        _tf = GetComponent<Transform>();
        _playerAnimator = GetComponentInChildren<Animator>();
    }

    public void CharacterUpdate()
    {
        _moveVector = new Vector3(_moveInput.x, 0, _moveInput.y);
    }

    public void FixedUpdate()
    {
        KeyBoardMove();
        KeyboardRotate();
        KeyBoardAnimator();
    }


    void KeyBoardMove()
    {
        _currentY = _rb.linearVelocity.y;

        Vector3 _velocity = _moveVector * _playerMoveSpeed;
        _velocity.y = _currentY;

        _rb.linearVelocity = _velocity;
    }

    void KeyboardRotate()
    {
        if (_moveInput.sqrMagnitude < 0.001f) return;
        _targetRot = Quaternion.LookRotation(_moveVector);
        float rotateSpeed = 360f;

        Quaternion newRot = Quaternion.RotateTowards(
            _rb.rotation,
            _targetRot,
            rotateSpeed * Time.fixedDeltaTime
        );
        _rb.MoveRotation(newRot);
    }

    void KeyBoardAnimator()
    {
        float nowSpeed = _rb.linearVelocity.magnitude;
        _playerAnimator.SetFloat("PlayerSpeed", nowSpeed / 15);
    }
}

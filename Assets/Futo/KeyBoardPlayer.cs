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
    private Vector3 _direction;
    private Quaternion _rotate;


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
        KeyBoardMove();
        KeyBoardAnimator();
    }

    void KeyBoardMove()
    {
        _x = _moveInput.x * _playerMoveSpeed;
        _z = _moveInput.y * _playerMoveSpeed;

        _currentY = _rb.linearVelocity.y;
        _direction = new Vector3(_x, 0, _z);

        if(_direction.magnitude > 0.01)
        {
            _rotate = Quaternion.LookRotation(_direction);
            _tf.rotation = _rotate;
        }

        _rb.linearVelocity = new Vector3(_x, _currentY, _z);
    }

    void KeyBoardAnimator()
    {
        float nowSpeed = _rb.linearVelocity.magnitude;
        _playerAnimator.SetFloat("PlayerSpeed", nowSpeed / 15);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClickPlayer : MonoBehaviour, ICharacter
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private string _walkablePosition;

    private Vector3 _targetPosition;
    private Vector2 _mousePos;
    private Ray _ray;
    private RaycastHit _hit;
    private Rigidbody _rb;
    private Animator _playerAnimator;

    public void CharacterSetup()
    {
        _rb = GetComponent<Rigidbody>();
        _targetPosition = transform.position;
    }

    public void CharacterUpdate()
    {
        MouseMove();
        //KeyBoardAnimator();
    }

    private void MouseMove()
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

        _rb.MovePosition(
            Vector3.MoveTowards(
                _rb.position,
                _targetPosition,
                _moveSpeed * Time.fixedDeltaTime
            )
        );
    }

    //void KeyBoardAnimator()
    //{
    //    float nowSpeed = _rb.linearVelocity.magnitude;
    //    _playerAnimator.SetFloat("PlayerSpeed", nowSpeed / 15);
    //}
}

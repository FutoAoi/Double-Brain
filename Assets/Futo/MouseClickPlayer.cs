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
        // 現在地とターゲット位置が同じ場合は回転しない
        Vector3 direction = _targetPosition - _rb.position;

        // 水平方向だけを使う（上下の傾きは無視）
        direction.y = 0f;

        // 方向の長さがほぼ 0 なら回転しない
        if (direction.sqrMagnitude < 0.0001f)
            return;

        // 向きたい回転を作る
        Quaternion targetRot = Quaternion.LookRotation(direction);

        // 今の回転から目標回転へゆっくり向ける（回転スピードは好きに調整）
        float rotateSpeed = 360f; // 1秒で1回転分（好みで変更可）

        Quaternion newRot = Quaternion.RotateTowards(
            _rb.rotation,       // 今の角度
            targetRot,          // 目標角度
            rotateSpeed * Time.fixedDeltaTime   // 1フレームの回転量
        );

        // Rigidbody に物理的に回転させる
        _rb.MoveRotation(newRot);
    }

    private void MouseMove()
    {
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

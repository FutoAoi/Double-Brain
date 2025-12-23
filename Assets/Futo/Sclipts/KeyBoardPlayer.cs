using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBoardPlayer : MonoBehaviour,ICharacter, IPlayer
{
    [SerializeField] private float _playerHp;
    [SerializeField] private float _playerMoveSpeed;
    [SerializeField] private Transform _itemTransform;
    [SerializeField] private GameObject _haveItem;
    [SerializeField] private float _throwPower = 20f;

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private int _linePointCount = 30;
    [SerializeField] private float _timeStep = 0.1f;

    public GameObject HaveItem => _haveItem;

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

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UseItem();
        }
    }

    public void CharacterSetup()
    {
        _rb = GetComponent<Rigidbody>();
        _tf = GetComponent<Transform>();
        _playerAnimator = GetComponentInChildren<Animator>();
        _lineRenderer = GetComponent<LineRenderer>();
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
        DrawThrowPrediction();
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

    public void GetItem(GameObject item)
    {
        _haveItem = item;
        item.transform.parent = _itemTransform;
        item.transform.localPosition = new Vector3(0,-3,0);
    }

    public void UseItem()
    {
        if (_haveItem == null) return;

        ItemBase item = _haveItem.GetComponent<ItemBase>();

        Vector3 throwDirection = transform.forward;

        item.Throw(throwDirection, _throwPower);

        _haveItem = null;
    }

    void DrawThrowPrediction()
    {
        // アイテムを持っていないなら表示しない
        if (_haveItem == null)
        {
            _lineRenderer.enabled = false;
            return;
        }

        _lineRenderer.enabled = true;

        // 投げ始めの位置
        Vector3 startPos = _itemTransform.position;

        // 投げる方向（前）
        Vector3 startVelocity = transform.forward * _throwPower;

        // 点の数を設定
        _lineRenderer.positionCount = _linePointCount;

        for (int i = 0; i < _linePointCount; i++)
        {
            float time = i * _timeStep;

            // 放物線の計算
            Vector3 position =
                startPos +
                startVelocity * time +
                Physics.gravity * (time * time) / 2f;

            _lineRenderer.SetPosition(i, position);
        }
    }
}

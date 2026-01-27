using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClickPlayer : MonoBehaviour, ICharacter, IPlayer
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private string _walkablePosition;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private Transform _itemTransform;
    [SerializeField] private GameObject _haveItem;
    [SerializeField] private float _throwPower = 20f;

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private int _linePointCount = 30;
    [SerializeField] private float _timeStep = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LineRenderer _aimLineRenderer;
    [SerializeField] private Texture2D _aimCursor;

    [SerializeField] private GameObject _targetMarkerPrefab; // マーカーのPrefab
    private GameObject _targetMarkerInstance;

    private AudioManager _audioManager;
    private Vector3 _targetPosition;
    private Vector2 _mousePos;
    private Ray _ray;
    private RaycastHit _hit;
    private Rigidbody _rb;
    private Animator _playerAnimator;
    private Vector3 _previousPosition;
    private bool _isAim = false;

    public GameObject HaveItem => _haveItem;

    public void CharacterSetup()
    {
        _rb = GetComponent<Rigidbody>();
        _playerAnimator = GetComponentInChildren<Animator>();
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _targetPosition = transform.position;
        _targetPosition = transform.position;
        _previousPosition = transform.position;
        _audioManager = AudioManager.Instance;
    }

    public void CharacterUpdate()
    {
        MouseInput();
    }

    public void FixedUpdate()
    {
        if (!_isAim)
        {
            MouseMove();
        }
        MouseRotate();
        MouseAnimator();
        DrawThrowPrediction();
        
    }

    void MouseInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_isAim)
            {
                ShootToMousePosition();
                return;
            }

            _mousePos = Mouse.current.position.ReadValue();

            _ray = Camera.main.ScreenPointToRay(_mousePos);

            if (Physics.Raycast(_ray, out _hit,float.MaxValue,_groundLayer))
            {
                if (_hit.collider.CompareTag(_walkablePosition))
                {
                    _targetPosition = _hit.point;

                    SetTargetMarker(_hit.point);
                }
            }
        }

        if(Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (_haveItem != null)
            {
                UseItem();
            }
            else
            {
                AimModeChange();
            }
        }
    }

    private void MouseRotate()
    {
        Vector3 direction;

        if (_isAim)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out _hit, float.MaxValue))
            {
                direction = _hit.point - _rb.position;

                _aimLineRenderer.enabled = true;
                _aimLineRenderer.SetPosition(0, _rb.position + (Vector3.up * 3f));
                _aimLineRenderer.SetPosition(1, _hit.point);
            }
            else
            {
                _aimLineRenderer.enabled = false;
                return;
            }
        }
        else
        {
            _aimLineRenderer.enabled = false;
            direction = _targetPosition - _rb.position;
        }

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(direction);

        _rb.MoveRotation(
            Quaternion.RotateTowards(
                _rb.rotation,
                targetRot,
                360f * Time.fixedDeltaTime
            )
        );
    }

    void SetTargetMarker(Vector3 position)
    {
        // まだマーカーが無ければ生成
        if (_targetMarkerInstance == null)
        {
            _targetMarkerInstance = Instantiate(
                _targetMarkerPrefab, // Prefab
                position,            // 位置
                Quaternion.identity  // 回転なし
            );
        }
        else
        {
            // 既にあれば位置だけ移動
            _targetMarkerInstance.transform.position = position;
        }
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

    public void GetItem(GameObject item)
    {
        _haveItem = item;
        item.transform.parent = _itemTransform;
        item.transform.localPosition = new Vector3(0, -3, 0);
    }

    public void UseItem()
    {
        if (_haveItem == null) return;

        ItemBase item = _haveItem.GetComponent<ItemBase>();

        Vector3 throwDirection = transform.forward;

        item.Throw(throwDirection, _throwPower);

        _haveItem = null;
    }

    private void AimModeChange()
    {
        _isAim = !_isAim;
        if (_isAim)
        {
            Cursor.SetCursor(_aimCursor, new Vector2(10,10), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);
        }
        _playerAnimator.SetBool("Aim", _isAim);
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
    private void ShootToMousePosition()
    {
        _audioManager.PlaySe("Shoot");
    }
}

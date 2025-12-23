using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour,ICharacter
{
    private Collider _collider;
    private KeyBoardPlayer _keyBoardPlayer;
    private MouseClickPlayer _mouseClickPlayer;

    private Rigidbody _rb;

    [Header("攻撃設定")]
    [SerializeField] private GameObject _projectilePrefab; // 弾のプレハブ
    [SerializeField] private float _attackRange = 10f; // 攻撃が届く範囲
    [SerializeField] private float _attackSpaceRange = 2f; // 攻撃の間隔（秒）
    private float _attackTimer; // 攻撃用タイマー
    public float speed = 3.0f;


    private void Awake()
    {
        CharacterSetup();

        _rb = GetComponent<Rigidbody>();
    }

    public void CharacterSetup()
    {
        _collider = GetComponent<BoxCollider>();

        _keyBoardPlayer = Object.FindAnyObjectByType<KeyBoardPlayer>();

        _mouseClickPlayer = Object.FindAnyObjectByType<MouseClickPlayer>();

    }

    public void CharacterUpdate()
    {
        // プレイヤーが見つかっていない場合は何もしない（エラー防止）
        if (_keyBoardPlayer == null) return;
        {
            // ターゲットとの距離を測る
            float distance = Vector3.Distance(transform.position, _keyBoardPlayer.transform.position);
            // 攻撃範囲内にいるかチェック
            if (distance <= _attackRange)
            {
                // 攻撃間隔（タイマー）の管理
                _attackTimer -= Time.deltaTime;
                if (_attackTimer <= 0)
                {
                    Attack(); // 遠距離攻撃実行
                    // タイマーをリセット
                    _attackTimer = _attackSpaceRange; 
                }
            }
        }
    }

    private void Attack()
    {
        Debug.Log("プレイヤーに向かって遠距離攻撃！");

        if (_projectilePrefab != null)
        {
            // プレイヤーへの方向を計算
            Vector3 direction = (_keyBoardPlayer.transform.position - transform.position).normalized;

            _rb.linearVelocity = new Vector3(direction.x, 0, direction.z);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        _keyBoardPlayer = Object.FindAnyObjectByType<KeyBoardPlayer>();

        // keyBoardPlayerがあったら以下の内容を実行する
        if (_keyBoardPlayer != null)
        {
            Debug.Log("見つけたプレイヤー: " + _keyBoardPlayer.gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CharacterUpdate();
    }
}

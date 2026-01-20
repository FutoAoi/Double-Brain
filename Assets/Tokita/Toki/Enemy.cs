using UnityEngine;


public class Enemy : MonoBehaviour,ICharacter
{
    private Collider _collider;
    


    

    private Rigidbody _rb;

    [Header("攻撃設定")]
    [SerializeField] private GameObject[] _targetPrefab; // 体当たりされるターゲットのプレハブ
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

    }

    public void CharacterUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _TargetDamage();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _TargetDamage()
    {
        Debug.Log("プレイヤー検知");

        // ターゲットとの距離を測る
        float distance0 = Vector3.Distance(transform.position, _targetPrefab[0].transform.position);
        // 攻撃範囲内にいるかチェック
        if (distance0 <= _attackRange)
        {
            // 攻撃間隔（タイマー）の管理
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0)
            {
                _Damage(); // 体当たり攻撃実行
                          // タイマーをリセット
                _attackTimer = _attackSpaceRange;
            }
        }

        // ターゲットとの距離を測る
        float distance1 = Vector3.Distance(transform.position, _targetPrefab[1].transform.position);
        // 攻撃範囲内にいるかチェック
        if (distance1 <= _attackRange)
        {
            // 攻撃間隔（タイマー）の管理
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0)
            {
                _Damage(); // 体当たり攻撃実行
                          // タイマーをリセット
                _attackTimer = _attackSpaceRange;
            }
        }

        float Distance = distance0 + distance1;
    }

    private void _Damage()
    {
        // プレイヤー1への方向を計算
        Vector3 direction0 = (_targetPrefab[0].transform.position - transform.position).normalized;

        _rb.linearVelocity = new Vector3(direction0.x, 0, direction0.z);

        

        if (_targetPrefab[0] != null)
        {
            Destroy(_targetPrefab[0]);

            Debug.Log("プレイヤー1撃破");
        }

        // プレイヤー2への方向を計算
        Vector3 direction1 = (_targetPrefab[1].transform.position - transform.position).normalized;

        _rb.linearVelocity = new Vector3(direction1.x, 0, direction1.z);

        if (_targetPrefab[1] != null)
        {
            Destroy(_targetPrefab[1]);

            Debug.Log("プレイヤー2撃破");
        }
    }
}

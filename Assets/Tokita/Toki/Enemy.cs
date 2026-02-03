using UnityEngine;

public class Enemy : MonoBehaviour, ICharacter
{
    public Transform[] _Players;     // プレイヤーの配列
    [SerializeField] private float _speed = 5f;  // 移動速度
    [SerializeField] private float searchRange = 10f; // 索敵範囲（この距離に入ったら追いかける）
    [SerializeField] private Animator _anim;
    private bool isWalk;

    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        MoveTowardsNearestPlayer();
    }

    private void MoveTowardsNearestPlayer()
    {
        Transform target = GetNearestPlayer();

        // 追いかける対象がいない場合は何もしない
        if (target == null) return;
        {
            _anim.SetBool("Walk", false);
        }
        

        // 現在位置とターゲットの距離計算
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > searchRange) return;
        {
            _anim.SetBool("Walk", true);
        }
        

        // 対象に向かって移動
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;

        // オプション: 進行方向を向かせる
        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }

    // 生きているプレイヤーの中で一番近い人を探す
    private Transform GetNearestPlayer()
    {
        Transform nearest = null;
        float minDistance = float.MaxValue;

        foreach (var p in _Players)
        {
            if (p == null) continue; // 破壊済みの場合はスキップ

            float dist = Vector3.Distance(transform.position, p.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = p;
            }
        }
        return nearest;
    }

    // 接触した瞬間の処理
    private void OnCollisionEnter(Collision collision)
    {
        // 接触相手のタグを確認 (Player1, Player2両方に対応)
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{collision.gameObject.name} はワンパンでやられた！");

            

            // 相手を破壊
            Destroy(collision.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }

    public void CharacterSetup() { }
    public void CharacterUpdate() { }
}
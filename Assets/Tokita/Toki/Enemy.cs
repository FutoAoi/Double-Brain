using UnityEngine;

public class Enemy : MonoBehaviour, ICharacter
{
    public Transform[] _Players;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float searchRange = 10f;
    [SerializeField] private float attackRange = 2f;

    Animator anim;
    bool Attack = false;
    bool isDie = false;

    public GameObject _MouseClickPlayer;
    public GameObject _KeyboardPlayer;
    public GameObject _EnemyPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CharacterSetup()
    {
        anim = GetComponentInChildren<Animator>();
    }
    public void CharacterUpdate()
    {
        if (isDie) return;

        MoveTowardsNearestPlayer();
    }

    void MoveTowardsNearestPlayer()
    {
        Transform target = GetNearestPlayer();
        if (target == null)
        {
            anim.SetFloat("Speed", 0);
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        // 攻撃
        if (distance <= attackRange)
        {
            Attack = true;
            anim.SetBool("Attack", true);
            anim.SetFloat("Speed", 0);
            return; // ★移動しない

            //MouseClickPlayer script = _MouseClickPlayer.GetComponent<MouseClickPlayer>();

            //script.Hit();

            //KeyBoardPlayer Script = _KeyboardPlayer.GetComponent<KeyBoardPlayer>();

            //Script.Hit();
        }

        // 追跡
        if (distance <= searchRange)
        {
            anim.SetFloat("Speed", 1);

            Vector3 dir = (target.position - transform.position).normalized;
            transform.position += dir * _speed * Time.deltaTime;
            transform.forward = dir;
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }

    Transform GetNearestPlayer()
    {
        Transform nearest = null;
        float min = float.MaxValue;

        foreach (var p in _Players)
        {
            if (!p) continue;

            float d = Vector3.Distance(transform.position, p.position);
            if (d < min)
            {
                min = d;
                nearest = p;
            }
        }
        return nearest;
    }

    // アニメイベントから呼ぶ
    public void EndAttack()
    {
        Attack = false;
        anim.SetBool("Attack", false);
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}

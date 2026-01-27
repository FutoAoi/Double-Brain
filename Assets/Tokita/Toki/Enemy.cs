using UnityEngine;



public class Enemy : MonoBehaviour,ICharacter
{
    public Transform[] player;       // プレイヤーのTransform
    public float speed = 5f;       // 移動速度
    public float stopDistance = 1f; // 接触判定距離

    


    private void Awake()
    {
        CharacterSetup();

        
    }

    public void CharacterSetup()
    {
        

    }

    public void CharacterUpdate()
    {
        
    }

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player[0] == null) return;

        // プレイヤー1の方向へ移動
        Vector3 direction1 = (player[0].position - transform.position).normalized;
        transform.position += direction1 * speed * Time.deltaTime;

        // プレイヤー1との距離が近ければ攻撃
        if (Vector3.Distance(transform.position, player[0].position) <= stopDistance)
        {
            Destroy(player[0]);
            OnePunch();
        }

        if (player[1] == null) return;
        // プレイヤー2の方向へ移動
        Vector3 direction2 = (player[1].position - transform.position).normalized;
        transform.position += direction2 * speed * Time.deltaTime;

        // プレイヤー2との距離が近ければ攻撃
        if (Vector3.Distance(transform.position, player[1].position) <= stopDistance)
        {
            Destroy (player[1]);
            OnePunch();
        }

    }

    void OnePunch()
    {
        Debug.Log("プレイヤーはワンパンでやられた！");
        // ここでゲームオーバー処理を呼び出す
        // 例: GameManager.Instance.GameOver();
    }
}

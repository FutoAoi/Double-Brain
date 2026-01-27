using UnityEngine;



public class Enemy : MonoBehaviour,ICharacter
{
    public Transform player;       // プレイヤーのTransform
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
        if (player == null) return;

        // プレイヤー方向へ移動
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // プレイヤーとの距離が近ければ攻撃
        if (Vector3.Distance(transform.position, player.position) <= stopDistance)
        {
            Destroy(player);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AudioManagerクラス: BGMとSEの再生を管理するシングルトン
public class AudioManager : MonoBehaviour
{
    // シングルトンのインスタンス。外部からアクセス可能（読み取り専用）
    public static AudioManager Instance { get; private set; }

    // サウンドデータ（BGMやSEのクリップ情報など）を保持するScriptableObject
    [SerializeField]
    private SoundDataBaseSo _soundDataBase;

    // BGM再生に使用するAudioSource (通常は常設の単一ソース)
    [SerializeField]
    private AudioSource _bgmSource;

    // BGM用のAudioSourceをまとめるルートTransform (階層を整理するため)
    [SerializeField]
    private Transform _bgmRoot;

    // BGM用AudioSourceの初期プールサイズ
    [SerializeField]
    private int _bgmRootSize = 10;

    // SE用のAudioSourceをまとめるルートTransform
    [SerializeField]
    private Transform _seRoot;

    // SE用AudioSourceの初期プールサイズ
    [SerializeField]
    private int _seRootSize = 10;

    // BGM用AudioSourceのオブジェクトプール（未使用時に待機）
    private readonly Queue<AudioSource> _bgmAudioSourcePools = new Queue<AudioSource>();

    // SE用AudioSourceのオブジェクトプール
    private readonly Queue<AudioSource> _seAudioSourcePools = new Queue<AudioSource>();


    // コンポーネントがロードされた直後に一度だけ呼ばれる
    private void Awake()
    {
        // 既にインスタンスが存在する場合（二重起動防止）
        if (Instance != null)
        {
            Destroy(gameObject); // このオブジェクトを破棄
            return;
        }

        // インスタンスを自身に設定し、シーンをまたいで破棄されないようにする
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 最初のフレーム更新の直前に呼ばれる
    void Start()
    {
        // BGM用のルートTransformが設定されていない場合、自身のTransformを使用
        if (_bgmRoot == null)
        {
            _bgmRoot = transform;
        }

        // BGM用AudioSourceを初期プールサイズ分だけ生成
        // （ただし、この実装ではBGM再生には_bgmSourceのみを使用し、プールは使われていない模様）
        for (int i = 0; i < _bgmRootSize; i++)
        {
            // AudioSourceコンポーネントを持つ新しいGameObjectを生成
            var instance = new GameObject("bgmAudioSource_" + i, typeof(AudioSource));
            instance.transform.SetParent(_bgmRoot); // ルート下に設定
            instance.gameObject.SetActive(false); // 非アクティブにして待機
            _bgmAudioSourcePools.Enqueue(instance.GetComponent<AudioSource>()); // プールに追加
        }

        // SE用のルートTransformが設定されていない場合、自身のTransformを使用
        if (_seRoot == null)
        {
            _seRoot = transform;
        }

        // SE用AudioSourceを初期プールサイズ分だけ生成し、プールに追加
        for (int i = 0; i < _seRootSize; i++)
        {
            var instance = new GameObject("seAudioSource_" + i, typeof(AudioSource));
            instance.transform.SetParent(_seRoot);
            instance.gameObject.SetActive(false);
            _seAudioSourcePools.Enqueue(instance.GetComponent<AudioSource>());
        }
    }

    // --- BGM再生関連 ---

    // int型のIDを受け取ってBGMを再生
    public void PlayBGM(int id)
    {
        StopBGM(); // 現在再生中のBGMを停止

        // IDに対応するBGMデータをデータベースから取得
        var soundData = _soundDataBase.GetBGMData(id);

        if (soundData == null)
        {
            Debug.LogWarning("Sound Data not found for ID: " + id);
            return;
        }

        // BGM再生用のAudioSource（_bgmSource）をサウンドデータで準備（クリップ設定、音量など）
        // ※ PrepareAudioSource は、どこか別の場所で定義された拡張メソッド（Extension Method）と推測される
        _bgmSource.PrepareAudioSource(soundData);

        _bgmSource.Play(); // BGM再生開始
    }

    // BGMを停止
    public void StopBGM()
    {
        if (_bgmSource.isPlaying)
        {
            _bgmSource.Stop();
        }
    }

    // --- SE再生関連 ---

    // enumのID（SEID）を受け取ってSEを再生
    public void PlaySe(SEID id)
    {
        PlaySe((int)id);
    }

    // int型のIDを受け取ってSEを再生
    public void PlaySe(int id)
    {
        // IDに対応するSEデータをデータベースから取得
        var soundData = _soundDataBase.GetSEData(id);
        if (soundData == null)
        {
            Debug.LogWarning("Sound Data not found for ID: " + id);
            return;
        }

        AudioSource seAudioSource = default;

        // **オブジェクトプールからの取得**
        // プールからAudioSourceを取り出そうと試みる
        if (_seAudioSourcePools.TryDequeue(out AudioSource source))
        {
            seAudioSource = source; // 取得成功
        }
        else
        {
            // プールが枯渇した場合、新しいAudioSourceを生成（動的な拡張）
            seAudioSource = new GameObject("seAudioSource_" + "NewInstance", typeof(AudioSource)).GetComponent<AudioSource>();
            seAudioSource.transform.SetParent(_seRoot); // ルート下に置く
        }

        // 取得したAudioSourceをサウンドデータで準備
        seAudioSource.PrepareAudioSource(soundData);
        seAudioSource.gameObject.SetActive(true); // アクティブにして
        seAudioSource.Play(); // SE再生開始

        // 再生終了後、AudioSourceをプールに戻すコルーチンを開始
        StartCoroutine(ReturnToPoolAfterPlaying(seAudioSource));
    }

    // AudioSourceの再生終了後にプールに戻す処理（コルーチン）
    private IEnumerator ReturnToPoolAfterPlaying(AudioSource source)
    {
        // isPlayingがfalseになる（再生が終了する）まで待機
        yield return new WaitWhile(() => source.isPlaying);

        // 待機後、AudioSourceを非アクティブにしてプールに戻す
        source.gameObject.SetActive(false);
        _seAudioSourcePools.Enqueue(source);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private SoundDataBaseSo _soundDataBase;

    [SerializeField]
    private AudioSource _bgmSource;

    [SerializeField]
    private Transform _bgmRoot;

    [SerializeField]
    private int _bgmRootSize = 10;

    [SerializeField]
    private Transform _seRoot;

    [SerializeField]
    private int _seRootSize = 10;

    private readonly Queue<AudioSource> _bgmAudioSourcePools = new Queue<AudioSource>();

    private readonly Queue<AudioSource> _seAudioSourcePools = new Queue<AudioSource>();


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_bgmRoot == null)
        {
            _bgmRoot = transform;
        }

        for (int i = 0; i < _bgmRootSize; i++)
        {
            var instance = new GameObject("bgmAudioSource_" + i, typeof(AudioSource));
            instance.transform.SetParent(_bgmRoot);
            instance.gameObject.SetActive(false);
            _bgmAudioSourcePools.Enqueue(instance.GetComponent<AudioSource>());
        }

        if (_seRoot == null)
        {
            _seRoot = transform;
        }

        for (int i = 0; i < _seRootSize; i++)
        {
            var instance = new GameObject("seAudioSource_" + i, typeof(AudioSource));
            instance.transform.SetParent(_seRoot);
            instance.gameObject.SetActive(false);
            _seAudioSourcePools.Enqueue(instance.GetComponent<AudioSource>());
        }
    }

    // string keyからint idに変更 -> enumを受け取るようにオーバーロード (または修正)
    public void PlayBGM(BGMID id)
    {
        PlayBGM((int)id);
    }

    // string keyからint idに変更
    public void PlayBGM(int id)
    {
        StopBGM();
        // IDでBGMDataを取得
        var soundData = _soundDataBase.GetBGMData(id);

        if (soundData == null)
        {
            Debug.LogWarning("Sound Data not found for ID: " + id);
            return;
        }

        _bgmSource.PrepareAudioSource(soundData);

        _bgmSource.Play();
    }

    public void StopBGM()
    {
        if (_bgmSource.isPlaying)
        {
            _bgmSource.Stop();
        }
    }

    public void PlaySe(SEID id)
    {
        PlaySe((int)id);
    }

    // string keyからint idに変更
    public void PlaySe(int id)
    {
        // IDでSEDataを取得
        var soundData = _soundDataBase.GetSEData(id);
        if (soundData == null)
        {
            Debug.LogWarning("Sound Data not found for ID: " + id);
            return;
        }

        AudioSource seAudioSource = default;
        if (_seAudioSourcePools.TryDequeue(out AudioSource source))
        {
            seAudioSource = source;
        }
        else
        {
            // プールが枯渇した場合、新しいAudioSourceを生成
            seAudioSource = new GameObject("seAudioSource_" + "NewInstance", typeof(AudioSource)).GetComponent<AudioSource>();
            seAudioSource.transform.SetParent(_seRoot); // ルート下に置く
        }

        seAudioSource.PrepareAudioSource(soundData);
        seAudioSource.gameObject.SetActive(true);
        seAudioSource.Play();

        // コルーチンをAudioManagerインスタンスで実行
        StartCoroutine(ReturnToPoolAfterPlaying(seAudioSource));
    }

    private IEnumerator ReturnToPoolAfterPlaying(AudioSource source)
    {
        // isPlayingがfalseになるまで待機
        yield return new WaitWhile(() => source.isPlaying);
        source.gameObject.SetActive(false);
        _seAudioSourcePools.Enqueue(source);
    }
}

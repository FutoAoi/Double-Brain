
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Audio/SoundData", order = 1)]
public class SoundDataBaseSo : ScriptableObject
{
    [SerializeField]
    private List<SoundData> _soundMap = new List<SoundData>();

    public SoundData GetSoundData(string key)
    {
        return _soundMap.FirstOrDefault(x => x.Key == key);
    }
}

[Serializable]
public class SoundData
{
    [SerializeField, Header("keyを指定する")]
    private string _key;

    [SerializeField, Header("サウンドの種類を設定する")]
    private SoundDataUtility.SoundType _type;

    [SerializeField, Header("ループ再生させるか")]
    private bool _isLoop;

    [SerializeField, Header("即再生させるか")]
    private bool _playOnAwake;

    [SerializeField, Header("AudioClipをアサインする")]
    private AudioClip _clip;

    [SerializeField, Header("音量"), Range(0,1)]
    private float _volume = 1;

    public string Key => _key;
    public SoundDataUtility.SoundType Type => _type;
    public bool IsLoop => _isLoop;
    public bool PlayOnAwake => _playOnAwake;
    public AudioClip Clip => _clip;
    public float Volume => _volume; 
}


using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName ="Audio/SoundDataBese")]
public class SoundDataBaseSo : ScriptableObject
{
    [SerializeField]
    private List<BGMData> _bgmSound = new List<BGMData>();

    [SerializeField]
    private List<SEData> _seSound = new List<SEData>();

    public BGMData GetBGMData(int id)
    {
        return _bgmSound.FirstOrDefault(x => x.ID == id);
    }
    public SEData GetSEData(int id)
    {
        return _seSound.FirstOrDefault(x => x.SEID == id);
    }
}





// インスペクターで設定できるように、カスタムクラスをSerializableにする
[Serializable]
public class BGMData
{
    [SerializeField, Header("BGMのnumberを指定")]
    private int _BGMID;

    [SerializeField, Header("ループ再生させるのか")]
    private bool _isLoop;

    [SerializeField, Header("BGMのボリューム設定")]
    [Range(0, 1)] private float _BGMVolume;

    [SerializeField,Header("AudioClipをアサインする")]
    private AudioClip _clip;
    public int ID => _BGMID;

    public bool IsLoop => _isLoop;

    public float Volume => _BGMVolume;

    public AudioClip Clip => _clip;
}

[Serializable]
public class SEData
{
    [SerializeField, Header("SEのnumberを指定")]
    private int _SEID;

    [SerializeField, Header("SEのボリューム設定")]
    [Range(0, 1)] private float _SEVolume;

    [SerializeField,Header("AudioClipをアサインする")]
    private AudioClip _clip;

    public int SEID => _SEID;

    public float SEVolume => _SEVolume;

    public AudioClip Clip => _clip;
}


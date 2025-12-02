using System;
using NUnit.Framework;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [System.Serializable]
    class SoundData
    {
        [Header("BGMÝ’è")]
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioClip _bgmClipA;

        [Range(0f, 1f)]
        [SerializeField] private float _volume = 1f;

        public AudioSource BgmSource => _bgmSource;
        public AudioClip BgmClipA => _bgmClipA;
        public float Volume => _volume;
    }
    [SerializeField] SoundData[] _soundDatas;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Play(string soundname)
    {
        Debug.Log("SoundManager: {soundname} ‚ðÄ¶‚µ‚Ü‚·B");


        if (_soundDatas.Length > 0)
        {
            if (!_soundDatas[0].BgmSource.isPlaying)
            {
                _soundDatas[0].BgmSource.clip = _soundDatas[0].BgmClipA;
                _soundDatas[0].BgmSource.Play();
            }
        }
    }
}

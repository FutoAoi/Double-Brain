using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // インスペクターで設定できるように、カスタムクラスをSerializableにする
    [System.Serializable]
    public class SoundGroup
    {
        [Tooltip("このグループの音量を制御するUIスライダー")]
        public Slider controlSlider;

        [Tooltip("このスライダーで音量を制御したいAudioSourceのリスト")]
        public List<AudioSource> audioSources;
    }

    [Tooltip("管理したい音源グループ（BGM, SEなど）と対応するスライダーの設定リスト")]
    public List<SoundGroup> soundGroups;

    void Start()
    {
        // 全てのSoundGroupの設定をチェック
        foreach (var group in soundGroups)
        {
            // スライダーが設定されているか確認
            if (group.controlSlider == null)
            {
                Debug.LogError("SoundGroupに割り当てられたスライダーがありません。インスペクターを確認してください。", this);
                continue;
            }

            // 初期音量を設定し、スライダーのイベントリスナーを設定
            float initialVolume = group.controlSlider.value;
            SetGroupVolumes(group.audioSources, initialVolume);

            // スライダーの値が変更されたときに、対応する音源の音量を変更する処理を登録
            // このリスナーを登録することで、Update()で毎フレーム処理する必要がなくなります。
            group.controlSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(group); });
        }
    }

    /// <summary>
    /// スライダーの値が変更されたときに呼び出されるメソッド
    /// </summary>
    /// <param name="group">変更されたスライダーが所属するSoundGroup</param>
    private void OnSliderValueChanged(SoundGroup group)
    {
        float newVolume = group.controlSlider.value;
        SetGroupVolumes(group.audioSources, newVolume);
    }

    /// <summary>
    /// 指定されたAudioSourceリストの全ての音量を一括で設定するメソッド
    /// </summary>
    /// <param name="sources">設定するAudioSourceのリスト</param>
    /// <param name="volume">設定する音量（0.0fから1.0f）</param>
    private void SetGroupVolumes(List<AudioSource> sources, float volume)
    {
        if (sources == null) return;

        foreach (AudioSource source in sources)
        {
            if (source != null)
            {
                source.volume = volume;
            }
        }
    }
}
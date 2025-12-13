using UnityEngine;

public static class SoudDataUtility
{
    public static void PrepareAudioSource(this AudioSource source, BGMData bgmData)
    {
        source.volume = bgmData.Volume;
        source.loop = bgmData.IsLoop;
        source.clip = bgmData.Clip;
    }
    public static void PrepareAudioSource(this AudioSource source, SEData sEData)
    {
        source.volume = sEData.SEVolume;
        source.clip = sEData.Clip;
    }
}

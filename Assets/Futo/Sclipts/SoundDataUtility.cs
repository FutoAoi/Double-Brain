using UnityEngine;

public static class SoundDataUtility
{
    public static class KeyConfig
    {
        public static class Se
        {
            public static readonly string Shoot = "Shoot";
            public static readonly string Door = "Door";
            public static readonly string Swich = "Swich";
            public static readonly string KeyDoor = "KeyDoor";
        }

        public static class Bgm
        {
            public static readonly string InGame = "InGame";
            public static readonly string Title = "Title";
            public static readonly string GameOver = "GameOver";
            public static readonly string GameClear = "GameClear";
        }
    }

    public enum SoundType
    {
        Bgm = 0,
        Se = 1
    }

    public static void PrepareAudioSource(this AudioSource source, SoundData soundData)
    {
        source.playOnAwake = soundData.PlayOnAwake;
        source.loop = soundData.IsLoop;
        source.clip = soundData.Clip;
        source.volume = soundData.Volume;
    }
}
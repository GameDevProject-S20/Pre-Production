using Assets.Scripts.Settings;
using Extentions;
using SIEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField]
    public List<Song> Songs;
    public GameObject AudioGameObject;

    public AudioSource AudioSource { get; private set; }

    private const float MAX_VOLUME = 0.5f;
    private const float FADE_STEP_TIME = 0.25f;
    private bool isFading = false;
    private float fadeTimeLeft;
    private float timeToNextFadeStep;
    private float finalVolume;
    private float volumeFadeStep;

    private float playTime = 0;
    private float silenceTime = 0;

    [Serializable]
    public class Song
    {
        [SerializeField]
        public AudioClip Clip;
        [SerializeField]
        public MusicTag Tag;
        public bool Played { get; private set; }

        public Song(AudioClip clip, MusicTag tag)
        {
            Clip = clip;
            Tag = tag;
            Played = false;
        }

        public void Play(AudioSource source)
        {
            source.clip = Clip;
            source.Play();
            Played = true;
        }

        public void Reset()
        {
            Played = false;
        }
    }

    public enum MusicTag
    {
        GENERAL,
        CAMPFIRE
    }

    private MusicTag currentTag = MusicTag.GENERAL;

    private static class Fader
    {
        private const float DEFAULT_FADE_OUT = 1.5f;
        private const float DEFAULT_FADE_IN = 3.0f;
        
        // Fades out current song and plays next song
        public static IEnumerator FadeOut(MusicManager manager, float fadeOutTime = DEFAULT_FADE_OUT)
        {
            AudioSource source = manager.AudioSource;
            float startVolume = source.volume;

            while (source.volume > 0)
            {
                source.volume -= startVolume * Time.deltaTime / fadeOutTime;
                yield return null;
            }

            source.Stop();
            source.volume = startVolume;
            manager.PlayASong();
        }

        // Plays a new song with a fade in
        public static IEnumerator FadeIn(MusicManager manager, float targetVolume, float fadeInTime = DEFAULT_FADE_IN)
        {
            AudioSource source = manager.AudioSource;
            float deltaVolume = targetVolume - source.volume;

            manager.PlayASong();

            while (source.volume < targetVolume)
            {
                source.volume += deltaVolume * Time.deltaTime / fadeInTime;
                yield return null;
            }
            source.volume = targetVolume;
        }

        // Fades out current song and plays next song with fade in
        public static IEnumerator Crossfade(MusicManager manager, float fadeOutTime = DEFAULT_FADE_OUT, float fadeInTime = DEFAULT_FADE_IN)
        {
            AudioSource source = manager.AudioSource;
            float startVolume = source.volume;

            while (source.volume > 0)
            {
                source.volume -= startVolume * Time.deltaTime / fadeOutTime;
                yield return null;
            }

            source.Stop();
            manager.PlayASong();

            while (source.volume < startVolume)
            {
                source.volume += startVolume * Time.deltaTime / fadeInTime;
                yield return null;
            }

            source.volume = startVolume;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != Instance)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>(); 

        UnityEngine.Random.InitState((int)System.Environment.TickCount);

        Songs.AddRange(Resources.LoadAll("Music/General").Select(f => new Song(f as AudioClip, MusicTag.GENERAL)));

        Songs.AddRange(Resources.LoadAll("Music/Campfire").Select(f => new Song(f as AudioClip, MusicTag.CAMPFIRE)));

        PlayASong();

        if (DataTracker.Current)
        {
            AddVolumeChangedEvent();
        }
        else
        {
            EventManager.Instance.onDataTrackerLoad.AddListener(AddVolumeChangedEvent);
        }

        EventManager.Instance.OnCampfireStarted.AddListener(() => SetSongType(MusicTag.CAMPFIRE));
        EventManager.Instance.OnCampfireEnded.AddListener(() => SetSongType(MusicTag.GENERAL));
    }

    // Update is called once per frame
    /// <summary>
    /// This checks if the song is still playing or not. Once it has stoped
    /// it will call to start a new song 
    /// </summary>
    void Update()
    {
        if (!AudioSource.isPlaying) { PlayASong(); }

        // Music & silence timers
        if (isFading)
        {
            FadeVolume();
        }
        else
        {
            float dTime = Time.deltaTime;
            if (playTime <= 0 && silenceTime <= 0)
            {
                InitSongPlayTime();
                InitVolumeFade(MAX_VOLUME, UnityEngine.Random.Range(8, 18));
            }
            else if (playTime > 0)
            {
                playTime -= dTime;
                if (playTime <= 0)
                {
                    InitVolumeFade(0f, UnityEngine.Random.Range(8, 18));
                }
            }
            else
            {
                silenceTime -= dTime;
            }
        }
    }

    /// <summary>
    /// This will play a new song randomly without repeating until there is no more songs to play.
    /// Once it is out of songs to play, it will reset the order then start playing again. 
    /// </summary>
    void PlayASong()
    {

        var songsOfTag = Songs.Where(s => s.Tag == currentTag);
        int count = songsOfTag.Count();
        if (count == 0)
        {
            Debug.LogError(string.Format("No songs found for tag {0}", currentTag));
            return;
        }

        var unplayedSongsOfTag = songsOfTag.Where(s => !s.Played);
        count = unplayedSongsOfTag.Count();
        if (count == 0)
        {
            unplayedSongsOfTag = songsOfTag.ForEach(s => s.Reset());
        }

        unplayedSongsOfTag.ElementAt(UnityEngine.Random.Range(0, count)).Play(AudioSource);

        if (playTime <= 0 && silenceTime <= 0)
        {
            InitSongPlayTime();
        }
    }

    private void SetSongType(MusicTag type)
    {
        currentTag = type;
        StartCoroutine(Fader.Crossfade(this));
    }

    /// <summary>
    /// This will fade a volume to a specified volume over a specified number of seconds.
    /// Fading is up or down, relative to the current volume.
    /// This method must be called to initiate the fade.
    /// </summary>
    void InitVolumeFade(float finalVol, float time)
    {
        finalVolume = finalVol;
        fadeTimeLeft = time;
        timeToNextFadeStep = FADE_STEP_TIME;
        float nSteps = time / timeToNextFadeStep;
        volumeFadeStep = (finalVol - AudioSource.volume) / nSteps;
        isFading = true;
    }

    /// <summary>
    /// This will fade a volume to a specified volume over a specified number of seconds.
    /// Fading is up or down, relative to the current volume.
    ///
    /// This method should only be called in the Update() method.
    /// </summary>
    void FadeVolume()
    {
        float dTime = Time.deltaTime;
        timeToNextFadeStep -= dTime;
        fadeTimeLeft -= dTime;
        if (timeToNextFadeStep <= 0)
        {
            AudioSource.volume += volumeFadeStep;

            // This accounds for the fact that timeToNextFadeStep is probably negative
            // Call this sooner on the next iteration
            timeToNextFadeStep = FADE_STEP_TIME + timeToNextFadeStep;
        }

        if (AudioSource.volume < 0)
        {
            AudioSource.volume = 0;
        }
        else if (AudioSource.volume > MAX_VOLUME * GetMultiplier())
        {
            AudioSource.volume = MAX_VOLUME * GetMultiplier();
        }

        if (fadeTimeLeft <= 0)
        {
            isFading = false;
            UpdateVolume();
        }
    }

    /// <summary>
    /// Sets the period of time for song playback and silence break.
    /// </summary>
    void InitSongPlayTime()
    {
        playTime = UnityEngine.Random.Range(1, 3) * 60;  // s
        silenceTime = UnityEngine.Random.Range(6, 10);  // s
        Debug.Log($"song play time = {playTime}");
        Debug.Log($"song silence time = {silenceTime}");
    }

    /// <summary>
    /// Sets the current volume to the maximum * the current multiplier.
    /// </summary>
    private void UpdateVolume()
    {
        AudioSource.volume = MAX_VOLUME * GetMultiplier();
    }

    /// <summary>
    /// Retrieves the volume multiplier from the settings manager.
    /// </summary>
    /// <returns></returns>
    float GetMultiplier()
    {
        if (!DataTracker.Current || DataTracker.Current.SettingsManager == null)
        {
            return 1;
        }

        return DataTracker.Current.SettingsManager.VolumeMultiplier;
    }

    /// <summary>
    /// Adds an event to listen for when volume changes.
    /// </summary>
    void AddVolumeChangedEvent()
    {
        DataTracker.Current.EventManager.OnSettingsChanged.AddListener((setting) =>
        {
            if (isFading || setting != SettingTypes.VolumeMultiplier.ToString())
            {
                return;
            }

            UpdateVolume();
        });
    }
}

using Assets.Scripts.Settings;
using SIEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackroundMusic : MonoBehaviour
{

    [SerializeField]    
    public List<AudioClip> Songs;
    public GameObject AudioGameObject;

    private AudioSource AS;
    private List<int> Played = new List<int> { -1 }; 

    private const float MAX_VOLUME = 0.5f;
    private const float FADE_STEP_TIME = 0.25f;
    private bool isFading = false;
    private float fadeTimeLeft;
    private float timeToNextFadeStep;
    private float finalVolume;
    private float volumeFadeStep;

    private float playTime = 0;
    private float silenceTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        AS = AudioGameObject.GetComponent<AudioSource>(); 
        DontDestroyOnLoad(AS);
        PlayASong();

        if (DataTracker.Current)
        {
            AddVolumeChangedEvent();
        }
        else
        {
            EventManager.Instance.onDataTrackerLoad.AddListener(AddVolumeChangedEvent);
        }
    }

    // Update is called once per frame
    /// <summary>
    /// This checks if the song is still playing or not. Once it has stoped
    /// it will call to start a new song 
    /// </summary>
    void Update()
    {
        if (!AS.isPlaying) { PlayASong(); }

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
                InitVolumeFade(MAX_VOLUME, Random.Range(8, 18));
            }
            else if (playTime > 0)
            {
                playTime -= dTime;
                if (playTime <= 0)
                {
                    InitVolumeFade(0f, Random.Range(8, 18));
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
        int random = -1;

        Random.InitState((int)System.Environment.TickCount);  

        if ((Played.Count+1) == Songs.Count)
        {
            Played = new List<int> { -1 }; 
        }


        while (Played.Contains(random))
            random = Random.Range(0, Songs.Count);

        Debug.Log(random);  

        Played.Add(random);
        AS.clip = Songs[random];
        UpdateVolume();
        AS.Play();

        if (playTime <= 0 && silenceTime <= 0)
        {
            InitSongPlayTime();
        }
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
        volumeFadeStep = (finalVol - AS.volume) / nSteps;
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
            AS.volume += volumeFadeStep;

            // This accounds for the fact that timeToNextFadeStep is probably negative
            // Call this sooner on the next iteration
            timeToNextFadeStep = FADE_STEP_TIME + timeToNextFadeStep;
        }

        if (AS.volume < 0)
        {
            AS.volume = 0;
        }
        else if (AS.volume > MAX_VOLUME * GetMultiplier())
        {
            AS.volume = MAX_VOLUME * GetMultiplier();
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
        playTime = Random.Range(1, 3) * 60;  // s
        silenceTime = Random.Range(6, 10);  // s
        Debug.Log($"song play time = {playTime}");
        Debug.Log($"song silence time = {silenceTime}");
    }

    /// <summary>
    /// Sets the current volume to the maximum * the current multiplier.
    /// </summary>
    private void UpdateVolume()
    {
        AS.volume = MAX_VOLUME * GetMultiplier();
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

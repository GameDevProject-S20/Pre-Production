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

    private const float FADE_STEP_TIME = 0.25f;
    private bool isFading = false;
    private float fadeTimeLeft;
    private float timeToNextFadeStep;
    private float volumeFadeStep;

    // Start is called before the first frame update
    void Start()
    {
        AS = AudioGameObject.GetComponent<AudioSource>(); 
        DontDestroyOnLoad(AS);
        PlayASong(); 
    }

    // Update is called once per frame
    /// <summary>
    /// This checks if the song is still playing or not. Once it has stoped
    /// it will call to start a new song 
    /// </summary>
    void Update()
    {
        if (!AS.isPlaying) { PlayASong(); }

        if (isFading)
        {
            FadeVolume();
        }
        /*
        else
        {
            InitVolumeFade(0f, 5f);
        }
        */
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
        AS.Play();

        AS.volume = 1f;
    }

    /// <summary>
    /// This will fade a volume to a specified volume over a specified number of seconds.
    /// Fading is up or down, relative to the current volume.
    /// This method must be called to initiate the fade.
    /// </summary>
    void InitVolumeFade(float finalVolume, float time)
    {
        fadeTimeLeft = time;
        timeToNextFadeStep = FADE_STEP_TIME;
        float nSteps = time / timeToNextFadeStep;
        volumeFadeStep = (finalVolume - AS.volume) / nSteps;
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
            timeToNextFadeStep = FADE_STEP_TIME;
        }

        if (AS.volume < 0)
        {
            AS.volume = 0;
        }
        else if (AS.volume > 1.0f)
        {
            AS.volume = 1.0f;
        }

        if (fadeTimeLeft <= 0) isFading = false;
    }

}

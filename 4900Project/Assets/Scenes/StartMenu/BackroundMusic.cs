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
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //public static List<Track> trackList = new List<Track>();
    public List<AudioClip> trackList = new List<AudioClip>();
    public static SoundManager instance;

    /*
    private static float clipLength;
    private static bool keepFadingIn;
    private static bool keepFadingOut;

    public static List<AudioSource> sources = new List<AudioSource>();*/

    public AudioSource musicSourceA;
    public AudioSource musicSourceB;

    public static float musicVolume = 0.5f;
    public int currentTrack = 0;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        musicSourceA = gameObject.AddComponent<AudioSource>();
        musicSourceB = gameObject.AddComponent<AudioSource>();
        musicSourceA.volume = musicVolume;
        musicSourceA.loop = true;
        musicSourceB.volume = 0.0f;
        musicSourceB.loop = true;


        musicSourceA.clip = trackList[0];
        musicSourceA.Play();

        SceneLoader.Start();

       // StartCoroutine(PlayMusic());
    }
    
    public void CrossFadeCaller(AudioSource A, AudioSource B, float seconds)
    {
        StartCoroutine(CrossFade(A, B, seconds));
    }

    public void SwitchTrackCaller(int i)
    {
        //UnityEngine.Debug.Log("made it to switch call");
        StartCoroutine(SwitchTrack(i));
    }

    public void PlayMusicCaller()
    {
        StartCoroutine(PlayMusic());
    }

    public static IEnumerator CrossFade(AudioSource A, AudioSource B, float seconds)
    {
        //UnityEngine.Debug.Log("Starting crossfade");

        // Calculate duration of each step
        float stepInterval = seconds / 20.0f;
        float volumeInterval = musicVolume / 20.0f;

        B.Play();

        // Fade between the two
        for(int i = 0; i < 20; i++)
        {
            A.volume -= volumeInterval;
            B.volume += volumeInterval;

            yield return new WaitForSeconds(stepInterval);
        }

        A.Stop();

        UnityEngine.Debug.Log("Crossfade done");
    }

    public IEnumerator SwitchTrack(int i)
    {
        UnityEngine.Debug.Log("switching track");

        bool playA = true;
        if(musicSourceB.volume == 0.0f)
        {
            playA = false;
        }

        if(playA)
        {
            musicSourceA.clip = trackList[i];
            yield return StartCoroutine(CrossFade(musicSourceB, musicSourceA, 2.0f));
        }
        else
        {
            musicSourceB.clip = trackList[i];
            yield return StartCoroutine(CrossFade(musicSourceA, musicSourceB, 2.0f));
        }

        UnityEngine.Debug.Log("track switched");
    }

    public IEnumerator PlayMusic()
    {
        
        while(true)
        {
            int i = currentTrack;
            while(i == currentTrack)
            {
                i = Random.Range(0, trackList.Count - 1);
            }

            currentTrack = i;
            StartCoroutine(SwitchTrack(currentTrack));

            // Wait for the track to finish before continuing to the next
            // Maybe replace with clip.length
            yield return new WaitForSeconds(20);
        }

    }

    public static IEnumerator Fade(AudioSource audioSource, float duration, float targetVolume)
    {
        UnityEngine.Debug.Log("fade started");
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    public AudioClip[] sounds;
    AudioClip enemySound;
    AudioSource enemyAudio;

    /*
    string sound1 = "(Light Eating)";
    string sound2 = "(Slime)";
    string sound3 = "(Louder Eating)";
    string sound4 = "(Burping)";
    string sound5 = "(Monster Noises)"; */

    float volume = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        enemySound = sounds[Random.Range(1, sounds.Length + 1) - 1];
        enemyAudio = gameObject.GetComponent<AudioSource>();
        enemyAudio.clip = enemySound;
        enemyAudio.volume = volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyAudio.isPlaying)
        {
            enemyAudio.volume = volume;
            enemyAudio.Play();
            StartCoroutine(SoundManager.Fade(enemyAudio, 1.0f, volume));

        }
    }
}

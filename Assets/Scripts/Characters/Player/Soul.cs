using UnityEngine;

public class Soul : MonoBehaviour
{
    public GameObject stats;
    public AudioClip soulPickupSound;

    private AudioSource playerAudio;

    private void Start()
    {
        stats = GameObject.Find("PlayerStats");
        playerAudio = GameObject.Find("Player").GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        //Maybe a little animation?
    }


    void OnTriggerEnter2D(Collider2D col)
	{
        if(col.gameObject.tag.Equals("Player"))
        {
            print("soul picked up");
            playerAudio.PlayOneShot(soulPickupSound);
            stats.GetComponent<Stats>().souls++;
            Destroy(gameObject);
        }
	}
}

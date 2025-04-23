using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] songs;
    bool isPlayingSong = false;

    private void Start()
    {
        source.loop = false;
        source.playOnAwake = false;
        playSong();
    }

    void playSong()
    {
        int rnd = Random.Range(0, songs.Length - 1);
        source.clip = songs[rnd];
        source.Play();
        isPlayingSong = true;
    }

    private void Update()
    {
        if (!source.isPlaying)
        {
            playSong();
        }
    }
}

using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public struct Sound {
        
        public string name;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume;

        [Range(.1f, 3f)]
        public float pitch;

        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }

    public int numSongsA, numSongsB;
    public Sound[] sounds;
    public Sound s;
    public Sound sNull;

    private char side = 'A';
    
    
    void Awake() 
    {
        DontDestroyOnLoad(gameObject); //this will allow music to persist throughout scenes
        sNull.source = null;
    }

    public void Init()
    {
        for(int i = 0; i<sounds.Length; i++)
        {
            sounds[i].source = gameObject.AddComponent<AudioSource>();

            sounds[i].source.clip = sounds[i].clip;
            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.pitch = sounds[i].pitch;
            sounds[i].source.loop = sounds[i].loop;
            sounds[i].source.playOnAwake = false;
        }
    }

    private void Update()
    {
        if(MusicGameState.currentAlbum == gameObject && MusicGameState.turning && s.source) //&& !s.source.isPlaying) //the song should change
        {
           // Debug.Log("seconds left: " + (s.clip.length - s.source.time));
            if(s.clip.length - s.source.time < 1f) //switch songs
            {
                int index = Array.IndexOf(sounds, s);
                switch (side)
                {
                    case 'A':
                        if (index + 2 > numSongsA) MusicGameState.turning = false;
                        else
                        {
                            LineUpSong(index + 2);
                            PlayPause();
                          
                            MusicGameState.vinyl.songChosen++;
                            MusicGameState.vinyl.startSong(index + 2, 0);
                        }
                        break;
                    case 'B':
                        if (index + 2 > (numSongsB + numSongsA)) MusicGameState.turning = false;
                        else
                        {
                            LineUpSong(index + 2 - numSongsA); //here
                            PlayPause();
                      
                            MusicGameState.vinyl.songChosen++;
                            MusicGameState.vinyl.startSong(index + 2 - numSongsA, 0);
                        }
                        break;
                }
            }

              
        }
    }

    public void changePitch()
    {
        if(s.source)
        {
            if (MusicGameState.rpm < 34f) s.source.pitch = 1f; //MusicGameState.rpm = 33.3f
            else s.source.pitch = 1.5f; //MusicGameState.rpm = 45f
        }
    }

    public void LineUpSong(int songChosen, float fraction = 0f)
    {
        switch (side)
        {
            case 'A':
                s = sounds[songChosen - 1];
                break;
            case 'B':
                s = sounds[numSongsA + songChosen - 1];
                break;
        }
        float time = fraction * s.clip.length;
        s.source.time = time;

        Debug.Log("time: " + time);

        Debug.Log("s.name: "+s.name);
    }

    public void switchSide()
    {
        side = (side == 'A') ? 'B' : 'A';
        if(s.source) s.source.Pause();
        GameObject.Find("cross").GetComponent<toneArmDrag>().PlaceBack();
        MusicGameState.turning = false;
        MusicGameState.needleDrop = false;
        if(side == 'A')
        {
            GameObject.Find("RecordPlayer").GetComponent<RecordPlayer>().changeRecord(numSongsA);
        }else if(side == 'B')
            GameObject.Find("RecordPlayer").GetComponent<RecordPlayer>().changeRecord(numSongsB);
    }

    public void PlayPause()
    {
        if (!MusicGameState.needleDrop || !s.source) return;

        if (!MusicGameState.turning)
        {
            if(s.source.isPlaying) s.source.Pause(); //maybe im pausing twice?
        }
        else
        {
            s.source.volume = 1f;
            changePitch();
            s.source.Play();
        }

       // Debug.Log("turning: " + MusicGameState.turning);

    }

    public void ResetAlbum()
    {
        MusicGameState.turning = false;
        side = 'A';

    }
}

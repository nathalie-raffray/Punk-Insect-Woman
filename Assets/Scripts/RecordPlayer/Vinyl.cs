using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vinyl : MonoBehaviour
{
    
    public int songChosen;
    public int numRings;
    float[] radii;
    public float[] angles;

    [HideInInspector]
    public bool turning;

    GameObject tonearm;

    AudioManager am;

    void Start()
    {
        MusicGameState.vinyl = this;

        turning = false;

        int num = numRings;
        int index;
        radii = new float[numRings];

        foreach (Transform child in transform)
        {   
            index = int.Parse(child.name.Trim('r', 'i', 'n', 'g'));
           
            radii[index - 1] = child.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        }

        tonearm = GameObject.Find("tonearm");
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();

    }

    void Update()
    {
        if(MusicGameState.turning)
        {
            transform.Rotate(0, 0, MusicGameState.rpm / 60 * -360 * Time.deltaTime);
           if(MusicGameState.needleDrop && songChosen != numRings)
            {
                float fraction = MusicGameState.currAM.s.source.time / MusicGameState.currAM.s.clip.length;
              //  Debug.Log("fraction: " + fraction);
                float rotz = angles[songChosen - 1] + (angles[songChosen] - angles[songChosen - 1]) * fraction;
              //  Debug.Log("rotz: " + rotz);
                tonearm.transform.rotation = Quaternion.Euler(0, 0, rotz);

            }
        }
    }

    public void putDownToneArm(Vector3 pos)
    {

        MusicGameState.needleDrop = true; 
        if (songChosen == 0)
        {
            MusicGameState.needleDrop = false;
            return;
        }
        if (songChosen == numRings)
        {
            MusicGameState.currAM.s = MusicGameState.currAM.sNull;
            startSong(songChosen, 0);
            Debug.Log("songchosen = numrings 1");
            //MusicGameState.currAM.PlayPause();
            return;
        }

        //Debug.Log("radii length" + radii.Length);
       // Debug.Log("songChosen: " + songChosen);

        float start = radii[songChosen-1];
        float end = radii[songChosen];
        float current = (pos - transform.position).magnitude;

        float fractionOfSong = (current - start) / (end - start);

        if (fractionOfSong < 0.2f)
        {
            startSong(songChosen, 0);
        }else if(fractionOfSong > 0.8f)
        {
            songChosen += 1;
            if (songChosen == numRings)
            {
                //MusicGameState.currAM.PlayPause();
                //NEED TO MAKE SURE S.SOURCE IS NULL FOR NEXT TIME YOU PRESS PLAY
                //songChosen -= 1;
                MusicGameState.currAM.s = MusicGameState.currAM.sNull;
                startSong(songChosen, 0);
                Debug.Log("songchosen = numrings 1.2");
                Debug.Log("fractionOfSong: " + fractionOfSong);
                Debug.Log("song chosen: " + songChosen);
                return;
            }
            startSong(songChosen, 0);
        }
        else
        {
            startSong(songChosen, fractionOfSong);
        }

        //Debug.Log("start: " + start);
        //Debug.Log("current: " + current);
        //Debug.Log("end: " + end);

        Debug.Log("fractionOfSong: " + fractionOfSong);

    }

    public void startSong(int songNum, float fraction)
    {
        if(songNum == numRings)
        {
            //end of vinyl
            tonearm.transform.rotation = Quaternion.Euler(0, 0, angles[songNum - 1]);
            Debug.Log("songchosen = numrings 2");
        }

        else if(Mathf.Abs(fraction) < 0.1f) //checking if fraction is 0
        {
            //start song at start
            tonearm.transform.rotation = Quaternion.Euler(0, 0, angles[songNum - 1]);
            MusicGameState.currAM.LineUpSong(songNum, 0);
            MusicGameState.currAM.PlayPause();
        }
        else
        {
            //start song at fraction
            MusicGameState.currAM.LineUpSong(songNum, fraction);
            MusicGameState.currAM.PlayPause();
        }
        Debug.Log("song chosen: " + songChosen);
        Debug.Log("song num: " + songNum);
 
    }

}

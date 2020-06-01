using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPlayer : MonoBehaviour
{

    public GameObject album;
    public AudioManager am;

    private GameObject tonearm;

    public GameObject vinyl2;
    public GameObject vinyl3;
    public GameObject vinyl4;
    public GameObject vinyl5;

    void Awake()
    {
        tonearm = transform.Find("tonearm").gameObject;
    }

    private void Start()
    {
        am = album.GetComponent<AudioManager>();
    }

 
    public void changeRecord(int numTracks)
    {
        if(GameObject.FindGameObjectWithTag("vinyl") != null) Destroy(GameObject.FindGameObjectWithTag("vinyl"));
        MusicGameState.needleDrop = false;
        MusicGameState.turning = false;
        GameObject g = null;
        switch (numTracks)
        {
            case 2:
                g = Instantiate(vinyl2, new Vector3(0,0,0), Quaternion.identity);
                break;
            case 3:
                g = Instantiate(vinyl3, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case 4:
                g = Instantiate(vinyl4, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case 5:
                g = Instantiate(vinyl5, new Vector3(0, 0, 0), Quaternion.identity);
                break;
        }
        if (g != null)
        {
            MusicGameState.vinyl = g.GetComponent<Vinyl>();
            g.transform.SetParent(transform);
            g.transform.localPosition = new Vector3(-0.72f, 0.25f, 0);
            g.transform.localScale = new Vector3(2.1f, 2.1f, 0);
        }

    }
}



using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RecordRing : MonoBehaviour
{
    private int trackNumber;
    toneArmDrag t;

    void Start()
    {
        trackNumber = int.Parse(name.Trim('r', 'i', 'n', 'g'));
        t = GameObject.Find("cross").GetComponent<toneArmDrag>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "needle" && t.dragging)
        {
            if (MusicGameState.vinyl.songChosen < trackNumber)
            {
                MusicGameState.vinyl.songChosen = trackNumber;
            }
        }      
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "needle" && t.dragging) MusicGameState.vinyl.songChosen--;
    }
}

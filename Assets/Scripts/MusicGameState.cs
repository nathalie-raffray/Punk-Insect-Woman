using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MusicGameState 
{
    public static GameObject currentAlbum;
    public static AudioManager currAM;
    public static Vinyl vinyl;
    public static int songPlaying;
    public static bool turning = false;
    public static bool needleDrop = false;
    public static float rpm = 33.3f;
}

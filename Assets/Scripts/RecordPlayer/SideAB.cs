using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAB : MonoBehaviour
{
    private void OnMouseUp()
    {
        MusicGameState.currAM.switchSide();
    }
}

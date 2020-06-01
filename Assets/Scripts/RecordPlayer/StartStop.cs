using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStop : MonoBehaviour
{
    public Sprite hoverSprite;
    public Sprite idleSprite;
    private SpriteRenderer sr;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        transform.position -= new Vector3(0, 0.2f, 0);
        MusicGameState.turning = !MusicGameState.turning;
        MusicGameState.currAM.PlayPause(); 
    }

    private void OnMouseUp()
    {
        transform.position += new Vector3(0, 0.2f, 0);
    }

    private void OnMouseOver()
    {
        sr.sprite = hoverSprite;
    }
    private void OnMouseExit()
    {
        sr.sprite = idleSprite;
    }


}

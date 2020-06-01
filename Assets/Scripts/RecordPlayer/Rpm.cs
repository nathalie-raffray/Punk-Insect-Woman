using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rpm : MonoBehaviour
{

    public Sprite idle;
    public Sprite threeClick;
    public Sprite threeHover;
    public Sprite fourClick;
    public Sprite fourHover;

    private SpriteRenderer sr;

    private bool clicked;

    void Start()
    {
        clicked = false;
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        transform.position -= new Vector3(0, 0.2f, 0);
        
    }

    private void OnMouseUp()
    {
        transform.position += new Vector3(0, 0.2f, 0);
        if (GetMouseWorldPos().x < transform.position.x)
        {
            sr.sprite = threeClick;
            MusicGameState.rpm = 33.3f;
            MusicGameState.currAM.changePitch();
        }
        else
        {
            sr.sprite = fourClick;
            MusicGameState.rpm = 45f;
            MusicGameState.currAM.changePitch();
        }
    }

    Vector3 GetMouseWorldPos()
    {

        Vector3 mousePoint = Input.mousePosition;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}

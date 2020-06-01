using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toneArmDrag : MonoBehaviour
{
    float maxAngle;
    float minAngle;

    GameObject tonearm;
    RecordPlayer rp;

    Vector3 crossRestingPos;
    Vector3 tonearmRestingPos;

    Vector3 u, v;

    public bool dragging = false;

    void Start()
    {

        minAngle = 0;
        maxAngle = 56f;

        tonearm = transform.parent.gameObject;
        rp = transform.parent.parent.gameObject.GetComponent<RecordPlayer>();

        crossRestingPos = transform.position;
        tonearmRestingPos = transform.parent.position;

        u = crossRestingPos - tonearmRestingPos;
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDown()
    {
        Debug.Log("mouse down");
    }

    private void OnMouseDrag()
    {
       // Debug.Log("dragging");

        dragging = true;
        if (MusicGameState.currAM.s.source) MusicGameState.currAM.s.source.Pause();
        MusicGameState.needleDrop = false;
        v = GetMouseWorldPos() - tonearmRestingPos;
        v.z = 0;
        u.z = 0;

        double theta = Math.Acos( Vector3.Dot(u, v) / (u.magnitude * v.magnitude) )* (180/Math.PI);

        if(GetMouseWorldPos().x > crossRestingPos.x)
        {
            tonearm.transform.rotation = Quaternion.Euler(0, 0, minAngle);
        }
        else if(theta >= minAngle && theta <= maxAngle)
        {
            tonearm.transform.rotation = Quaternion.Euler(0, 0, (float)-theta);
        }else if(theta > maxAngle)
        {
            tonearm.transform.rotation = Quaternion.Euler(0, 0, -maxAngle);
        }     
    }

    private void OnMouseUp()
    {
        MusicGameState.vinyl.putDownToneArm(transform.position);
        dragging = false;
    }

    public void PlaceBack()
    {
        tonearm.transform.rotation = Quaternion.Euler(0, 0, minAngle);
    }
}

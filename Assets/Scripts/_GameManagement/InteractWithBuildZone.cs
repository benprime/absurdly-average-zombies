﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractWithBuildZone : MonoBehaviour
{
    private static GameObject lastHit = null;

    // Use this for initialization
    void Start()
    {
    }

    public void HideRadialMenu()
    {
        if (lastHit) lastHit.GetComponent<BuildZone>().CloseOut();
    }

    void Update()
    {
        //int pointerId = (Input.touchCount == 1) ? Input.GetTouch(0).fingerId : -1;  //Only accounts for single touches TODO: make it work nicely for accidental multi touches

        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, 20, LayerMask.GetMask("BuildZone", "UI"));

        // over a game object?
        bool isOverGameObject = false;

        if (EventSystem.current.IsPointerOverGameObject())
            isOverGameObject = true;

        foreach (Touch t in Input.touches)
        {
            if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
                isOverGameObject = true;
        }

        if (hitInfo && !isOverGameObject)
        { //mouse is over a BuildZone && do not place object when mouse is over button
            GameObject hitZone = hitInfo.transform.gameObject;

            /*
            if (Input.GetMouseButtonDown(0))
            {
                //hitSprite.color = Color.green;  //TODO: make flag in BuildZone class that sets color when mouse is over it
            }
            */

            bool isSingleTouchEnding = (Input.touchCount == 1) ? (Input.GetTouch(0).phase == TouchPhase.Ended && Input.GetTouch(0).phase == TouchPhase.Canceled) : false; //check to see if there is touch input, and if so, if the touch just ended
            if (Input.GetMouseButtonUp(0) || isSingleTouchEnding)
            {
                if (lastHit && lastHit.GetComponent<BuildZone>().menuOpen) return;  //temp fix for bug #1
                if (lastHit && lastHit != hitZone) lastHit.GetComponent<BuildZone>().CloseOut();
                lastHit = hitZone;

                hitZone.GetComponent<BuildZone>().PopRadialMenu(hitZone.transform.position);
            }
        }
        else
        { //mouse is not over a BuildZone			
            if (!isOverGameObject)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (lastHit) lastHit.GetComponent<BuildZone>().CloseOut();
                }
            }
        }

    }

}

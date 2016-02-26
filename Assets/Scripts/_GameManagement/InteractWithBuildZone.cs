using UnityEngine;
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
        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, 20, LayerMask.GetMask("BuildZone", "UI"));

        // over a game object?
        bool isOverUIButton = EventSystem.current.IsPointerOverGameObject();

        foreach (Touch t in Input.touches)
        {
            if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
            {
                isOverUIButton = true;
                break;
            }
        }
        bool isSingleTouchEnding = (Input.touchCount == 1) ? (Input.GetTouch(0).phase == TouchPhase.Ended && Input.GetTouch(0).phase == TouchPhase.Canceled) : false; //check to see if there is touch input, and if so, if the touch just ended

        // mouse is over a BuildZone && do not place object when mouse is over button
        if (hitInfo && !isOverUIButton)
        {
            GameObject hitZone = hitInfo.transform.gameObject;

            if (Input.GetMouseButtonUp(0) || isSingleTouchEnding)
            {
                if (lastHit && lastHit != hitZone) lastHit.GetComponent<BuildZone>().CloseOut();
                lastHit = hitZone;

                hitZone.GetComponent<BuildZone>().PopRadialMenu(hitZone.transform.position);
            }
        }
        else
        {
            // mouse is not over a BuildZone			
            if (!isOverUIButton)
            {
                if (Input.GetMouseButtonDown(0) || isSingleTouchEnding)
                {
                    if (lastHit) lastHit.GetComponent<BuildZone>().CloseOut();
                }
            }
        }

    }

}

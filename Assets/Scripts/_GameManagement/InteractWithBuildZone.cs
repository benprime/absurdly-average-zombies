using ICSharpCode.SharpZipLib;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractWithBuildZone : MonoBehaviour
{
    private static BuildZone lastBuildZoneClicked = null;

    // Use this for initialization
    void Start()
    {
    }

    public void HideRadialMenu()
    {
        lastBuildZoneClicked.CloseOut();
        lastBuildZoneClicked = null;
    }

    private bool IsPointerOverUIComponent()
    {
        bool isOverUIComponent = EventSystem.current.IsPointerOverGameObject();
        if (isOverUIComponent)
        {
            return true;
        }

        foreach (Touch t in Input.touches)
        {
            if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
            {
                return true;
            }
        }
        return false;
    }

    private BuildZone IsPointerOverBuildZone()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, 20, LayerMask.GetMask("BuildZone", "UI"));
        if (hitInfo.transform == null || hitInfo.transform.gameObject == null)
        {
            return null;
        }

        return hitInfo.transform.gameObject.GetComponent<BuildZone>();
    }

    private bool ClickOccurred()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        if (ClickOccurred())
        {
            // The user has clicked on a UI component.
            // Return immediately to prevent click bleeding.
            if (IsPointerOverUIComponent())
            {
                return;
            }

            // The user has clicked a build zone.
            BuildZone bz = IsPointerOverBuildZone();
            if (bz != null)
            {
                // If the user currently had a popup radial
                // open for a different build zone, we close it.
                if (lastBuildZoneClicked != null)
                {
                    lastBuildZoneClicked.CloseOut();
                    lastBuildZoneClicked = null;
                }

                // popup radial for the click build zone
                // and track what build zone we have open.
                bz.PopRadialMenu();
                lastBuildZoneClicked = bz;
            }

            // if the user has clicked anywhere else on the
            // screen, if there is a radial open, we close it.
            if (bz == null && lastBuildZoneClicked != null)
            {
                lastBuildZoneClicked.CloseOut();
                lastBuildZoneClicked = null;
            }
        }
    }
}

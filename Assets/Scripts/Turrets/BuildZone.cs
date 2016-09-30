using UnityEngine;

public class BuildZone : MonoBehaviour
{
    public enum ZONE_STATE { EMPTY, BUILT_ON, DESTROYED };
    public ZONE_STATE currentState = ZONE_STATE.EMPTY;
    public GameObject weaponRadial, upgradeRadial;
    private GameObject currentHub = null;
    public GameObject currentWeapon = null;
    public bool menuOpen = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Clear()
    {
        Destroy(this.currentWeapon);
        this.currentState = ZONE_STATE.EMPTY;
    }

    public void PopRadialMenu()
    {
        if (currentHub != null)
            return;

        var uiCanvas = FindObjectOfType<Canvas>().transform;
        if (currentState == ZONE_STATE.EMPTY)
        {
            currentHub = Instantiate(weaponRadial, gameObject.transform.position, Quaternion.identity) as GameObject;
			currentHub.GetComponent<UI_WeaponRadial>().connectedZone = this;
			currentHub.GetComponent<UI_WeaponRadial>().InitRadial();
            menuOpen = true;
        }

        if (currentState == ZONE_STATE.BUILT_ON)
        {
            currentHub = Instantiate(upgradeRadial, transform.position, Quaternion.identity) as GameObject;
            currentHub.GetComponent<UI_UpgradeRadial>().connectedZone = this;
            currentHub.GetComponent<UI_UpgradeRadial>().InitRadial();
            menuOpen = true;
            currentWeapon.transform.FindChild("DetectionZone").GetComponent<SpriteRenderer>().enabled = true;
        }
        currentHub.transform.SetParent(uiCanvas, false);
        currentHub.transform.position = ClampGameObjectInsideCamera(gameObject.transform.position, currentHub);
    }

    public void CloseOut()
    {
        if (currentWeapon) currentWeapon.transform.FindChild("DetectionZone").GetComponent<SpriteRenderer>().enabled = false;
        if (currentHub) Destroy(currentHub);
        menuOpen = false;
    }

    private Vector3 ClampGameObjectInsideCamera(Vector3 location, GameObject gameObject)
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(
            Camera.main.pixelWidth, Camera.main.pixelHeight));

        var cameraRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);

        var canvasSize = FindObjectOfType<Canvas>().transform.GetComponent<RectTransform>().rect.size;
        var widthRatio = canvasSize.x / cameraRect.width;
        var heightRatio = canvasSize.y / cameraRect.height;

        var radialMenuSize = gameObject.GetComponent<RectTransform>().rect.size;
        var halfSizeX = (radialMenuSize.x / widthRatio) / 2;
        var halfSizeY = (radialMenuSize.y / heightRatio) / 2;

        return new Vector3(
            Mathf.Clamp(location.x, cameraRect.xMin + halfSizeX, cameraRect.xMax - halfSizeX),
            Mathf.Clamp(location.y, cameraRect.yMin + halfSizeY, cameraRect.yMax - halfSizeY),
            transform.position.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMaster : MonoBehaviour {

    [Header("Camera Settings")]
    [Range(-1.5f, 1.5f)]
    public float xOffset;
    [Range(-1.5f, 1.5f)]
    public float yOffset;
    public bool deadzonesEnabled;
    public Vector2 deadzoneBox;
    public bool boundsEnabled;
    public Axis boundType;
    public Vector2 boundsMin;
    public Vector2 boundsMax;
    public CameraFollow mainCamera;
    public Transform background;

    private void Start()
    {
        
    }

    public void InstantiateRoom()
    {
        StartCoroutine(ApplyCameraSettings());
    }

    public void SnapParallax()
    {
        if (background)
        {
            background.position = new Vector3(transform.position.x, transform.position.y, background.position.z);
            background.GetComponent<Parallax>().ResetParallax();
        }
    }

    private IEnumerator ApplyCameraSettings()
    {
        mainCamera.xOffset = xOffset;
        mainCamera.yOffset = yOffset;
        mainCamera.deadzonesEnabled = deadzonesEnabled;
        mainCamera.deadzoneBox = deadzoneBox;
        mainCamera.boundsEnabled = boundsEnabled;
        mainCamera.boundType = boundType;
        mainCamera.boundsMin = boundsMin;
        mainCamera.boundsMax = boundsMax;
        yield return 0;
        mainCamera.SnapToDestination();
    }
}

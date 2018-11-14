using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [Header("Offset")]
    [Range(-1.5f, 1.5f)]
    public float xOffset;
    [Range(-1.5f, 1.5f)]
    public float yOffset;

    [Header("Following Properties")]
    [Range(0, 2)]
    public float damp;
    public Transform follow;

    [Header("Deadzones")]
    public bool deadzonesEnabled;
    public Vector2 deadzoneBox;

    [Header("Bounds")]
    public bool boundsEnabled;
    public Axis boundType;
    public Vector2 boundsMin;
    public Vector2 boundsMax;

    private Vector3 velocity = Vector3.zero;
    private Camera main;
    private Vector3 destination;

    private void Start()
    {
        main = GetComponent<Camera>();
    }

    void Update()
    {
        if (follow)
        {
            Vector3 delta = follow.position - main.ViewportToWorldPoint(new Vector3(xOffset, yOffset, transform.position.z));
            destination = transform.position + delta;
            destination.z = transform.position.z;

            if (deadzonesEnabled)
            {
                destination = transform.position;

                if (delta.x > deadzoneBox.x)
                {
                    destination.x = follow.position.x - deadzoneBox.x;
                }
                if (delta.x < -deadzoneBox.x)
                {
                    destination.x = follow.position.x + deadzoneBox.x;
                }
                if (delta.y > deadzoneBox.y)
                {
                    destination.y = follow.position.y - deadzoneBox.y;
                }
                if (delta.y < -deadzoneBox.y)
                {
                    destination.y = follow.position.y + deadzoneBox.y;
                }
            }

            if (boundsEnabled)
            {
                if (boundType == Axis.Horizontal || boundType == Axis.Both)
                {
                    destination.x = Mathf.Clamp(destination.x, boundsMin.x, boundsMax.x);
                }
                if (boundType == Axis.Vertical || boundType == Axis.Both)
                {
                    destination.y = Mathf.Clamp(destination.y, boundsMin.y, boundsMax.y);
                }
            }

            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(destination.x, destination.y, destination.z), ref velocity, damp);
        }
    }

    public void SnapToDestination()
    {
        destination.z = transform.position.z;
        transform.position = destination;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    // Awake
    public static CameraController i;
    void Awake()
    {
        i = this;
    }

    // Move
    private float orthSize;
    private float aspectSize;
    public void Move(Vector3 newPos)
    {
        // Get
        orthSize = Camera.main.orthographicSize;
        aspectSize = Camera.main.aspect;

        // Reposition
        /*
        transform.position = new Vector3(
            Mathf.Clamp(newPos.x, MapLoader.i.minPos.x+(aspectSize*orthSize), MapLoader.i.maxPos.x-(aspectSize*orthSize)),
            Mathf.Clamp(newPos.y, MapLoader.i.maxPos.y+orthSize, MapLoader.i.minPos.y-orthSize),
            0
        );
        */

        // Reposition
        transform.position = new Vector3(
            Mathf.Clamp(Mathf.Lerp(transform.position.x, newPos.x, 4f * Time.deltaTime), MapLoader.i.minPos.x+(aspectSize*orthSize), MapLoader.i.maxPos.x-(aspectSize*orthSize)),
            Mathf.Clamp(Mathf.Lerp(transform.position.y, newPos.y, 4f * Time.deltaTime), MapLoader.i.maxPos.y+orthSize, MapLoader.i.minPos.y-orthSize),
            0
        );
    }

    // Late Update
    private bool camDragging = false;
    private Vector3 camOrigin;
    private Vector3 camDifference;
    void LateUpdate()
    {
        // Input
        if (Input.GetMouseButton(0))
        {
            camDifference = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position);
            if (!camDragging)
            {
                camOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                camDragging = true;
            }
        }
        else
        {
            camDragging = false;
        }

        // Moving
        Move((camOrigin - camDifference));
        // Moving
        //if (camDragging) Move((camOrigin - camDifference));
    }
}
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

    // Start
    private float targetZoom;
    void Start()
    {
        targetZoom = Camera.main.orthographicSize;
    }

    // Move
    private float camOrthSize;
    private float camAspectSize;
    [HideInInspector] public bool camActive = true;
    public void Move(Vector3 newPos)
    {
        // Get
        camOrthSize = Camera.main.orthographicSize;
        camAspectSize = Camera.main.aspect;

        // Return
        if (!camActive) return;

        // Position
        float xLerp = Mathf.Lerp(transform.position.x, newPos.x, 4f * Time.deltaTime);
        float yLerp = Mathf.Lerp(transform.position.y, newPos.y, 4f * Time.deltaTime);

        // Reposition
        Reposition(new Vector3(xLerp, yLerp, 0f));
    }

    // Zoom
    private float zoomSpeed = 2f;
    private float zoomSmooth = 2f;
    public void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f) {
            targetZoom -= scroll * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, 1f, 4f);
        }

        // Resize
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetZoom, zoomSmooth * Time.deltaTime);
        Reposition(transform.position);
    }

    // Reposition
    void Reposition(Vector3 newPos)
    {
        // Repositions and clamps view
        transform.position = new Vector3(
            Mathf.Clamp(newPos.x, MapLoader.i.minPos.x+(camAspectSize*camOrthSize), MapLoader.i.maxPos.x-(camAspectSize*camOrthSize)),
            Mathf.Clamp(newPos.y, MapLoader.i.maxPos.y+camOrthSize, MapLoader.i.minPos.y-camOrthSize),
            newPos.z);
    }

    // Late Update
    private bool camDragging = false;
    private Vector3 camOrigin;
    private Vector3 camDifference;
    void LateUpdate()
    {
        // Return
        if (!camActive) return;

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
        Zoom();
        Move((camOrigin - camDifference));
    }

    // Pinpoint image
    public Vector3 pinpointPos; // Can be customized from inspector for different points.
    public string PinPoint(Vector3 targetPos)
    {
        // Calculate
        Vector3 camPos = transform.position;
        float camWidth = (Camera.main.orthographicSize) * Camera.main.aspect;
        float camHeight = Camera.main.orthographicSize;

        // No need for ScreenToWorldPoint
        Vector3 worldPos = camPos + new Vector3((targetPos.x * camWidth), (targetPos.y * camHeight), 0f);

        // Picked
        GameObject closeObject = GetClosestObject(worldPos, GameObject.FindGameObjectsWithTag("Tile"));

        // Returning
        string objectName = closeObject.GetComponent<Image>().sprite.name;
        return objectName;
    }

    // Finds nearest object to pos
    GameObject GetClosestObject(Vector3 pos, GameObject[] objects)
    {
        GameObject bestPick = null;
        float minDistance = Mathf.Infinity;
        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(obj.transform.position, pos);
            if (distance < minDistance)
            {
                bestPick = obj;
                minDistance = distance;
            }
        }
        return bestPick;
    }

    // Enables/disables for UnityEvents
    public void Enable(bool enabled)
    {
        camActive = enabled;

        PinPoint(pinpointPos);
    }
}
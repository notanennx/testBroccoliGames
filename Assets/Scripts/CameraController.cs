using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Awake
    public static CameraController i;
    void Awake()
    {
        i = this;
    }

    // Start
    private float aspect;
    private float orthSize;
    void Start()
    {
        aspect = Camera.main.aspect;
        orthSize = Camera.main.orthographicSize;

        print("aspect: "+aspect);
        print("orthSize: "+orthSize);
        print("calculated:"+(aspect*orthSize));
    }

    // Move
    public void Move(float targetX, float targetY)
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + targetX, MapLoader.i.minPos.x+(aspect*orthSize), MapLoader.i.maxPos.x-(aspect*orthSize)),
            Mathf.Clamp(transform.position.y + targetY, MapLoader.i.maxPos.y+orthSize, MapLoader.i.minPos.y-orthSize),
            0
        );
    }

    // Update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) Move(0f, 1f);
        if (Input.GetKeyDown(KeyCode.S)) Move(0f, -1f);

        if (Input.GetKeyDown(KeyCode.A)) Move(-1f, 0);
        if (Input.GetKeyDown(KeyCode.D)) Move(1f, 0);

        //print("aspect: "+aspect+"; orthSize:"+orthSize+"; w&h: "+Camera.main.pixelWidth+","+Camera.main.pixelHeight);
        //print("aspect: "+aspect+"; orthSize:"+orthSize+"; custom: "+Camera.main.pixelWidth/Camera.main.pixelHeight);
    }
}

/*
public float speed = 2f;
public Transform min;
public Transform max;
private float aspect;
private float maxSize;

private void Start () 
{
    this.aspect = Camera.main.aspect;
    this.maxSize = max.position.x <= max.position.y ? max.position.x /2f / this.aspect :max.position.y / 2f;
}

private void Update () 
{
    float size = Input.GetAxis ("Mouse ScrollWheel");
    Camera.main.orthographicSize += size;
    if (Camera.main.orthographicSize > maxSize) 
    {
        Camera.main.orthographicSize = maxSize;
    }

    float x = Input.GetAxis ("Horizontal");
    float y = Input.GetAxis ("Vertical");

    Vector3 position = this.transform.position;
    position.x += x * Time.deltaTime * this.speed;
    position.y += y * Time.deltaTime * this.speed;
    float orthSize = Camera.main.orthographicSize;

    if (position.x < (this.min.position.x + orthSize * this.aspect)) 
    {
        position.x = this.min.position.x + orthSize * this.aspect;
    } 
    else if (position.x > (this.max.position.x - orthSize * this.aspect)) 
    {
        position.x = this.max.position.x - orthSize * this.aspect;
    }
    if (position.y < (this.min.position.y + orthSize))
    {
        position.y = this.min.position.y + orthSize;
    }
    else if(position.y > (this.max.position.y - orthSize)) 
    {
        position.y = this.max.position.y - orthSize;
    }
    this.transform.position = position;
}
*/
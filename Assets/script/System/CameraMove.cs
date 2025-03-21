using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player; // ให้กำหนดค่าตำแหน่งของ Player ใน Inspector
    public Transform leftBoundary; // ขอบเขตซ้ายของ Background
    public Transform rightBoundary; // ขอบเขตขวาของ Background
    public Transform topBoundary; // ขอบเขตซ้ายของ Background
    public Transform downBoundary; // ขอบเขตขวาของ Background

    public float smoothSpeed = 0.125f; // ความเร็วในการขยับของกล้อง
    public Vector3 offset; // ตำแหน่ง Offset ของกล้อง

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void LateUpdate()
    {
    float desiredX = player.position.x + offset.x;
    float clampedX = Mathf.Clamp(desiredX, leftBoundary.position.x, rightBoundary.position.x);

    float desiredY = player.position.y + offset.y;
    float clampedY = Mathf.Clamp(desiredY, downBoundary.position.y, topBoundary.position.y);

    Vector3 desiredPosition = new Vector3(clampedX, clampedY, transform.position.z);
    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    transform.position = smoothedPosition;


        /*Vector2 desiredPosition = (Vector2)target.position + offset;
        Vector2 smoothedPosition = Vector2.Lerp((Vector2)transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, transform.position.y, transform.position.z);
        //transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);*/
    }
}

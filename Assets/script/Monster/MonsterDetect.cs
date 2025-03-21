using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDetect : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [Header("Detect")]
    [SerializeField]
    private Vector3 sightAngle;
    [SerializeField]
    private float sightRange;
    [SerializeField]
    private Vector3 sightAngleInside;
    [SerializeField]
    private float sightRangeInside;
    private bool isOnPlayer;
    private Vector2 startRaycast;
    [SerializeField]
    private float rayDistance;
    public Vector2 lineOfSight;
    private bool hitPlayer;
    private bool hitFloor;
    private bool hitPlayerI;
    private bool hitFloorI;
    private bool donthitPlayerO;
    private bool isDetectPlayer;
    private bool dontseePlayer;
    bool isDetectPlayerInSide;

    [Header("Detect02")]
    private float circleDetectRadius;
    private float detectionTime;
    private float detectionTime02;
    private bool inDetectionZone;

    [Header("Layer Mask")]
    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private LayerMask floorLayerMask;
    [SerializeField]
    private LayerMask PlayerMask;
    [SerializeField]
    public LayerMask targetLayer;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectObject();
    }

        private void OnDrawGizmos(){
        Gizmos.color = isDetectPlayer ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + sightAngle, sightRange);

        Gizmos.color = isDetectPlayerInSide ? Color.green :Color.red;
        Gizmos.DrawWireSphere(transform.position + sightAngleInside, sightRangeInside);

        /*Gizmos.color = isOnPlayer ? Color.yellow : Color.red;
        Gizmos.DrawWireCube(transform.position + point1 , point2);
        Collider2D[] collidersInArea = Physics2D.OverlapAreaAll(point1, point2);*/

        Gizmos.color = Color.black;
        if (hitPlayerI)
        {
            Gizmos.color = Color.green;
        }
        else if (hitPlayer && hitFloor && hitFloorI)
        {
            Gizmos.color = Color.red;
        }
        else if (hitPlayer && hitFloor && !hitFloorI)
        {
            Gizmos.color = Color.green;
        }
        else if (hitPlayer && !hitFloor)
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawLine(startRaycast ,startRaycast + lineOfSight * rayDistance); // วาดเส้นในกรณีที่ไม่มีการชน

        //Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);


    }

    public void DetectObject()
    {
        startRaycast = transform.position; // ตำแหน่งเริ่มต้นของ Raycast
        if (spriteRenderer.flipX == true)
        {
            lineOfSight = Vector2.right;
        }
        else
        {
            lineOfSight = Vector2.left;
        }

        RaycastHit2D hitRayCastF = Physics2D.Raycast(startRaycast, lineOfSight, rayDistance, targetLayer);
        hitFloor = hitRayCastF;

        RaycastHit2D hitRayCastP = Physics2D.Raycast(startRaycast, lineOfSight, rayDistance, PlayerMask);
        hitPlayer = hitRayCastP;

        RaycastHit2D hitRayCastY = Physics2D.Raycast(hitRayCastP.point, lineOfSight - hitRayCastP.point, rayDistance, targetLayer);
        hitFloorI = hitRayCastY;

        Collider2D DetectColider = Physics2D.OverlapCircle(transform.position + sightAngle, sightRange, PlayerMask);
        isDetectPlayer = DetectColider;

        Collider2D DetectColiderInSide = Physics2D.OverlapCircle(transform.position + sightAngleInside, sightRangeInside, PlayerMask);
        isDetectPlayerInSide = DetectColiderInSide;
        
        // เพิ่มเวลาที่ detect

        // ทำสิ่งที่ต้องการ เช่น รีเซ็ตการนับเวลาหรือส่งอีเวนต์ต่าง ๆ
        if (DetectColiderInSide != null)
        {
            //ChangeState(CharacterState.Attack);
        }
        
        // เพิ่มเวลาที่ detect

        // ทำสิ่งที่ต้องการ เช่น รีเซ็ตการนับเวลาหรือส่งอีเวนต์ต่าง ๆ

        if (hitRayCastF.collider == null && hitRayCastP.collider == null)
        {
            hitPlayerI = false;
            hitFloorI = false;
        }
        else if (hitRayCastP.collider != null && hitRayCastF.collider == null)
        {
            /*if (Attackstate == true)
            {
            }
            else if(Idlestate == true && Attackstate == false)
            {
                ChangeState(CharacterState.Suspect);
            }*/
            
            if(hitRayCastP.distance <= sightRange)
            {
                detectionTime += Time.deltaTime;
            }       
        }
        else if (hitRayCastF.collider != null && hitRayCastP.collider != null && hitRayCastY.collider == null)
        {
            //hitRayCastP.point, lineOfSight - hitRayCastP.point กำหนดระยะระหว่างทาง

            /*if(Idlestate == true && Attackstate == false)
            {
                ChangeState(CharacterState.Suspect);
            }*/

            if(hitRayCastP.distance <= sightRange)
            {
                detectionTime += Time.deltaTime;
            }
        }
        else{
            // รีเซ็ตเวลาที่ detect ถ้าอยู่นอกระยะ detect
            //detectionTime = 0f;
        }

        if (DetectColider != null)
        {
            inDetectionZone = true;
        }
        else if(DetectColider == null)
        {
            inDetectionZone = false;
            detectionTime = 0f;
            detectionTime02 = 0f;
        }

        if (inDetectionZone == true)
        {
            detectionTime02 += Time.deltaTime;  
        }
    }

}

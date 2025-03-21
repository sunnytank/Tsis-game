using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
[Header("REF")]
[SerializeField] 
private Transform PlayerTranform;


[Header("Filp Rotation Stats")]
[SerializeField] private float flipYRotationTime = 0.5f;

private Coroutine trunCoroutine;
private Playermovement playermovement;
private bool isFacingRight;


private void Awake()
{
    playermovement = PlayerTranform.GetComponent<Playermovement>();

    isFacingRight = playermovement.isFacingRight;
}

void Update()
{

    transform.position = PlayerTranform.position;
    //PlayerTranform.position = transform.position;
}

public void CallTurn()
{
    //trunCoroutine = StartCoroutine(FlipYLerp());
    LeanTween.rotateY(gameObject, DetermineEndRotaion() , flipYRotationTime).setEaseInOutSine();
}

/*private IEnumerator FlipYLerp()
{
    float startRotation = transform.localEulerAngles.y;
    float endRotationAmount = DetermineEndRotaion();
    float yRotation = 0f;

    float elapsedTime = 0f;
    while(elapsedTime < flipYRotationTime)
    {
        elapsedTime += Time.deltaTime;

        yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
        transform.rotation = Quaternion.Euler(0f, yRotation , 0f);

        yield return null;
    }
}*/

private float DetermineEndRotaion()
{
    isFacingRight = !isFacingRight;
    if(isFacingRight)
    {
        return 180f;
    }
    else{
        return 0f;
    }


}

    
}

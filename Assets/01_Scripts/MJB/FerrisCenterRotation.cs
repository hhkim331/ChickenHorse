using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisCenterRotation : MonoBehaviour
{
    //회전 속도
    public float speed = 90f;

    private void Update()
    {
        //나를 z축으로 회전시킨다.
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
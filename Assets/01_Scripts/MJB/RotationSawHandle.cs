using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//나 자신을 z축으로 회전하자
public class RotationSawHandle : MonoBehaviour
{
    //회전 속도
    public float speed = 90f;

    private void Update()
    {
        //회전
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
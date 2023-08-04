using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{

    //플레이어를 이동시키고 싶다.
    private float speed = 10f;
    public Rigidbody rigid;

    void Update()
    {

        Move();
        Jump();
    }

    private void Jump()
    {
        // 점프 버튼 누르면 점프
        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector3.up * 8, ForceMode.Impulse);
        }
    }

    private void Move()
    {
        //플레이어를 x, y축으로 이동시키고싶다.
        float hAxis = Input.GetAxis("Horizontal");

        //왼.오른쪽 설정
        Vector3 dir = transform.right * hAxis;

        rigid.AddForce(dir * speed * Time.deltaTime, ForceMode.Impulse);
    }
}

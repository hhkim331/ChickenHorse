using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{

    //�÷��̾ �̵���Ű�� �ʹ�.
    private float speed = 5f;
    public Rigidbody rigid;

    void Update()
    {

        Move();
        Jump();
    }

    private void Jump()
    {
        // ���� ��ư ������ ����
        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector3.up * 8, ForceMode.Impulse);
        }
    }

    private void Move()
    {
        //�÷��̾ x, y������ �̵���Ű��ʹ�.
        float hAxis = Input.GetAxis("Horizontal");

        //��.������ ����
        Vector3 dir = transform.right * hAxis;

        rigid.AddForce(dir * speed * Time.deltaTime, ForceMode.Impulse);
    }
}

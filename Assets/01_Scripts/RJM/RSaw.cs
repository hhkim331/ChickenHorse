﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class RSaw : ObjectScript
{
    public float speed = 10;
    public float minX = -11f; // X축 이동 제한
    public float maxX = 11f;  // X축 이동 제한 

    private Vector3 movement;
    private bool movingRight = true;

    public float rotateTime = 2.0f;
    public float timer = 0.0f;

    //회전횟수
    public int zCount = 0;

    //z의 각도
    public float zAngle;

    public bool rotateStart = false;

    private Vector3 rotationSpeed = new Vector3(0, 0, 360); // 회전 속도

    //오브젝트 소유자
    public PhotonView photonView;
    //// Start is called before the first frame update
    //void Start()
    //{
    //}

    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        float h = movingRight ? 1 : -1; // 이동 방향에 따라 속도의 부호 설정
        movement = Vector3.right * h;
        Vector3 velocity = movement * speed;
        transform.localPosition += velocity * Time.deltaTime;

        // X축 이동 제한 적용
        Vector3 clampedPosition = transform.localPosition;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.localPosition = clampedPosition;

        // 이동 방향 전환 체크
        if (clampedPosition.x >= maxX || clampedPosition.x <= minX)
        {
            movingRight = !movingRight; // 이동 방향을 반대로 전환
        }

        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponentInParent<KHHPlayerMain>().Hit(photonView.Owner.ActorNumber);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!active) return;

        if (collision.gameObject.tag == "Player")
        {
            print("플레이어맞음");
        }
    }

    public override void ResetObject()
    {
        transform.localPosition = new Vector3(-2.4f, 0.55f, 0f);
    }
}



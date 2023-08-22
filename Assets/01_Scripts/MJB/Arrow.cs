﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // 화살을 날리고 싶다.
    // 속력
    public float speed = 5f;

    // 화살 애니메이터를 가져온다.
    public Animator anim;

    private void Update()
    {
        // 위의 방향으로
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            other.transform.root.GetComponent<KHHPlayerMain>().Hit();
            //나의 캡슐 콜라이더를 비활성화 시킨다.
            GetComponent<CapsuleCollider>().enabled = false;
        }

        // 부셔지는 애니메이션을 실행시킨다.
        anim.SetTrigger("ArrowCrashing");
    }
}
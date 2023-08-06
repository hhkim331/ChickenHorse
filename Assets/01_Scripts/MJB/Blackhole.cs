﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    // 블랙홀 영역에 플레이어 태그가 들어오면
    // 물리 영향을 받지 않게 한다.
    // 1초 뒤에 삭제시킨다.
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.attachedRigidbody.isKinematic = true;
            Destroy(other.gameObject, 1f);
        }
    }
}
﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    //오브젝트 소유자
    public PhotonView photonView;
    // 블랙홀 영역에 플레이어 태그가 들어오면
    // 물리 영향을 받지 않게 한다.
    // 1초 뒤에 삭제시킨다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.GetComponentInParent<KHHPlayerMain>().Hit(photonView.Owner.ActorNumber);
            //other.attachedRigidbody.isKinematic = true;W
            //Destroy(other.gameObject, 1f);
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barbWire : MonoBehaviour
{
    //오브젝트 소유자
    public PhotonView photonView;
    //wire에 플레이어가 태그가 되면 0.5초 뒤에 삭제시킨다.
    void OnCollisionEnter(Collision collision)
    {
        //layer가 player이면
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponentInParent<KHHPlayerMain>().Hit(photonView.Owner.ActorNumber);
            ////플레이어의 물리 현상을 정지한다.
            //collision.rigidbody.isKinematic = true;
            //Destroy(collision.gameObject, 1f);
        }
    }
}

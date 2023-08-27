using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTireTrigger : ObjectScript
{
    private PhotonView player;

    private void Update()
    {
        if (player == null) return;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player != null) return;
        //플레이어 태그를 만나면
        if (other.CompareTag("PlayerFoot"))
        {
            player = other.gameObject.GetComponentInParent<PhotonView>();
            if (!player.IsMine)
            {
                player = null;
                return;
            }
            else
            {
                player.transform.parent = transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (player == null) return;
        //플레이어 태그가 나간다면
        if (other.CompareTag("PlayerFoot"))
        {
            if (player == other.gameObject.GetComponentInParent<PhotonView>())
            {
                //플레이어의 transform을 null로 만든다.
                player.transform.parent = null;
                player = null;
            }
        }
    }

    public override void ResetObject()
    {
    }
}

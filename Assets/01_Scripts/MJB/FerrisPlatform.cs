using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//나의 부모, 부모, 부모의 z회전 값을 가져와서 나의 회전 값에 더해준다.
//플레이어가 triggerenter 되었을 때 나의 부모의 부모로 놓고, exit 되었을 때 부모를 null한다.
public class FerrisPlatform : ObjectScript
{
    private PhotonView player;

    private void Update()
    {
        if (!active) return;
        transform.Rotate(0, 0, -transform.GetComponentInParent<FerrisCenterRotation>().speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player != null) return;
        //플레이어 태그를 만나면
        if (other.CompareTag("PlayerFoot"))
        {
            //플레이어 transform은 종이 비행기의 tranform으로 한다.
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
        transform.rotation = Quaternion.identity;
    }
}
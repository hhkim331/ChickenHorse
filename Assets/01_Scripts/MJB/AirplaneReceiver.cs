using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneReceiver : MonoBehaviour
{
    public GameObject player;

    //boxCollider 두개를 모두 담을 변수
    public BoxCollider[] airPlaneCollider;

    public void StopMove()
    {
        //boxCollider들을 비활성화 한다.
        foreach (var boxCollider in airPlaneCollider)
        {
            boxCollider.enabled = false;
        }
    }

    public void DestroyAirplane()
    {
        //플레이어의 부모가 있을때만
        if (player.transform.parent != null)
        {
            // 부모를 해채시킨다.
            player.transform.parent = null;
        }
        else if (player.transform.parent == null)
        {
            //부모의 부모까지 다 삭제시킨다.
            ObjectPool.DisabledAirplane(transform.parent.parent.gameObject);
        }
    }

}
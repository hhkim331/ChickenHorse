using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneReceiver : MonoBehaviour
{
    public GameObject player;

    //삭제시키는 이벤트 함수를 만든다.,
    public void DestroyAirplane()
    {
        //플레이어의 부모가 있을때만
        if (player.transform.parent != null)
        {
            // 부모를 해채시킨다.
            player.transform.parent = null;
        }
        //부모의 부모까지 다 삭제시킨다.
        Destroy(transform.parent.parent.gameObject);
    }
}
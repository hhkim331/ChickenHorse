using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneReceiver : MonoBehaviour
{
    //삭제시키는 이벤트 함수를 만든다.,
    public void DestroyAirplane()
    {
        //부모의 부모까지 다 삭제시킨다.
        Destroy(transform.parent.parent.gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barbWire : MonoBehaviour
{
    //wire에 플레이어가 태그가 되면 0.5초 뒤에 삭제시킨다.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //플레이어의 물리 현상을 정지한다.
            collision.rigidbody.isKinematic = true;
            Destroy(collision.gameObject, 1f);
        }
    }
}

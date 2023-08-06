using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // 화살을 날리고 싶다.
    // 속력
    public float speed = 5f;
    void Update()
    {
        // 위의 방향으로
        transform.position += transform.up * speed * Time.deltaTime;
    }
    //충돌하려는 물체가
    void OnCollisionEnter(Collision collision)
    {
        //플레이어에게 충돌했을 때만 삭제시킨다.
        if (collision.gameObject.CompareTag("Player"))
        {
            //이 컴포넌트를 가지고 있는 오브젝트의 부모까지 삭제시킨다.
            Destroy(transform.parent.gameObject);
            // 부딪힌 물체의 오브젝트를 삭제한다.
            Destroy(collision.gameObject);
        }
        //만약 다른 물체에 충돌했다면 나를 삭제시킨다.
        else
        {
            //이 컴포넌트를 가지고 있는 오브젝트의 부모까지 삭제시킨다.
            Destroy(transform.parent.gameObject);
        }
    }
}

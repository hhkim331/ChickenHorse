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
        Destroy(gameObject);
        Destroy(collision.gameObject);
    }
}

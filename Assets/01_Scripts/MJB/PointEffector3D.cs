using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//블랙홀을 제작해보자
public class PointEffector3D : MonoBehaviour
{
    //구의 지름
    public float radius = 5f;

    //구의 속력
    public float force = 10f;

    private void Update()
    {
        //구의 현재 위치를 가져온다.
        Vector3 position = transform.position;

        //구의 충돌 위치 배열을 만든다.
        Collider[] colliders = Physics.OverlapSphere(position, radius);

        //배열을 순회하면서
        foreach (Collider hit in colliders)
        {
            //리지드바디를 가져온다.
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            //리지드바디가 있다면
            if (rb != null)
            {
                //리지드바디에 힘을 가한다.
                rb.AddExplosionForce(force, position, radius);
            }
        }
    }
}
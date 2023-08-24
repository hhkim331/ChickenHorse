using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 트리거 되었을 때 hit을 부른다.
//나의 하위 게임오브젝트 z축을 돌리고싶다.
public class SpinSaw : MonoBehaviour
{
    //회전 속도
    private float speed = 360f;

    private void Update()
    {
        //나의 하위 게임오브젝트 z축을 돌린다.
        transform.GetChild(0).Rotate(0, 0, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //플레이어가 트리거 되었을 때
        if (other.CompareTag("Player"))
        {
            //플레이어의 피격 함수를 호출한다.
            other.transform.GetComponentInParent<KHHPlayerMain>().Hit();
        }
    }
}
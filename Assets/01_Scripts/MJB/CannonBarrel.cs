using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

//플레이어가 캐논에 들어가면 오른쪽 45도 각도로 플레이어를 날리고 싶다.
public class CannonBarrel : MonoBehaviour
{
    //캐논의 발사 위치
    public Transform fireTransform;

    //플레이어 오브젝트를 담는 변수
    private GameObject player;

    //변수를 담을 애니메이터
    public Animator fireAnimator;

    public GameObject SmokeAnimator;

    //발사하고 있다는 bool 변수
    public bool isFiring = false;

    //발사 힘
    public float force = 10f;

    public void FirePlayer()
    {
        if (isFiring)
        {
            player.SetActive(true);
            //플레이어의 리지드 바디에 폭발적인 힘을 준다.
            player.GetComponent<Rigidbody>().AddForce(fireTransform.right * force, ForceMode.Impulse);
            //smokeAnimator를 활성화 하자
            SmokeAnimator.SetActive(true);
            //발사 끝
            isFiring = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //플레이어가 캐논에 닿으면 플레이어를 오브젝트를 담는다.
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isFiring)
        {
            //발사가 가능함
            isFiring = true;
            //최상단 부모를 넣는 이유는 플레이어의 boxCollider가 자식에 존재하기 때문에 그의 부모를 넣어야 한다. 자식의 boxCollider를 날리는 것이 아니다.
            player = other.transform.parent.gameObject;
            //플레이어의 최상단 부모를 비활성화한다.
            player.SetActive(false);
            //플레이어의 위치를 발사 위치로 이동시킨다.
            player.transform.position = fireTransform.position;
            fireAnimator.SetTrigger("Fire");
            //smokeAnimator를 비활성화 하자
            SmokeAnimator.SetActive(false);
        }
    }
}
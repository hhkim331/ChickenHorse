using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCoin : MonoBehaviour
{
    //플레이어와 충돌했을때 애니메이터의 trigger 애니메이션을 발생시킨다.
    public Animator anim;

    private void OnCollisionEnter(Collision collision)
    {
        //플레이어와 부딪혔다면 나를 삭제시킨다.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            anim.SetTrigger("Crash");
        }
    }
}
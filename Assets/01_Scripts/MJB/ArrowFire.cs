using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFire : MonoBehaviour
{
    // 화살 공장을 만들어서 발사한다.
    // 발사 공장
    public GameObject arrowFactory;
    // 발사 위치
    public Transform fireTransform;
    void FireArrow()
    {
        Debug.Log("FireArrow");
        // 화살 발사 공장을 가동한다.
        GameObject arrow = Instantiate(arrowFactory);
        // 발사 물체를 발사 위치에 놓는다.
        arrow.transform.position = fireTransform.position;
        // 발사 물체의 위 방향을 발사 위치의 위 방향이다.
        arrow.transform.up = fireTransform.up;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//종이를 발사시키고 싶다.
public class ArrowShooter : MonoBehaviour
{
    // 화살 공장을 만들어서 발사한다.
    // 발사 공장
    public GameObject arrowFactory;

    // 발사 위치
    public Transform arrowFireTransform;

    //오브젝트 생성 가능확인을 위한 StageObject
    public StageObject stageObject;

    public void FireArrowReady()
    {
        SoundManager.Instance.PlaySFX("ArrowFire");
    }

    public void FireArrow()
    {
        // 화살 발사 공장을 가동한다.
        GameObject arrow = Instantiate(arrowFactory);
        // 발사 물체를 발사 위치에 놓는다.
        arrow.transform.position = arrowFireTransform.position;
        // 발사 물체의 위 방향을 발사 위치의 위 방향이다.
        arrow.transform.up = arrowFireTransform.up;
        //SoundManager.Instance.PlaySFX("ArrowFire");
        Destroy(arrow, 10);
        if (!stageObject.IsPlay)
        {
            foreach (var transform in arrow.GetComponentsInChildren<Transform>())
            {
                transform.gameObject.layer = LayerMask.NameToLayer("PartyBox");
            }
            Destroy(arrow, 0.5f);
        }
    }
}
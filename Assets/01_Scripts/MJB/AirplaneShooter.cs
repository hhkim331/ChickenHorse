using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneShooter : MonoBehaviour
{
    // 종이 발사 공장을 만들어서 발사한다.
    public GameObject airPlaneFactory;

    // 발사 위치
    public Transform airPlaneFireTransform;

    //오브젝트 생성 가능확인을 위한 StageObject
    public StageObject stageObject;

    private void FireAirplaneReady()
    {
        SoundManager.Instance.PlaySFX("AirplaneReady");
    }

    private void FireAirplane()
    {
        //종이 발사 공장을 가동한다.
        GameObject airplane = ObjectPool.GetAirplane();
        // 발사 물체를 발사 위치에 놓는다.
        airplane.transform.position = airPlaneFireTransform.position;
        // 발사 물체의 옆 방향을 발사 위치의 옆 방향이다.
        airplane.transform.right = airPlaneFireTransform.right;
        //SoundManager.Instance.PlaySFX("AirplaneFire");

        StartCoroutine(IEDisableAirplane(airplane));

        if (!stageObject.IsPlay)
        {
            foreach (var transform in airplane.GetComponentsInChildren<Transform>())
            {
                transform.gameObject.layer = LayerMask.NameToLayer("PartyBox");
            }

            airplane.transform.parent = stageObject.transform.parent;
            // 발사 물체를 발사 위치에 놓는다.
            airplane.transform.position = airPlaneFireTransform.position;
            // 발사 물체의 옆 방향을 발사 위치의 옆 방향이다.
            airplane.transform.right = airPlaneFireTransform.right;

            airplane.transform.position += Vector3.back * 2;

            Destroy(airplane, 0.5f);
        }
        else
        {
            // 발사 물체를 발사 위치에 놓는다.
            airplane.transform.position = airPlaneFireTransform.position;
            // 발사 물체의 옆 방향을 발사 위치의 옆 방향이다.
            airplane.transform.right = airPlaneFireTransform.right;
        }
    }


    IEnumerator IEDisableAirplane(GameObject airplane)
    {
        yield return new WaitForSeconds(5f);
        //종이 비행기를 초기 위치로 놓자
        ObjectPool.DisabledAirplane(airplane);
    }

}
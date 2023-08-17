using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultipleTargetCamera : MonoBehaviourPun
{
    //위치 리스트가 필요하다.
    public List<Transform> targets;

    //위치값을 조정하고 싶다.
    public Vector2 offset;

    //fov 최소, 최댓값
    public float maxZoom = 12f, minZoom = 7f, limitZoom = 50f;

    //해당 카메라를 담을 변수 생성
    private CinemachineVirtualCamera virtualCam;

    private void Awake()
    {
        virtualCam = this.GetComponent<CinemachineVirtualCamera>();
    }

    // 타겟의 중심점을 가져온다.
    private void LateUpdate()
    {
        //타겟이 없을때는 함수를 빠져나간다.
        if (targets.Count == 0) return;

        Move();
        Zoom();
    }

    private void Move()
    {
        Vector2 centerPosition = GetCenterPosition();

        Vector2 newPosition = centerPosition + offset;
        //카메라의 중심을 부드럽게 이동을 지정한다.
        transform.position = newPosition;
    }

    private void Zoom()
    {
        //카메라 줌 값을 선형 보간한다.
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetGreatestPosition() / limitZoom);
        //그리고 메인 카메라의 보는 값과 타겟 줌 값을 선형 보간한다.
        virtualCam.m_Lens.OrthographicSize = Mathf.Lerp(virtualCam.m_Lens.OrthographicSize, newZoom, Time.deltaTime);
    }

    private float GetGreatestPosition()
    {
        //bound로 거리를 만든다
        var bounds = new Bounds(targets[0].position, Vector2.zero);
        //모든 물체를 순회하여 최대 거리값을 만든다.
        for (int i = 0; i < targets.Count; i++)
        {
            //물체를 넣어서 경계를 생성한다.
            bounds.Encapsulate(targets[i].position);
        }
        // 플레이어간의 x거리를 반환한다.
        return bounds.size.x;
    }

    private Vector2 GetCenterPosition()
    {
        //list에 하나만 있다면 처음 타겟의 위치 값을 가져오고
        if (targets.Count == 1) return targets[0].position;

        //bound를 이용하여 첫 타겟의 중심점을 마우스 위치 가져온다.
        var bounds = new Bounds(targets[0].position, Vector2.zero);
        //그게 아니면 리스트를 모두 순회하여
        for (int i = 0; i < targets.Count; i++)
        {
            // 새로운 오브젝트를 넣어 중심 값을 반환한다.
            bounds.Encapsulate(targets[i].position);
        }
        // 중심값을 반환한다.

        return bounds.center;
    }
}
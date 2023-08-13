﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneShooter : MonoBehaviour
{
    // 종이 발사 공장을 만들어서 발사한다.
    public GameObject airPlaneFactory;

    // 발사 위치
    public Transform airPlaneFireTransform;

    private void FireAirplane()
    {
        //종이 발사 공장을 가동한다.
        GameObject airplane = Instantiate(airPlaneFactory);
        // 발사 물체를 발사 위치에 놓는다.
        airplane.transform.position = airPlaneFireTransform.position;
        // 발사 물체의 옆 방향을 발사 위치의 옆 방향이다.
        airplane.transform.right = airPlaneFireTransform.right;
    }
}
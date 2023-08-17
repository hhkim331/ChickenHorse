using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RTestPlayerCursor : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //내가 만든 것만 동작하게
        if(photonView.IsMine)
        {
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x < 100) mousePos.x = 100;
            if (mousePos.x > 1920) mousePos.x = 1920;
            if (mousePos.y < 100) mousePos.y = 100;
            if (mousePos.y > 1080) mousePos.y = 1080;
            mousePos.z = -Camera.main.transform.position.z;
            //마우스 화면 좌표를 3D 공간 좌표로 변환
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            //나의 위치를 worldPos 로 설정        
            transform.position = worldPos;

        }
    }
}

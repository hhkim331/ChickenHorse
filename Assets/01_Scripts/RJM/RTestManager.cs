using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RTestManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.SerializationRate = 60;
        //나의 커서를 생성(다른 PC 에서도 보이게)
        PhotonNetwork.Instantiate("TestPlayerCursor", Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
